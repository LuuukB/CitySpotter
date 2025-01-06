﻿using CommunityToolkit.Mvvm.ComponentModel;
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

namespace CitySpotter.Domain.Services
{
    public partial class MapViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isStartEnabled = true;
        [ObservableProperty] private bool _isStopEnabled = false;

        private readonly IGeolocation _geolocation;


        private System.Timers.Timer? _locationTimer;

        //private readonly List<Location> _routeCoordinates = new();

        [ObservableProperty] private ObservableCollection<MapElement> _mapElements = new();

        private List<Location> _locationCache = [];


        [ObservableProperty] public MapSpan _currentMapSpan;

        //[ObservableProperty]
        //public string currentLocation;

        public MapViewModel(IGeolocation geolocation)
        {
            _geolocation = geolocation;
            //todo: beter gezegd de currentmapspan moet naar user toe op het moment dat de map word gemaakt.
            InitializeMap();
        }

        private async void InitializeMap()
        {
            try
            {
                var location = await _geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best));

                if (location is not null)
                {
                    CurrentMapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromMeters(10));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Fout bij ophalen locatie: {ex.Message}");
            }
        }

        //[RelayCommand]
        //public void RouteStarting()
        //{
        //    Debug.WriteLine("starting route/timer");
        //    _locationTimer = new System.Timers.Timer(5000);
        //    _locationTimer.Elapsed += OnTimedEvent;
        //    _locationTimer.AutoReset = true;
        //    _locationTimer.Start();
        //    IsStartEnabled = false;
        //    IsStopEnabled = true;
        //}

        //[RelayCommand]
        //public void RouteStop()
        //{
        //    Debug.WriteLine("stopping route/timer");

        //    if (_locationTimer != null)
        //    {
        //        _locationTimer.Stop();
        //        _locationTimer.Dispose();
        //        _locationTimer = null;
        //        IsStartEnabled = true;
        //        IsStopEnabled = false;
        //        MapElements.Clear();
        //        //todo eerst route opslaan voordat je de lijnen weg haalt idk wat we nog echt willen gaan doen.
        //    }
        //}

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            Task.Run(OnTimedEventAsync);
        }

        private async Task OnTimedEventAsync()
        {
            try
            {
                Debug.WriteLine("Running {0} at {1}", nameof(OnTimedEventAsync), DateTime.Now.ToShortTimeString());

                var location =
                    await _geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best));

                if (location is not null)
                {
                    Debug.WriteLine("Location: {0}", location);
                    CurrentMapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromMeters(10));

                    UpdateRoute(location);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Fout bij ophalen locatie: {ex.Message}");
            }
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