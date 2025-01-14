using CitySpotter.Domain.Model;
using CitySpotter.Domain.Services;
using CommunityToolkit.Mvvm.Messaging;
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