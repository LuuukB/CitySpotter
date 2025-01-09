using CitySpotter.Domain.Model;
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

        [ObservableProperty] private Route _selectedRoute;
        [ObservableProperty] private Route _routeName;



        [ObservableProperty] private ObservableCollection<Route> _routes;

        public MainPageViewModel(IDatabaseRepo database) 
        { 
            _databaseRepo = database;
            Routes = new ObservableCollection<Route>();
            LoadRoutes();
        }

      
        private void LoadRoutes()
        {
            var allRoutes = _databaseRepo.GetAllRoutes();
            Routes.Clear();

            foreach (var route in allRoutes)
            {
                Routes.Add(route);
                RouteName.RouteName = route.RouteName;
            }
        }

        [RelayCommand]
        private async Task NavigateToMap()
        {
            if (SelectedRoute != null)
            {
                await Shell.Current.GoToAsync($"MapPage?routeId={SelectedRoute.RouteId}");
            }
        }


    }
}
