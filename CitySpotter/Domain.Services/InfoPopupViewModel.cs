using CitySpotter.Infrastructure;
using CitySpotter.Locations.Locations;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitySpotter.Domain.Services
{
    public partial class InfoPopupViewModel : ObservableObject
    {
        private RouteLocation routeLocation { get; set; }
        public InfoPopupViewModel(RouteLocation location) { 
            routeLocation = location;
            setData();
        }

        [ObservableProperty]
        private string _ImageSource;
        [ObservableProperty]
        private string locationName;
        [ObservableProperty]
        private string _Description;

        public async void setData()
        {
            _ImageSource = routeLocation.imageSource;

            using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(routeLocation.description);
            using StreamReader reader = new StreamReader(fileStream);

            for (int i = 0; i < reader.Read(); i++)
            {
                _Description  += reader.ReadLine();
            }
        }
    }
}
