using CitySpotter.Infrastructure;
using CitySpotter.Resources.Languages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Globalization;

namespace CitySpotter.Domain.Services
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name = LocalizationResources.Instance["name"].ToString();

        public LocalizationResources LocalizationResources => LocalizationResources.Instance;
      
        [RelayCommand]
        private void Button_Clicked()
        {
            Debug.WriteLine(Name);
            var switchToCulture = AppResources.Culture.TwoLetterISOLanguageName
                .Equals("nl", StringComparison.InvariantCultureIgnoreCase) ?
                new CultureInfo("en-US") : new CultureInfo("nl-NL");

            LocalizationResources.SetCulture(switchToCulture);

            Name = LocalizationResources["name"].ToString();
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
    }
}
