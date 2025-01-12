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

        [ObservableProperty] private string _selectedRoute;
        [ObservableProperty] private string _nameOfRoute;



        [ObservableProperty] private ObservableCollection<string> _routeNames;


        public MainPageViewModel(IDatabaseRepo database)
        {
            _databaseRepo = database;
            RouteNames = new ObservableCollection<string>();
           


        }



        [RelayCommand]
        public void GetRoutesForView() 
        { 
            Debug.WriteLine("Loading routes");
            LoadRoutesNames();
           

        }
     
        private void LoadRoutesNames()
        {
            Debug.WriteLine("Loading routes");
            var allLocations = _databaseRepo.GetAllRoutes();
            RouteNames.Clear();

            // Haal de unieke RouteNames op
            var uniqueRouteNames = allLocations
                .Select(location => location.RouteName)
                .Distinct()
                .ToList();

            foreach (var routeName in uniqueRouteNames)
            {
                RouteNames.Add(routeName);
            }
        }



        [RelayCommand]
        private async Task NavigateToMap()
        {
            if (SelectedRoute != null)
            {
                Debug.WriteLine($"Navigating to map with route: {SelectedRoute}");

                //await Shell.Current.GoToAsync($"MapPage?routeName={SelectedRoute}");
            }
        }




    }
}
