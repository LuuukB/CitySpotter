using CitySpotter.Domain.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
            _permissionsService = locationPermissionsService;
            RouteNames = new ObservableCollection<string>();

            LoadRoutesLocations();
        }
        [RelayCommand]
        public void GetRoutesForView() 
        { 
            Debug.WriteLine("Loading routes");
            LoadRoutesLocations();
        }
        public async Task LoadRoutesLocations()
        { 
            var allRoutesNames = await _databaseRepo.GetAllNamesRoutes();
            RouteNames.Clear();
            foreach (var route in allRoutesNames)
            {
                RouteNames.Add(route);
            }
        }
        [RelayCommand]
        public async Task NavigateToMap(string routeName)
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
        public async Task PageLoad()
        {
            // On page load, we check perms and request it if needed.
            Debug.WriteLine("Requesting perms.");
            var currentStatus = await _permissionsService.CheckAndRequestLocationPermissionAsync();

            if (currentStatus == PermissionStatus.Denied)
            {
                Debug.WriteLine("No perms granted. Showing settings.");
                await _permissionsService.ShowSettingsIfPermissionDeniedAsync();
            }

            // We also initialize the database!
            
            await _databaseRepo.Init();
 //           await _databaseRepo.Drop();
            
        }
    }
}
