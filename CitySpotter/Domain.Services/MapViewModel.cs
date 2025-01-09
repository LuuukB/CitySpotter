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
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace CitySpotter.Domain.Services
{
    public partial class MapViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isStartEnabled = true;
        [ObservableProperty] private bool _isStopEnabled = false;

        private readonly IGeolocation _geolocation;


        private System.Timers.Timer? _locationTimer;

 

        [ObservableProperty] private ObservableCollection<MapElement> _mapElements = new();

        private List<Location> _locationCache = [];


        [ObservableProperty] public MapSpan _currentMapSpan;

  

        public MapViewModel(IGeolocation geolocation)
        {
            _geolocation = geolocation;
            _geolocation.LocationChanged += LocationChanged;
        }
        public void LocationChanged(object? sender, GeolocationLocationChangedEventArgs e) 
        {
            if (e?.Location != null)
            {
                try
                {
                    Debug.WriteLine("Location: {0}", e.Location);
                    CurrentMapSpan = MapSpan.FromCenterAndRadius(e.Location, Distance.FromMeters(30));

                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Fout bij ophalen locatie: {ex.Message}");
                }
            }

        }
        //deze methode moet eerst aangeroepen worden voordat het gps event gebint met werken. 
        public async Task ListeningToLocation()
        {
            await _geolocation.StartListeningForegroundAsync(new GeolocationListeningRequest
            {
                MinimumTime = TimeSpan.FromSeconds(5),
                DesiredAccuracy = GeolocationAccuracy.Best
            });

        }
        //voor het stoppen van het event. anders blijft die continu doorgaan.
        public void StopListeningToLocation()
        {
            _geolocation.StopListeningForeground();
        }
      

        
        private void UpdateRoute(Location location)
        {
            Debug.WriteLine("Running {0} at {1}.", nameof(UpdateRoute), DateTime.Now.ToShortTimeString());

            ProcessNewLocation(location);

            // Alleen tekenen als er meer dan 2 punten zijn. Anders heb je natuurlijk geen lijn!
            if (_locationCache.Count >= 2)
            {
                MapElements = [CreatePolyLineOfLocations(_locationCache)];
            }

            Debug.WriteLine("Added to {0}.", args: nameof(MapElements));

            Debug.WriteLine($"Route bijgewerkt: {location.Latitude}, {location.Longitude}");
        }

        private void ProcessNewLocation(Location location)
        {
            // TODO: Niet toevoegen als hij te dichtbij is bij de vorige locatie! En misschien andere logica toevoegen.
            const double minDistance = 0.01;
            if (_locationCache.Count == 0 || location.CalculateDistance(_locationCache.Last(), DistanceUnits.Kilometers) >= minDistance)
            {
                _locationCache.Add(location);
            }
            else
            {
                Debug.WriteLine($"Locatie te dicht bij de vorige locatie: {location.Latitude}, {location.Longitude}");
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


    }
}
