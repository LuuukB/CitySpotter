using CitySpotter.Domain.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Mopups.Services;
using CitySpotter.Domain.Services.Internet;
using System.Timers;
using Microsoft.Maui.Devices.Sensors;
using System.Net.NetworkInformation;
using CitySpotter.Domain.Services.FileServices;


namespace CitySpotter.Domain.Services;

public partial class MapViewModel : ObservableObject
{
    [ObservableProperty] private string _routeName;
    [ObservableProperty] private string _imageSource = "nointernetpopupscreen.jpg";
    [ObservableProperty] private ObservableCollection<MapElement> _mapElements = [];
    [ObservableProperty] private MapSpan _currentMapSpan;
    [ObservableProperty] private ObservableCollection<Pin> _pins = [];
    [ObservableProperty] private bool _RouteIsPaused = false;

    private Dictionary<Pin, Circle> _pinsAndCirkles = [];
    private System.Timers.Timer _locationTimer;
    public event EventHandler<string> InternetConnectionLost;
    public event EventHandler<string> LocationLost;

    private readonly IGeolocation _geolocation;
    private readonly IDatabaseRepo _databaseRepo;
    private readonly IInternetHandler _internetHandler;
    private bool _isShowingNetwerkError = true;
    private bool _isShowingLocationError = true;
    private bool _pause = false;

    public bool HasInternetConnection
    {
        get
        {
            //Debug.WriteLine("Checking Internet connection");
            return _internetHandler.HasInternetConnection();
        }
    }

    public MapViewModel(IGeolocation geolocation, IDatabaseRepo repository, IInternetHandler internetHandler)
    {
        _geolocation = geolocation;
        _databaseRepo = repository;
        _internetHandler = internetHandler;

        ZoomToBreda();
    }

    public void RouteStarting()
    {
        Debug.WriteLine("starting route/timer");
        _locationTimer = new System.Timers.Timer(2000);
        _locationTimer.Elapsed += OnTimedEvent;
        _locationTimer.AutoReset = true;
        _locationTimer.Start();

    }

    [RelayCommand]
    public void PauseRoute() {
        RouteIsPaused = true;
        _pause = true;
    }
    [RelayCommand]
    public void ContinueRoute()
    {
        _pause = false;
        RouteIsPaused = false;
    }

    [RelayCommand]
    public void StopRoute()
    {
        _pause = false;
        RouteIsPaused = false;

        if (_locationTimer != null)
        {
            _locationTimer.Stop();
            _locationTimer.Elapsed -= OnTimedEvent;
            _locationTimer.Dispose();
            _locationTimer = null;
        }

        //stop route
        Pins = [];
        _pinsAndCirkles = [];
        _pinActivationStatus = [];
        MapElements = [];
    }

    private void OnTimedEvent(object? sender, ElapsedEventArgs e)
    {
        Task.Run(OnTimedEventAsync);
    }

    private Dictionary<Pin, bool> _pinActivationStatus = new();

    private async Task OnTimedEventAsync()
    {
        var displayGpsError = false;

        try
        {
            Debug.WriteLine("Initializing location timer.");
            var location = await _geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.High));
            if (location is null)
            {
                displayGpsError = true;
                return;
            }
            if (_pause)
            {
                return;
            }

            foreach (Pin pin in Pins)
            {
               
                var distInMeters = location.CalculateDistance(pin.Location, DistanceUnits.Kilometers) * 1000;

                if (distInMeters <= 20)
                {
                    // Check if the pin is already activated (i.e., popup was shown recently)
                    if (_pinActivationStatus.TryGetValue(pin, out bool isActive) && isActive)
                    {
                        Debug.WriteLine($"Already handled pin: {pin.Label}");
                        continue; // Skip showing the popup again
                    }

                    Debug.WriteLine($"Close enough to {pin.Label}, showing popup.");
                    _pinActivationStatus[pin] = true; // Mark the pin as active


                    if (_pinsAndCirkles.TryGetValue(pin, out Circle circle))
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            // Verander de kleur van de cirkel
                            circle.FillColor = Colors.Green;

                            // Notify de UI (omdat MapElements geobserveerd wordt)
                            OnPropertyChanged(nameof(MapElements));
                        });
                    }
                        

                    MarkerClickedCommand.Execute(pin);
                }
                else
                {
                    // Mark pin as inactive when out of range
             //       _pinActivationStatus[pin] = false;
                }
            }
        }catch(FeatureNotEnabledException e)
        {
            displayGpsError = true;
            Debug.WriteLine(e);
        }
        catch (Exception ex)
        {
            displayGpsError = true;
            Debug.WriteLine(ex);
        }
        finally
        {
            if (displayGpsError)
            {
                if (_isShowingLocationError)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        LocationLost?.Invoke(this, "gps geeft geen locatie meer weer");
                    });
                    _isShowingLocationError = false;
                }

            }
            else 
            {
                _isShowingLocationError = true; 
            }
            
        }
        
    }


    private void ZoomToBreda()
    {
        Location location = new Location(51.588331, 4.777802);
        MapSpan mapSpan = new MapSpan(location, 0.015, 0.015);
        CurrentMapSpan = mapSpan;
    }

    private void CheckInternetConnection()
    {
        OnPropertyChanged(nameof(HasInternetConnection));
        if (!HasInternetConnection)
        {
            if (_isShowingNetwerkError)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    InternetConnectionLost?.Invoke(this, "Verbinding is verbroken");
                    _isShowingNetwerkError = false;
                });
            }
        }
        else
        {
            _isShowingNetwerkError = true;
        }
    }

    private MapElement CreatePolyLineOfLocations(IEnumerable<Location> locations)
    {
        Debug.WriteLine("Constructing {0}", args: nameof(Polyline));
        Polyline polyline = new Polyline
        {
            StrokeColor = Colors.Red,
            StrokeWidth = 12,
        };
        Debug.WriteLine("Adding to {0}.", args: nameof(polyline.Geopath));
        polyline.Geopath.Clear();
        foreach (var loc in locations)
        {
            Debug.WriteLine($"Adding location: {loc.Latitude}, {loc.Longitude}");
            polyline.Geopath.Add(loc);
        }

        return polyline;
    }

    public void OnLoad(string routeTag)
    {
        MainThread.InvokeOnMainThreadAsync(() => CreateRoute(routeTag));
        Task.Run(() => RouteStarting());

        InitInternetCheckTimer();
    }

    private void InitInternetCheckTimer()
    {
        const int updateTimeInMs = 4000;
        System.Timers.Timer timer = new System.Timers.Timer(updateTimeInMs);
        timer.Elapsed += (_, _) => CheckInternetConnection();
        timer.Start();
    }

    private async Task CreateRoute(string routeTag)
    {


        var routeLocations = await _databaseRepo.GetPointsSpecificRoute(routeTag);

        MapElements.Add(
            CreatePolyLineOfLocations(routeLocations.Select(x => new Location(x.longitude, x.latitude))));
        MapElements = new ObservableCollection<MapElement>(MapElements);
        OnPropertyChanged(nameof(MapElements));

        foreach (var routeLocation in routeLocations)
        {
            if (routeLocation.name is not null) CreatePin(routeLocation);
        }

    }

    public void CreatePin(RouteLocation routeLocation)
    {
        Pin pin = new Pin
        {
            Label = routeLocation.name,
            Location = new Location(routeLocation.longitude, routeLocation.latitude),
            Type = PinType.Generic
        };
        Pins.Add(pin);
        
        Circle circle = new Circle
        {
            Center = new Location(routeLocation.longitude, routeLocation.latitude),
            Radius = new Distance(20),
            FillColor = Colors.Red,
            
        };

        MapElements.Add(circle);

        MapElements = new ObservableCollection<MapElement>(MapElements);

        _pinsAndCirkles.Add(pin,circle);

        OnPropertyChanged(nameof(MapElements));
    }

    [RelayCommand]
    public async Task MarkerClicked(Pin pin)
    {
        Debug.WriteLine("Clicked on Marker.");

        // First find the proper routeLocation
        double tolerance = 0.0001D;
        var routeLocation = (await _databaseRepo.GetAllRoutes()).FirstOrDefault(ro =>
            // First check latitude
            Math.Abs(ro.longitude - pin.Location.Latitude) < tolerance

            // Then check longitude
            && Math.Abs(ro.latitude - pin.Location.Longitude) < tolerance);

        if (routeLocation is not null)
        {
            IFileService fileService = new FileService();
            // Then rev up those fryers
            Debug.WriteLine("Firing pop up.");
            var viewModel = new InfoPopupViewModel(new RouteLocation
            {
                longitude = routeLocation.longitude,
                latitude = routeLocation.latitude,
                name = routeLocation.name,
                descriptionNL = routeLocation.descriptionNL,
                descriptionENG = routeLocation.descriptionENG,
                imageSource = routeLocation.imageSource
            }, fileService);

            viewModel.setData();


            try
            {
                MopupService.Instance.PushAsync(new InfoPointPopup(viewModel));
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Pin Error: {e}");
            } 
        }
        else
        {
            Debug.WriteLine($"No {nameof(routeLocation)} found.");
        }
    }
}