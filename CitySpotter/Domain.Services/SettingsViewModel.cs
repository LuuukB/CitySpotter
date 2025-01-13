using CitySpotter.Domain.Model;
using CitySpotter.Infrastructure;
using CitySpotter.Resources.Languages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CitySpotter.Domain.Services
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name = LocalizationResources.Instance["name"].ToString();

        [ObservableProperty]
        private string pageTheme = "Resources/Styles/NormalMode.xaml";
        //private ResourceDictionary pageTheme = new Resources.Styles.NormalMode();

        public LocalizationResources LocalizationResources => LocalizationResources.Instance;

        public bool isis = true;

        [RelayCommand]
        private void Button_Clicked()
        {
            if (isis)
            {
                Preferences.Set("theme", "ColorBlindMode");
                WeakReferenceMessenger.Default.Send(new ThemeChangedMessage("ColorBlindMode"));
            } else
            {
                Preferences.Set("theme", "NormalMode");
                WeakReferenceMessenger.Default.Send(new ThemeChangedMessage("NormalMode"));
            }
            isis = !isis;
            //Preferences.Set("theme", "ColorBlindMode");
            //WeakReferenceMessenger.Default.Send(new ThemeChangedMessage("ColorBlindMode"));
            //LoadTheme("ColorBlindMode");
        }

        [RelayCommand]
        private void NederlandsButton_Clicked()
        {
            Debug.WriteLine("Nederlands");
            LocalizationResources.SetCulture(new CultureInfo("nl-NL"));
            Name = LocalizationResources["name"].ToString();
        }

        [RelayCommand]
        private void EnglishButton_Clicked()
        {
            Debug.WriteLine("English");
            LocalizationResources.SetCulture(new CultureInfo("en-US"));
            Name = LocalizationResources["name"].ToString();
        }

        private void LoadTheme(string theme)
        {
            ResourceDictionary dict = theme switch
            {
                "ColorBlindMode" => new Resources.Styles.ColorBlindMode(),
                "NormalMode" => new Resources.Styles.NormalMode()
            };

            if (dict != null)
            {
                //Resources.MergedDictionaries.Clear();
                //Resources.MergedDictionaries.Add(dict);
                //PageTheme = dict;
                
                PageTheme = "Resources/Styles/ColorBlindMode.xaml";
            }
        }
    }
}
