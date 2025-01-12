using CitySpotter.Domain.Model;
using CitySpotter.Locations.Locations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitySpotter.Domain.Services
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IDatabaseRepo _databaseRepo;
        private readonly ILocationPermissionsService _permissionsService;

        [ObservableProperty] public string _nameOfRoute;
        [ObservableProperty] public ObservableCollection<string> _routeNames;
        public MainPageViewModel(IDatabaseRepo database, ILocationPermissionsService locationPermissionsService)
        {
            _databaseRepo = database;
            RouteNames = new ObservableCollection<string>();
        }
        [RelayCommand]
        public void GetRoutesForView() 
        { 
            Debug.WriteLine("Loading routes");
            LoadRoutesLocations();
        }
        private void LoadRoutesLocations()
        { 
            var allRoutesNames = _databaseRepo.GetAllNamesRoutes();
            RouteNames.Clear();
            foreach (var route in allRoutesNames)
            {
                RouteNames.Add(route);
            }
        }
        [RelayCommand]
        private async Task NavigateToMap(string routeName)
        {
            Debug.WriteLine("In Navigate to Map");
            if (!string.IsNullOrWhiteSpace(routeName))
            {
                Debug.WriteLine($"Navigating to map with route: {routeName}");

                // Navigeren naar de MapPage met de geselecteerde route als queryparameter
                await Shell.Current.GoToAsync($"///MapPage?routeName={routeName}");
            }
        }

        [RelayCommand]
        private async Task PageLoad()
        {
            // On page load, we check perms and request it if needed.
            Debug.WriteLine("Requesting perms.");
            var currentStatus = await _permissionsService.CheckAndRequestLocationPermissionAsync();

            if (currentStatus == PermissionStatus.Denied)
            {
                Debug.WriteLine("No perms granted. Showing settings.");
                await _permissionsService.ShowSettingsIfPermissionDeniedAsync();
            }
        }
    }
}
