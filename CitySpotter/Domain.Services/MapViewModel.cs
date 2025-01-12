using Microsoft.Maui.Controls;
using CitySpotter.Domain.Model;
using CitySpotter.Locations.Locations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Mopups.Services;
using System.Reflection.Metadata;

namespace CitySpotter.Domain.Services
{
    public partial class MapViewModel : ObservableObject
    {
        [ObservableProperty] private string _routeName;
        [ObservableProperty] private ObservableCollection<MapElement> _mapElements = new();
        [ObservableProperty] public MapSpan _currentMapSpan;
        [ObservableProperty] private ObservableCollection<Pin> _pins = new();

        private readonly IGeolocation _geolocation;


        private IDatabaseRepo _databaseRepo;

        private System.Timers.Timer? _locationTimer;
        private List<RouteLocation> _routeLocations;
        private readonly LocationPermissionService _locationService;

        public MapViewModel(IGeolocation geolocation, IDatabaseRepo repository)
        {
            _databaseRepo = repository;
            _geolocation = geolocation;
            _locationService = new LocationPermissionService();
            


            _databaseRepo.Init();

            //new RouteLocation{ longitude = 51.592496, latitude = 4.779975, name = "Monument ValkenburgPark", description = "info text over dit monument", imageSource = "nassaubaroniemonument.jpg"};
            
            Location location = new Location(51.588331, 4.777802);
            MapSpan mapSpan = new MapSpan(location, 0.015, 0.015);
            CurrentMapSpan = mapSpan;
        }

        // wordt dit gebruikt???
        public async Task<List<RouteLocation>> GetRouteLocations()
        {
            return await _databaseRepo.GetAllRoutes();
        }
        public async Task HandleOnAppearingAsync()
        {
            var hasPermission = await _locationService.CheckAndRequestLocationPermissionAsync();

            if (hasPermission == PermissionStatus.Denied)
            {
                bool openedSettings = await _locationService.ShowSettingsIfPermissionDeniedAsync();

                if (!openedSettings)
                {
                    // Sluit de app af als de gebruiker niet naar instellingen wil gaan
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
            }

            var routes = await _databaseRepo.GetAllRoutes();
            if (routes.Count == 0)
            {
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.594112, latitude = 4.779417, name = "VVV-Kantoor", description = "vvvkantoorbeschrijving.txt", imageSource = "vvvkantoor.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.593278, latitude = 4.779388, name = "Liefdeszuster", description = "liefdeszusterbeschrijving.txt", imageSource = "liefdeszuster.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.592500, latitude = 4.779695, name = "Nassau-Baroniemonument", description = "nassaubaroniemonumentbeschrijving.txt", imageSource = "nassaubaroniemonument.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.592500, latitude = 4.779388, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.592833, latitude = 4.778472, name = "Vuurtoren", description = "vuurtorenbeschrijving.txt", imageSource = "vuurtoren.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.592667, latitude = 4.777917, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.590612, latitude = 4.777000, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.590612, latitude = 4.776167, name = "Kasteel Breda", description = "kasteelbeschrijving.txt", imageSource = "kasteelbreda.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.589695, latitude = 4.776138, name = "Stadhouderspoort", description = "stadhouderspoortbeschrijving.txt", imageSource = "stadhouderspoort.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.590333, latitude = 4.776000, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.590388, latitude = 4.775000, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.590028, latitude = 4.774362, name = "Huis van Brecht", description = "huisvanbrechtbeschrijving.txt", imageSource = "huisvanbrecht7.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.590195, latitude = 4.773445, name = "Spanjaardsgat", description = "spanjaardsgatbeschrijving.txt", imageSource = "spanjaardsgat.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.589833, latitude = 4.773333, name = "Vismarkt", description = "vismarktbeschrijving.txt", imageSource = "vismarktbreda.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.589362, latitude = 4.774445, name = "Havermarkt", description = "havermarktbeschrijving.txt", imageSource = "havermarkt.webp", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.588778, latitude = 4.774888, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.588833, latitude = 4.775278, name = "Grote Kerk Breda", description = "grotekerkbeschrijving.txt", imageSource = "grotekerk.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.588778, latitude = 4.774888, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.588195, latitude = 4.775138, name = "Het Poortje", description = "poortjebeschrijving.txt", imageSource = "hetpoortje.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.587083, latitude = 4.775750, name = "Ridderstraat", description = "ridderstraatbeschrijving.txt", imageSource = "ridderstraat.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.587417, latitude = 4.776555, name = "De Grote Markt", description = "grotemarktbeschrijving.txt", imageSource = "grotemarktbreda.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.588028, latitude = 4.776333, name = "Bevrijdingsmonument", description = "bevrijdingsmonumentbeschrijving.txt", imageSource = "bevrijdingsmonument.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.588750, latitude = 4.776112, name = "Stadhuis", description = "stadhuisbeschrijving.txt", imageSource = "stadhuis.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.587972, latitude = 4.776362, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.587500, latitude = 4.776555, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.587638, latitude = 4.777250, name = "Antonius van Padua Kerk", description = "antoniusvanpadaukerkbeschrijving.txt", imageSource = "paduakerk.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.588278, latitude = 4.778500, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.588000, latitude = 4.778945, name = "Bibliotheek", description = "bibliotheekbeschrijving.txt", imageSource = "bibliotheekbreda.webp", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.587362, latitude = 4.780222, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.587722, latitude = 4.781028, name = "Kloosterkazerne", description = "kloosterkazernebeschrijving.txt", imageSource = "kloosterkazerne.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.587750, latitude = 4.782000, name = "Chassé theater", description = "chassetheaterbeschrijving.txt", imageSource = "chassetheater.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.587750, latitude = 4.781250, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.588612, latitude = 4.780888, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.589500, latitude = 4.780445, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.589667, latitude = 4.781000, name = "Beyerd", description = "beyerdbeschrijving.txt", imageSource = "beyerd.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.589500, latitude = 4.780445, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.589555, latitude = 4.780000, name = "Gasthuispoort", description = "gasthuispoortbeschrijving.txt", imageSource = "gasthuispoort.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.589417, latitude = 4.779862, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.589028, latitude = 4.779695, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.588555, latitude = 4.778333, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.589112, latitude = 4.777945, name = "Willem Merkxtuin", description = "willemmerkxtuinbeschrijving.txt", imageSource = "willemmerkxtuin.webp", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.589667, latitude = 4.777805, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.589695, latitude = 4.778362, name = "Begijnhof", description = "begijnhofbeschrijving.txt", imageSource = "begijnhof.jpg", routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.589667, latitude = 4.777805, routeTag = "historischeKilometer" });
                await _databaseRepo.AddRoute(new RouteLocation { longitude = 51.589500, latitude = 4.776250, routeTag = "historischeKilometer" });
            }
            Debug.WriteLine("De database heef zoveel punten: " + routes.Count);



        }
        //methode wordt veranderd en mag binnenkort weg
        public async Task LoadRoute(string routeName)
        {
            Debug.WriteLine($"Loading route: {routeName}");
            RouteStarting();
            var routeLocations = await _databaseRepo.GetPointsSpecificRoute(routeName);
            if (routeLocations.Count() >= 2)
            {
                var routePolyline = CreatePolyLineOfLocations(routeLocations.Select(loc => new Location(loc.latitude, loc.longitude)));
               
                var existingPolylines = MapElements.OfType<Polyline>().ToList();
                foreach (var polyline in existingPolylines)
                {
                    MapElements.Remove(polyline);
                }
                // nieuwe polyline 
                MapElements.Add(routePolyline);
            }
            else
            {
                Debug.WriteLine("Geen locaties gevonden voor deze route.");
            }
        }
        public void RouteStarting()
        { 
            Debug.WriteLine("starting route/timer");
            _locationTimer = new System.Timers.Timer(5000);
            _locationTimer.Elapsed += OnTimedEvent;
            _locationTimer.AutoReset = true;
            _locationTimer.Start();
        }
        public void RouteStop()
        {
            Debug.WriteLine("stopping route/timer");

            if (_locationTimer != null)
            {
                _locationTimer.Stop();
                _locationTimer.Dispose();
                _locationTimer = null;
            }
        }
        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
         //   Task.Run(OnTimedEventAsync);
        }
        private async Task OnTimedEventAsync()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status == PermissionStatus.Granted)
            {
                try
                {
                    Debug.WriteLine("Running {0} at {1}", nameof(OnTimedEventAsync), DateTime.Now.ToShortTimeString());

                    var location = await _geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best));

                    if (location is not null)
                    {
                        Debug.WriteLine("Location: {0}", location);
                        CurrentMapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromMeters(60));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Fout bij ophalen locatie: {ex.Message}");
                }
            }
            else if (status == PermissionStatus.Denied) 
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
        }
        private Polyline CreatePolyLineOfLocations(IEnumerable<Location> locations)
        {
            Debug.WriteLine("Constructing {0}", args: nameof(Polyline));
            Polyline polyline = new Polyline
            {
                StrokeColor = Colors.Red,
                StrokeWidth = 12,
            };
            Debug.WriteLine("Adding to {0}.", args: nameof(polyline.Geopath));
            foreach (var loc in locations)
            {
                polyline.Geopath.Add(loc);
            }
            return polyline;
        }
        public async Task CreateRoute(string routeTag)
        {
            List<RouteLocation> routeLocations = await _databaseRepo.GetPointsSpecificRoute(routeTag);

            Polyline line = new Polyline();
            foreach (var routeLocation in routeLocations)
            {
                line.Add(new Location(routeLocation.latitude, routeLocation.longitude));

                if (routeLocation.name != null)
                {
                    CreatePin(routeLocation);
                }
            }
            MapElements.Add(line);
        }
        public void CreatePin(RouteLocation routeLocation)
        {
            Pin pinCreated = new Pin
            {
                Label = routeLocation.name,
                Location = new Location(routeLocation.longitude, routeLocation.latitude),
                Type = PinType.Generic
            };
            Pins.Add(pinCreated);


        }

        [RelayCommand]
        private async Task MarkerClicked(Pin pin)
        {
            Debug.WriteLine("Clicked on Marker.");

            // First find the proper routeLocation
            double tolerance = 0.0001D;
            var routeLocationasync = await _databaseRepo.GetAllRoutes();
            var routeLocation = routeLocationasync.FirstOrDefault(ro =>
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
}
