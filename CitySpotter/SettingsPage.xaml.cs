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

		WeakReferenceMessenger.Default.Register<ThemeChangedMessage>(this, (r, m) => 
		{
			setTheme(m.Value);
		});

		var theme = Preferences.Get("theme", "System");

		setTheme("NormalMode");
	}

	public void setTheme(string theme)
	{
        if (theme == "System")
        {
            //theme = Current.PlatformAppTheme.ToString();
        }

        ResourceDictionary dictionary = theme switch
        {
            "ColorBlindMode" => new Resources.Styles.ColorBlindMode(),
            "NormalMode" => new Resources.Styles.NormalMode()
        };

        if (dictionary != null)
        {
            Resources.MergedDictionaries.Clear();

            Resources.MergedDictionaries.Add(dictionary);
        }
    }
}