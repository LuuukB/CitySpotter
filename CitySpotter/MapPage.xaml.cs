using CitySpotter.Domain.Services;
using Microsoft.Maui.Controls.Maps;

namespace CitySpotter;
[QueryProperty(nameof(RouteName), "routeName")]
public partial class MapPage : ContentPage
{
    private string _routeName;
    private MapViewModel _mapViewModel;
    public MapPage(MapViewModel viewModel)
	{
		InitializeComponent();
        _mapViewModel = viewModel;
        _mapViewModel.InternetConnectionLost += async (sender, message) =>
        {
            await DisplayAlert("Geen verbinding", message, "OK");
        };
        _mapViewModel.LocationLost += async (sender, message) =>
        {
            await DisplayAlert("Geen locatie", message, "OK");
        };

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
                viewModel.OnLoad(value);
            }
        }
    }

    private void Pin_OnMarkerClicked(object? sender, PinClickedEventArgs e)
    {
        if (this.BindingContext is MapViewModel viewModel)
        {
            if (sender is Pin pin)
            {
                viewModel.MarkerClickedCommand.Execute(pin);
            }
        }
    }
}