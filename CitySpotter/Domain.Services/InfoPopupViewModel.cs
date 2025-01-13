using CitySpotter.Domain.Services.FileServices;
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
        private IFileService _fileService { get; set; }
        public InfoPopupViewModel(RouteLocation location, IFileService fileService) { 
            routeLocation = location;
        }
        [ObservableProperty] private string _ImageSource;
        [ObservableProperty] private string locationName;
        [ObservableProperty] private string _Description;

        public async Task setData()
        {
            ImageSource = routeLocation.imageSource;
            LocationName = routeLocation.name;

            Description = await _fileService.ReadFileAsync(routeLocation.description);
            
        }
    }
}
