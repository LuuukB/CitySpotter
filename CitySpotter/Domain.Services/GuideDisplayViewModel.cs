using CommunityToolkit.Mvvm.ComponentModel;
using CitySpotter.Domain.Services.FileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CitySpotter.Domain.Services
{
    public partial class GuideDisplayViewModel : ObservableObject
    {
        [ObservableProperty] private string _GuideContent;
        private string guideNL = "handleiding.txt";
        private string guideENG = "handleidingENG.txt";
        private IFileService fileSource;

        public GuideDisplayViewModel(IFileService fileSource)
        {
            this.fileSource = fileSource;
        }
        public async Task setData()
        {
            if (CultureInfo.CurrentCulture.Equals(new CultureInfo("nl-NL")))
            {

                GuideContent = await fileSource.ReadFileAsync(guideNL);
            }
            else
            {

                GuideContent = await fileSource.ReadFileAsync(guideENG);
            }
        }
    }
}
