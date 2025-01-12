using CitySpotter.Domain.Services;
using CitySpotter.Locations.Locations;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using Mopups.Services;
using System.Diagnostics;

namespace CitySpotter;
[QueryProperty(nameof(RouteName), "routeName")]
public partial class MapPage : ContentPage
{
    private List<RouteLocation> routes = new List<RouteLocation>();
    private string _routeName;
    public MapPage(MapViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

        routes = viewModel.GetRouteLocations();
        createRoute(routes);
	}
    public string RouteName
    {
        get => _routeName;
        set
        {
            _routeName = value;
            if (BindingContext is MapViewModel viewModel && !string.IsNullOrEmpty(value))
            {
                viewModel.LoadRoute(value);
            }
        }
    }
    public void createRoute(List<RouteLocation> routeLocations)
    {
        foreach (var routeLocation in routeLocations)
        {
            if(routeLocation.name != null) {
                createPin(routeLocation);
            }
        }
    }
    public void createPin(RouteLocation routeLocation)
    {
        Pin pinCreated = new Pin
        {
            Label = routeLocation.name,
            Location = new Location(routeLocation.longitude, routeLocation.latitude),
            Type = PinType.Generic
        }; 
        pinCreated.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation {longitude = routeLocation.longitude, latitude = routeLocation.latitude, name = routeLocation.name, description = routeLocation.description, imageSource = routeLocation.imageSource})));

        };
        MapView.Pins.Add(pinCreated);
    }
}