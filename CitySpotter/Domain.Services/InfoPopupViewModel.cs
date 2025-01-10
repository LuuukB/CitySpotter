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
            setImage();
        }

        [ObservableProperty]
        private string _ImageSource;
        [ObservableProperty]
        private string locationName;

        public void setImage()
        {
            _ImageSource = routeLocation.imageSource;
        }
    }
}
