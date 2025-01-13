using CitySpotter.Domain.Services;
using System.Diagnostics;

namespace CitySpotter;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}