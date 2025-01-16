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

    private System.Timers.Timer _locationTimer;
    public event EventHandler<string> InternetConnectionLost;
    public event EventHandler<string> LocationLost;

    private readonly IGeolocation _geolocation;
    private readonly IDatabaseRepo _databaseRepo;
    private readonly IInternetHandler _internetHandler;
    private bool _isShowingNetwerkError = true;
    private bool _isShowingLocationError = true;

    public bool HasInternetConnection
    {
        get
        {
            Debug.WriteLine("Checking Internet connection");
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

    private void OnTimedEvent(object? sender, ElapsedEventArgs e)
    {
        Task.Run(OnTimedEventAsync);
    }

    //private readonly Dictionary<Pin, bool> _pinActivationStatus = new();

    //private async Task OnTimedEventAsync()
    //{
    //    var displayGpsError = false;

    //    try
    //    {
    //        Debug.WriteLine("Initializing location timer.");
    //        var location = await _geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.High));
    //        if (location is null)
    //        {
    //            displayGpsError = true;
    //            return;
    //        }

    //        foreach (var pin in Pins)
    //        {
    //            Debug.WriteLine($"Checking if {location.Latitude}, {location.Longitude} close to {pin.Location.Latitude}, {pin.Location.Longitude}");
    //            var distInMeters = location.CalculateDistance(pin.Location, DistanceUnits.Kilometers) * 1000;

    //            if (distInMeters <= 20)
    //            {
    //                // Check if the pin is already activated (i.e., popup was shown recently)
    //                if (_pinActivationStatus.TryGetValue(pin, out bool isActive) && isActive)
    //                {
    //                    Debug.WriteLine($"Already handled pin: {pin.Label}");
    //                    continue; // Skip showing the popup again
    //                }

    //                Debug.WriteLine($"Close enough to {pin.Label}, showing popup.");
    //                _pinActivationStatus[pin] = true; // Mark the pin as active
    //                MarkerClickedCommand.Execute(pin);
    //            }
    //            else
    //            {
    //                // Mark pin as inactive when out of range
    //                //       _pinActivationStatus[pin] = false;
    //            }
    //        }


    //    }
    //    catch (FeatureNotEnabledException e)
    //    {
    //        displayGpsError = true;
    //        Debug.WriteLine(e);
    //    }
    //    catch (Exception ex)
    //    {
    //        displayGpsError = true;
    //        Debug.WriteLine(ex);
    //    }
    //    finally
    //    {
    //        if (displayGpsError)
    //        {
    //            if (_isShowingLocationError)
    //            {
    //                MainThread.BeginInvokeOnMainThread(() =>
    //                {
    //                    LocationLost?.Invoke(this, "gps geeft geen locatie meer weer");
    //                });
    //                _isShowingLocationError = false;
    //            }

    //        }
    //        else
    //        {
    //            _isShowingLocationError = true;
    //        }

    //    }

    //}


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

        foreach (var routeLocation in routeLocations)
        {
            if (routeLocation.name is not null) CreatePin(routeLocation);
        }

        MapElements.Add(
            CreatePolyLineOfLocations(routeLocations.Select(x => new Location(x.longitude, x.latitude))));
        MapElements = new ObservableCollection<MapElement>(MapElements);
        OnPropertyChanged(nameof(MapElements));
    }

    public void CreatePin(RouteLocation routeLocation)
    {
        Pins.Add(new Pin
        {
            Label = routeLocation.name,
            Location = new Location(routeLocation.longitude, routeLocation.latitude),
            Type = PinType.Generic
        });
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
    //till here



    private List<Pin> _visitedPins = new List<Pin>();
    private bool IsPinVisited(Pin pin)
    {
        bool visited = _visitedPins.Any(p => p.Label == pin.Label);
        Debug.WriteLine($"Checking if pin {pin.Label} is visited: {visited}");
        return visited;
    }


    private MapElement CreatePolyLineOfLocations(IEnumerable<Location> locations, List<Pin> pins)
    {
        Debug.WriteLine("Constructing {0}", args: nameof(Polyline));
        Polyline polyline = new Polyline
        {
            StrokeWidth = 12,
        };

        // Loop door locaties en bepaal kleur per sectie
        Debug.WriteLine("Adding to {0}.", args: nameof(polyline.Geopath));
        polyline.Geopath.Clear();

        for (int i = 0; i < locations.Count() - 1; i++)
        {
            var currentPin = pins[i];
            var nextPin = pins[i + 1];

            // Controleer of de pin bezocht is op basis van de Label of locatie
            var lineColor = IsPinVisited(currentPin) ? Colors.Blue : Colors.Red;

            // Debugging: Print de gekozen kleur
            Debug.WriteLine($"Changing line color to: {lineColor}");

            polyline.StrokeColor = lineColor;  // Kleur instellen buiten de loop

            polyline.Geopath.Add(locations.ElementAt(i));
            polyline.Geopath.Add(locations.ElementAt(i + 1));
            OnPropertyChanged(nameof(MapElements));

        }

        return polyline;
    }



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

            foreach (var pin in Pins)
            {
                var distInMeters = location.CalculateDistance(pin.Location, DistanceUnits.Kilometers) * 1000;

                Debug.WriteLine($"Checking distance to pin: {pin.Label}, Distance: {distInMeters} meters");

                if (distInMeters <= 20 && !_visitedPins.Contains(pin))
                {
                    Debug.WriteLine($"Pin {pin.Label} is within range, adding to visited pins.");
                    _visitedPins.Add(pin);
                    MarkerClickedCommand.Execute(pin);
                    // Herbouw de polyline na het toevoegen van de bezochte pin
                    
                        MapElements.Clear();
                        MapElements.Add(CreatePolyLineOfLocations(Pins.Select(p => new Location(p.Location.Latitude, p.Location.Longitude)), Pins.ToList()));
                        OnPropertyChanged(nameof(MapElements));
                    

                    // Als de gebruiker de laatste pin heeft bereikt, stop dan de route
                    if (_visitedPins.Count == Pins.Count)
                    {
                        _locationTimer.Stop();
                        Debug.WriteLine("Route complete.");
                    }

                    break; // Stop met verder zoeken, we hebben een pin gevonden
                }
            }
        }
        catch (FeatureNotEnabledException e)
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
                        LocationLost?.Invoke(this, "GPS geeft geen locatie meer weer");
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





}