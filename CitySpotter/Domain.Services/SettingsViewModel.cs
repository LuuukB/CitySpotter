
using CitySpotter.Domain.Model;
using CitySpotter.Domain.Services.FileServices;
using CitySpotter.Infrastructure;
using CitySpotter.Resources.Languages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace CitySpotter.Domain.Services
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string languagePickerItem;

        [ObservableProperty]
        private string themePickerItem;

        public LocalizationResources LocalizationResources => LocalizationResources.Instance;

        public bool colorPickerEnabled = true;

        public SettingsViewModel()
        {
            Debug.WriteLine(CultureInfo.CurrentCulture);
            if (CultureInfo.CurrentCulture.Equals(new CultureInfo("nl-NL")))
            {
                LanguagePickerItem = "Nederlands";
            } else
            {
                LanguagePickerItem = "English";
            }

            ThemePickerItem = "Standard";

        }

        [RelayCommand]
        private void ThemeChange()
        {
            Debug.WriteLine(ThemePickerItem);
            if (ThemePickerItem.Equals("Standard"))
            {
                Preferences.Set("theme", "NormalMode");
                WeakReferenceMessenger.Default.Send(new ThemeChangedMessage("NormalMode"));
            }
            else if (ThemePickerItem.Equals("Deuteranopie"))
            {
                Preferences.Set("theme", "DeuteranopieMode");
                WeakReferenceMessenger.Default.Send(new ThemeChangedMessage("DeuteranopieMode"));
            } 
            else if (ThemePickerItem.Equals("Anatropie"))
            {
                Preferences.Set("theme", "AnatropieMode");
                WeakReferenceMessenger.Default.Send(new ThemeChangedMessage("AnatropieMode"));
            }
        }

        [RelayCommand]
        private void LanguageChange()
        {
            Debug.WriteLine(LanguagePickerItem);
            if (LanguagePickerItem.Equals("Nederlands"))
            {
                LocalizationResources.SetCulture(new CultureInfo("nl-NL"));
                Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
            } else
            {
                LocalizationResources.SetCulture(new CultureInfo("en-US"));
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            }
        }
        [RelayCommand]
        private void GuideButton()
        {
            IFileService fileService = new FileService();
            var viewModel = new GuideDisplayViewModel(fileService);
            viewModel.setData();

            try
            {
                MopupService.Instance.PushAsync(new GuideDisplay(viewModel));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString);
            }
        }
    }
}
