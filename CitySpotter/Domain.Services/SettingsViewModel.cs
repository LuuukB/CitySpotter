using CitySpotter.Domain.Model;
using CitySpotter.Infrastructure;
using CitySpotter.Resources.Languages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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
        private string pickerItem;

        public LocalizationResources LocalizationResources => LocalizationResources.Instance;

        public bool colorPickerEnabled = true;

        public SettingsViewModel()
        {
            Debug.WriteLine(CultureInfo.CurrentCulture);
            if (CultureInfo.CurrentCulture.Equals(new CultureInfo("nl-NL")))
            {
                PickerItem = "Nederlands";
            } else
            {
                PickerItem = "English";
            }
        }

        [RelayCommand]
        private void Button_Clicked()
        {
            if (colorPickerEnabled)
            {
                Preferences.Set("theme", "ColorBlindMode");
                WeakReferenceMessenger.Default.Send(new ThemeChangedMessage("ColorBlindMode"));
            } else
            {
                Preferences.Set("theme", "NormalMode");
                WeakReferenceMessenger.Default.Send(new ThemeChangedMessage("NormalMode"));
            }
            colorPickerEnabled = !colorPickerEnabled;
        }

        [RelayCommand]
        private void LanguageChange()
        {
            Debug.WriteLine(PickerItem);
            if (pickerItem.Equals("Nederlands"))
            {
                LocalizationResources.SetCulture(new CultureInfo("nl-NL"));
                Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            } else
            {
                LocalizationResources.SetCulture(new CultureInfo("en-US"));
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            }
        }
    }
}
