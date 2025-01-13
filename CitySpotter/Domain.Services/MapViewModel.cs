using CitySpotter.Domain.Model;
using CitySpotter.Locations.Locations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;
using Mopups.Services;
using CitySpotter.Domain.Services.Internet;

namespace CitySpotter.Domain.Services;

public partial class MapViewModel : ObservableObject
{
    [ObservableProperty] private string _routeName;
    [ObservableProperty] private ObservableCollection<MapElement> _mapElements = [];
    [ObservableProperty] private MapSpan _currentMapSpan;
    [ObservableProperty] private ObservableCollection<Pin> _pins = [];

    private readonly IGeolocation _geolocation;
    private readonly IDatabaseRepo _databaseRepo;

    private System.Timers.Timer? _locationTimer;
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
        _databaseRepo = repository;
        _geolocation = geolocation;
        _internetHandler = internetHandler;

        _databaseRepo.Init();

        if (_databaseRepo.GetAllRoutes().Count == 0)
        {
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.594112,
                latitude = 4.779417,
                name = "VVV-Kantoor",
                description = "vvvkantoorbeschrijving.txt",
                imageSource = "vvvkantoor.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.593278,
                latitude = 4.779388,
                name = "Liefdeszuster",
                description = "liefdeszusterbeschrijving.txt",
                imageSource = "liefdeszuster.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.592500,
                latitude = 4.779695,
                name = "Nassau-Baroniemonument",
                description = "nassaubaroniemonumentbeschrijving.txt",
                imageSource = "nassaubaroniemonument.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.592500, latitude = 4.779388, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.592833,
                latitude = 4.778472,
                name = "Vuurtoren",
                description = "vuurtorenbeschrijving.txt",
                imageSource = "vuurtoren.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.592667, latitude = 4.777917, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.590612, latitude = 4.777000, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.590612,
                latitude = 4.776167,
                name = "Kasteel Breda",
                description = "kasteelbeschrijving.txt",
                imageSource = "kasteelbreda.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.589695,
                latitude = 4.776138,
                name = "Stadhouderspoort",
                description = "stadhouderspoortbeschrijving.txt",
                imageSource = "stadhouderspoort.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.590333, latitude = 4.776000, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.590388, latitude = 4.775000, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.590028,
                latitude = 4.774362,
                name = "Huis van Brecht",
                description = "huisvanbrechtbeschrijving.txt",
                imageSource = "huisvanbrecht7.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.590195,
                latitude = 4.773445,
                name = "Spanjaardsgat",
                description = "spanjaardsgatbeschrijving.txt",
                imageSource = "spanjaardsgat.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.589833,
                latitude = 4.773333,
                name = "Vismarkt",
                description = "vismarktbeschrijving.txt",
                imageSource = "vismarktbreda.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.589362,
                latitude = 4.774445,
                name = "Havermarkt",
                description = "havermarktbeschrijving.txt",
                imageSource = "havermarkt.webp",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.588778, latitude = 4.774888, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.588833,
                latitude = 4.775278,
                name = "Grote Kerk Breda",
                description = "grotekerkbeschrijving.txt",
                imageSource = "grotekerk.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.588778, latitude = 4.774888, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.588195,
                latitude = 4.775138,
                name = "Het Poortje",
                description = "poortjebeschrijving.txt",
                imageSource = "hetpoortje.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.587083,
                latitude = 4.775750,
                name = "Ridderstraat",
                description = "ridderstraatbeschrijving.txt",
                imageSource = "ridderstraat.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.587417,
                latitude = 4.776555,
                name = "De Grote Markt",
                description = "grotemarktbeschrijving.txt",
                imageSource = "grotemarktbreda.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.588028,
                latitude = 4.776333,
                name = "Bevrijdingsmonument",
                description = "bevrijdingsmonumentbeschrijving.txt",
                imageSource = "bevrijdingsmonument.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.588750,
                latitude = 4.776112,
                name = "Stadhuis",
                description = "stadhuisbeschrijving.txt",
                imageSource = "stadhuis.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.587972, latitude = 4.776362, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.587500, latitude = 4.776555, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.587638,
                latitude = 4.777250,
                name = "Antonius van Padua Kerk",
                description = "antoniusvanpadaukerkbeschrijving.txt",
                imageSource = "paduakerk.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.588278, latitude = 4.778500, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.588000,
                latitude = 4.778945,
                name = "Bibliotheek",
                description = "bibliotheekbeschrijving.txt",
                imageSource = "bibliotheekbreda.webp",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.587362, latitude = 4.780222, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.587722,
                latitude = 4.781028,
                name = "Kloosterkazerne",
                description = "kloosterkazernebeschrijving.txt",
                imageSource = "kloosterkazerne.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.587750,
                latitude = 4.782000,
                name = "ChassÃ© theater",
                description = "chassetheaterbeschrijving.txt",
                imageSource = "chassetheater.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.587750, latitude = 4.781250, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.588612, latitude = 4.780888, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.589500, latitude = 4.780445, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.589667,
                latitude = 4.781000,
                name = "Beyerd",
                description = "beyerdbeschrijving.txt",
                imageSource = "beyerd.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.589500, latitude = 4.780445, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.589555,
                latitude = 4.780000,
                name = "Gasthuispoort",
                description = "gasthuispoortbeschrijving.txt",
                imageSource = "gasthuispoort.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.589417, latitude = 4.779862, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.589028, latitude = 4.779695, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.588555, latitude = 4.778333, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.589112,
                latitude = 4.777945,
                name = "Willem Merkxtuin",
                description = "willemmerkxtuinbeschrijving.txt",
                imageSource = "willemmerkxtuin.webp",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.589667, latitude = 4.777805, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            {
                longitude = 51.589695,
                latitude = 4.778362,
                name = "Begijnhof",
                description = "begijnhofbeschrijving.txt",
                imageSource = "begijnhof.jpg",
                routeTag = "historischeKilometer"
            });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.589667, latitude = 4.777805, routeTag = "historischeKilometer" });
            _databaseRepo.AddRoute(new RouteLocation
            { longitude = 51.589500, latitude = 4.776250, routeTag = "historischeKilometer" });
        }

        Debug.WriteLine("De database heef zoveel punten: " + _databaseRepo.GetAllRoutes().Count);

        ZoomToBreda();
        //todo dit in methode zetten 
        const int updateTimeInMs = 4000;
        System.Timers.Timer timer = new System.Timers.Timer(updateTimeInMs);
        timer.Elapsed += (sender, args) => CheckInternetConnection();
        timer.Start();

        Task.Run(() => InitListener(geolocation));
    }
    private async Task InitListener(IGeolocation geolocation, GeolocationAccuracy accuracy = GeolocationAccuracy.Best)
    {
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
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
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

    public void CreateRoute(string routeTag)
    {
        var routeLocations = _databaseRepo.GetPointsSpecificRoute(routeTag);

        foreach (var routeLocation in routeLocations)
        {
            if (routeLocation.name is not null) CreatePin(routeLocation);
        }

        // NOTE: Yes, lat & long reversed ;)
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
        var routeLocation = _databaseRepo.GetAllRoutes().FirstOrDefault(ro =>
            // NOTE: LONGIDUDE EN LATITUDE ZIJN OMGEDRAAIT WTF BROEDERS> MAAR DIT WERKT
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