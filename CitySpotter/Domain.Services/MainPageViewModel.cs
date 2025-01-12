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
        private async Task NavigateToMap()
        {
            Debug.WriteLine("in Navigate to Map");
            if (SelectedRoute != null)
            {
                Debug.WriteLine($"Navigating to map with route: {SelectedRoute}");

                //await Shell.Current.GoToAsync($"MapPage?routeName={SelectedRoute}");
            }
        }




    }
}
