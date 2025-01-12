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
	}
    public string RouteName
    {
        get => _routeName;
        set
        {
            _routeName = value;
            if (BindingContext is MapViewModel viewModel && !string.IsNullOrEmpty(value))
            {
                viewModel.CreateRoute(value);
            }
        }
    }
}