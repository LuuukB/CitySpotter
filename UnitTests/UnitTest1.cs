using CitySpotter.Domain.Services.FileServices;
using CitySpotter.Domain.Services;
using CitySpotter.Domain.Model;
using Moq;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Controls.Maps;
using Syncfusion.Maui.Core.Carousel;
using CitySpotter.Domain.Services.Internet;
using CitySpotter.Infrastructure;
using System.Globalization;

namespace UnitTests
{
    public class UnitTest1
    { 

        [Fact]
        public async Task SetData_SetsImageSourceLocationNameAndDescriptionCorrectly()
        {

            Mock<IFileService>  _mockFileService = new Mock<IFileService>();

            _mockFileService.Setup(service => service.ReadFileAsync(It.IsAny<string>()))
                            .ReturnsAsync("Mock Description");

            RouteLocation _routeLocation = new RouteLocation
            {
                imageSource = "path/to/image.jpg",
                name = "Test Location",
                description = "path/to/description.txt"
            };

            InfoPopupViewModel _viewModel = new InfoPopupViewModel(_routeLocation, _mockFileService.Object);

            await _viewModel.setData();

            Assert.Equal(_routeLocation.imageSource, _viewModel.ImageSource);
            Assert.Equal(_routeLocation.name, _viewModel.LocationName);
            Assert.Equal("Mock Description", _viewModel.Description);
        }

        //[Fact]
        //public void VisibleRegionInMap_Changes_CallsMoveToRegion()
        //{
        //    // Arrange
        //    var map = new Mock<BindableMap>(); // Mocken van de BindableMap klasse
        //    var newRegion = new MapSpan(new Location(51.5074, -0.1278)); // Nieuwe MapSpan

        //    // Act
        //    map.Object.VisibleRegionInMap = newRegion;  // Zet de property VisibleRegionInMap

        //    // Assert
        //    map.Verify(m => m.MoveToRegion(newRegion), Times.Once);  // Verifieer of MoveToRegion eenmaal is aangeroepen
        //}

        [Fact]
        public void MvvmMapElementsProperty_Should_Update_MapElements()
        {

            // Arrange
            BindableMap _bindableMap = new BindableMap();
            var mapElements = new List<MapElement>
            {
                new Polyline { StrokeColor = Colors.Red, StrokeWidth = 2 }
            };

            // Act
            _bindableMap.MvvmMapElements = mapElements;

            // Assert
            Assert.Single(_bindableMap.MapElements);
            Assert.Same(mapElements[0], _bindableMap.MapElements.First());
        }

        [Fact]
        public void MvvmMapElementsProperty_Should_Clear_Old_Elements_Before_Adding_New()
        {
            // Arrange
            BindableMap _bindableMap = new BindableMap();
            var initialElements = new List<MapElement>
            {
                new Polyline { StrokeColor = Colors.Red, StrokeWidth = 2 }
            };
            var newElements = new List<MapElement>
            {
                new Polyline { StrokeColor = Colors.Blue, StrokeWidth = 2 }
            };

            _bindableMap.MvvmMapElements = initialElements;

            // Act
            _bindableMap.MvvmMapElements = newElements;

            // Assert
            Assert.Single(_bindableMap.MapElements);
            Assert.Equal(Colors.Blue, ((Polyline)_bindableMap.MapElements.First()).StrokeColor);
        }

        [Fact]
        public void HasInternetConnection_Should_Return_True_When_Internet_Is_Available()
        {
            // Arrange
            Mock<IGeolocation>  _mockGeolocation = new Mock<IGeolocation>();
            Mock<IDatabaseRepo>  _mockDatabaseRepo = new Mock<IDatabaseRepo>();
            Mock<IInternetHandler> _mockInternetHandler = new Mock<IInternetHandler>();

            MapViewModel _viewModel = new MapViewModel(_mockGeolocation.Object, _mockDatabaseRepo.Object, _mockInternetHandler.Object);

            _mockInternetHandler.Setup(i => i.HasInternetConnection()).Returns(true);

            // Act
            var result = _viewModel.HasInternetConnection;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetRoutesForView_LoadsRoutesAndAddsToRouteNames()
        {
            // Arrange
            Mock<IDatabaseRepo> _mockDatabaseRepo = new Mock<IDatabaseRepo>();
            Mock<ILocationPermissionsService>  _mockPermissionsService = new Mock<ILocationPermissionsService>();
            MainPageViewModel _viewModel = new MainPageViewModel(_mockDatabaseRepo.Object, _mockPermissionsService.Object);


            var mockRoutes = new List<string> { "Route1", "Route2", "Route3" };
            _mockDatabaseRepo.Setup(repo => repo.GetAllNamesRoutes())
                             .ReturnsAsync(mockRoutes);

            // Act
            _viewModel.GetRoutesForView();
            await Task.Delay(100);  // Wacht voor asynchrone methoden

            // Assert
            Assert.Equal(3, _viewModel.RouteNames.Count);
            Assert.Contains("Route1", _viewModel.RouteNames);
            Assert.Contains("Route2", _viewModel.RouteNames);
            Assert.Contains("Route3", _viewModel.RouteNames);
        }
        
    }
}