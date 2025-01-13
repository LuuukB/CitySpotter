using CitySpotter.Domain.Services;
using Microsoft.Extensions.Logging;
using CitySpotter.Domain.Model;
using CitySpotter.Infrastructure;
using Mopups.Hosting;
using CommunityToolkit.Maui;
using CitySpotter.Domain.Services.Internet;
using CitySpotter.Domain.Services.FileServices;

namespace CitySpotter
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .ConfigureMopups()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
  
                });


            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "route.db");

            builder.Services.AddSingleton<IDatabaseRepo>(s =>
                new DatabaseRepo(dbPath));

            builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);

            builder.Services.AddSingleton<MapViewModel>();
            builder.Services.AddSingleton<MapPage>(s => new MapPage(s.GetRequiredService<MapViewModel>()));
            builder.Services.AddTransient<InfoPopupViewModel>();
            builder.Services.AddTransient<InfoPointPopup>();

            builder.Services.AddSingleton<ILocationPermissionsService, LocationPermissionService>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<SettingsPage>();


            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainPageViewModel>();

            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
            builder.Services.AddSingleton<IInternetHandler, InternetHandler>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
