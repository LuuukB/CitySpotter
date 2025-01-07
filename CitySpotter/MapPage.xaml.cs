using CitySpotter.Domain.Services;
using Microsoft.Maui.Controls.Maps;

namespace CitySpotter;

public partial class MapPage : ContentPage
{
	public MapPage(MapViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        createPins();
	}

    public async Task<PermissionStatus> CheckAndRequestLocationPermission()
    {

        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status == PermissionStatus.Granted)
            return status;


        if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
        {
            return status;
        }

        if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.Android)
        {

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {

                bool shouldContinue = await DisplayAlert(
                    "Locatie vereist",
                    "De app heeft je locatie nodig anders kan de app niet goed werken.",
                    "Toestaan",
                    "Annuleren"
                );

                if (shouldContinue)
                {

                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }
                else
                {

                    return PermissionStatus.Denied;
                }
            }
            else
            {

                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
        }

        return status;
    }


    //overides voor bij het opstarten van de mainpage. 
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var status = await CheckAndRequestLocationPermission();


        if (status == PermissionStatus.Denied)
        {

            await DisplayAlert("Locatie permissie vereist", "De app heeft toegang tot je locatie nodig om goed te functioneren.", "Ok");

            var shouldOpenSettings = await DisplayAlert(
                "Locatie permissie vereist",
                "De app heeft toegang tot je locatie nodig om goed te functioneren. Ga naar de instellingen om deze in te schakelen.",
                "Naar Instellingen",
                "Annuleren"
            );
            if (shouldOpenSettings)
            {

#if ANDROID
  var context = Android.App.Application.Context;
        var intent = new Android.Content.Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
        intent.AddFlags(Android.Content.ActivityFlags.NewTask);
        var uri = Android.Net.Uri.FromParts("package", context.PackageName, null);
        intent.SetData(uri);
        context.StartActivity(intent);
#endif
            }
            else
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
    }

    public void createPins()
    {
        MapView.Pins.Add(new Pin
        {
            Label = "Monument ValkenbergPark",
            Location = new Location(51.592496, 4.77975 )
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Vuurtoren",
            Location = new Location(51.592833, 4.77872)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Kasteel van Breda",
            Location = new Location(51.590612, 4.776167)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "StadhoudersPoort/Beeld Willem III",
            Location = new Location(51.592496, 4.77975)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Huis van Brecht", //Huis aan de rechterkant
            Location = new Location(51.590028, 4.774362)
        });


        MapView.Pins.Add(new Pin
        {
            Label = "Spanjaardsgat", //langs het water aan de rechter kant
            Location = new Location(51.590195, 4.773445)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Vismarkt",
            Location = new Location(51.589833, 4.773333)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Havermarkt",
            Location = new Location(51.589362, 4.774445)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Grote kerk",
            Location = new Location(51.588833, 4.775278)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Het poortje", //Op streetview geen poortje te zien?
            Location = new Location(51.592496, 4.77975)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "RidderStraat",
            Location = new Location(51.587083, 4.775750)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Grote markt",
            Location = new Location(51.587417, 4.776555)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Bevrijdingsmonument", //Midden op de markt
            Location = new Location(51.588028, 4.776333)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Oud stadhuis", //Aan de rechterkant
            Location = new Location(51.588750, 4.776112)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Antonius van PaduaKerk", //Aan de rechterkant
            Location = new Location(51.5876638, 4.777250)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Bibliotheek", //Aan de rechterkant
            Location = new Location(51.588000, 4.778945)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Kloosterkazerne (Holland casino)", //Aan de rechterkant
            Location = new Location(51.592496, 4.77975)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Chasse theater", //Aan de rechterkant, aangebouwd in kloosterkazerne
            Location = new Location(51.587750, 4.782000)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Beyerd", // Aan de rechterkant
            Location = new Location(51.589667, 4.781000)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Gasthuispoort",
            Location = new Location(51.589555, 4.780000)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Willen Merkxtuin", //ingang aan de rechterkant
            Location = new Location(51.589112, 4.777945)
        });

        MapView.Pins.Add(new Pin
        {
            Label = "Begijnenhof",
            Location = new Location(51.589695, 4.778362)
        });

    }
}