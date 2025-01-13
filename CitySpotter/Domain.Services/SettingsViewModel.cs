using CitySpotter.Infrastructure;
using CitySpotter.Resources.Languages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitySpotter.Domain.Services
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name = LocalizationResources.Instance["name"].ToString();

        public LocalizationResources LocalizationResources => LocalizationResources.Instance;
        public SettingsViewModel() 
        {
        }

        [RelayCommand]
        private void Button_Clicked()
        {
            Debug.WriteLine(name);
            var switchToCulture = AppResources.Culture.TwoLetterISOLanguageName
                .Equals("nl", StringComparison.InvariantCultureIgnoreCase) ?
                new CultureInfo("en-US") : new CultureInfo("nl-NL");

            LocalizationResources.SetCulture(switchToCulture);

            name = LocalizationResources["name"].ToString();
        }

        [RelayCommand]
        private void NederlandsButton_Clicked()
        {
            Debug.WriteLine("Nederlands");
            LocalizationResources.SetCulture(new CultureInfo("nl-NL"));
            name = LocalizationResources["name"].ToString();
        }

        [RelayCommand]
        private void EnglishButton_Clicked()
        {
            Debug.WriteLine("English");
            LocalizationResources.SetCulture(new CultureInfo("en-US"));
            name = LocalizationResources["name"].ToString();
        }
    }
}
