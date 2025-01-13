using CitySpotter.Domain.Model;
using CitySpotter.Locations.Locations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Mopups.Services;
using CitySpotter.Domain.Services.Internet;


namespace CitySpotter.Domain.Services;

public partial class MapViewModel : ObservableObject
{
    [ObservableProperty] private string _routeName;
    [ObservableProperty] private string _imageSource = "nointernetpopupscreen.jpg";
    [ObservableProperty] private ObservableCollection<MapElement> _mapElements = [];
    [ObservableProperty] private MapSpan _currentMapSpan;
    [ObservableProperty] private ObservableCollection<Pin> _pins = [];

    private readonly IGeolocation _geolocation;
    private readonly IDatabaseRepo _databaseRepo;
    private readonly IInternetHandler _internetHandler;

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


    private async Task InitListener(IGeolocation geolocation, GeolocationAccuracy accuracy = GeolocationAccuracy.Best)
    {
        var displayGpsError = false;

        try
        {
            Debug.WriteLine("Initializing location listener.");
            geolocation.LocationChanged += Geolocation_LocationChanged;
            var request = new GeolocationListeningRequest(accuracy);
            var success = await geolocation.StartListeningForegroundAsync(request);

            string status = success
                ? "Started listening for foreground location updates"
                : "Couldn't start listening";

            Debug.WriteLine(status);

            if (!success) displayGpsError = true;
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
                await Application.Current.MainPage.DisplayAlert(
                    "Kan geen locatie verkrijgen",
                    "De app kan niet goed werken zolang je locatie niet te verkrijgen. Check je instellingen.",
                    "OK"
                );
            }
        }
    }
    private void ZoomToBreda()
    {
        Location location = new Location(51.588331, 4.777802);
        MapSpan mapSpan = new MapSpan(location, 0.015, 0.015);
        CurrentMapSpan = mapSpan;
    }

    private void Geolocation_LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
    {
        var curLoc = e.Location;

        Debug.WriteLine($"Last pulled pos: {curLoc.Latitude}, {curLoc.Longitude}");

        // Now check if close to location.
        foreach (var pin in Pins)
        {
            Debug.WriteLine($"Checking if {curLoc.Latitude}, {curLoc.Longitude} close to {pin.Location.Latitude}, {pin.Location.Longitude}");
            var distInMeters = curLoc.CalculateDistance(pin.Location, DistanceUnits.Kilometers) * 1000;
            if (distInMeters <= 20)
            {
                Debug.WriteLine($"Close enough to {pin.Label}");
                MarkerClickedCommand.Execute(pin);
            }
        }
    }

    private void CheckInternetConnection()
    {
        OnPropertyChanged(nameof(HasInternetConnection));
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
        Task.Run(() => InitListener(_geolocation));
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
    private async Task MarkerClicked(Pin pin)
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
            // Then rev up those fryers
            Debug.WriteLine("Firing pop up.");
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation
            {
                longitude = routeLocation.longitude,
                latitude = routeLocation.latitude,
                name = routeLocation.name,
                description = routeLocation.description,
                imageSource = routeLocation.imageSource
            })));
        }
        else
        {
            Debug.WriteLine($"No {nameof(routeLocation)} found.");
        }
    }
}