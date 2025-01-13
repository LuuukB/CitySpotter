using CitySpotter.Domain.Services.FileServices;
using CitySpotter.Domain.Model;
using CommunityToolkit.Mvvm.ComponentModel;


namespace CitySpotter.Domain.Services
{
    public partial class InfoPopupViewModel : ObservableObject
    {
        private RouteLocation routeLocation { get; set; }
        private IFileService _fileService { get; set; }
        public InfoPopupViewModel(RouteLocation location, IFileService fileService) { 
            routeLocation = location;
            _fileService = fileService;
        }
        [ObservableProperty] private string _ImageSource;
        [ObservableProperty] private string _locationName;
        [ObservableProperty] private string _Description;

        public async Task setData()
        {
            ImageSource = routeLocation.imageSource;
            LocationName = routeLocation.name;
            Description = await _fileService.ReadFileAsync(routeLocation.description);
        }
    }
}
