using CitySpotter.Domain.Services;
using CitySpotter.Locations.Locations;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using Mopups.Services;
using System.Diagnostics;

namespace CitySpotter;

public partial class MapPage : ContentPage
{
    
	public MapPage(MapViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;

        Location location = new Location(51.588331, 4.777802);
        MapSpan mapSpan = new MapSpan(location, 0.015, 0.015);
        MapView.VisibleRegion = mapSpan;

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
        
        Pin valkenBergParkPin = new Pin
        {
            Label = "Monument ValkenbergPark",
            Location = new Location(51.592496, 4.77975),
            Type = PinType.Generic
        };

        valkenBergParkPin.MarkerClicked += async (s, args) =>
        {
            
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.592496, latitude = 4.779975, name = "Monument ValkenburgPark", description = "nassaubaroniemonumentbeschrijving.txt", imageSource = "nassaubaroniemonument.jpg" })));
            Debug.WriteLine($"{valkenBergParkPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add( valkenBergParkPin );

        
        Pin vuurtorenPin = new Pin
        {
            Label = "Vuurtoren",
            Location = new Location(51.592833, 4.77872),
            Type = PinType.SavedPin
        };
        vuurtorenPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.592833, latitude = 4.779975, name = "Vuurtoren", description = "Breda ligt niet aan de haven", imageSource = "vuurtoren.jpg" })));
            Debug.WriteLine($"{vuurtorenPin.Label}");
        };
        MapView.Pins.Add(vuurtorenPin);

        /*MapView.Pins.Add(new Pin
        {
            Label = "Vuurtoren",
            Location = new Location(51.592833, 4.77872)
        });

        Pin KasteelPin = new Pin
        {
            Label = "Kasteel van Breda",
            Location = new Location(51.590612, 4.776167),
            Type = PinType.Generic
        };

        KasteelPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{KasteelPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(KasteelPin);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Kasteel van Breda",
        //    Location = new Location(51.590612, 4.776167)
        //});

        Pin stadhoudersPoortPin = new Pin
        {
            Label = "Stadhouderspoort",
            Location = new Location(51.592496, 4.77975),
            Type = PinType.Generic
        };

        stadhoudersPoortPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{stadhoudersPoortPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(stadhoudersPoortPin);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "StadhoudersPoort/Beeld Willem III",
        //    Location = new Location(51.592496, 4.77975)
        //});

        Pin huisVanBrecht = new Pin
        {
            Label = "Huis van Brecht",
            Location = new Location(51.590028, 4.774362),
            Type = PinType.Generic
        };

        huisVanBrecht.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{huisVanBrecht.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(huisVanBrecht);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Huis van Brecht", //Huis aan de rechterkant
        //    Location = new Location(51.590028, 4.774362)
        //});

        Pin spanjaardsgat = new Pin
        {
            Label = "Spanjaardsgat",
            Location = new Location(51.590195, 4.773445),
            Type = PinType.Generic
        };

        spanjaardsgat.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{spanjaardsgat.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(spanjaardsgat);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Spanjaardsgat", //langs het water aan de rechter kant
        //    Location = new Location(51.590195, 4.773445)
        //});

        Pin vismarkt = new Pin
        {
            Label = "Vismarkt",
            Location = new Location(51.589833, 4.773333),
            Type = PinType.Generic
        };

        vismarkt.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{vismarkt.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(vismarkt);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Vismarkt",
        //    Location = new Location(51.589833, 4.773333)
        //});

        MapView.Pins.Add(new Pin
        {
            Label = "Havermarkt",
            Location = new Location(51.589362, 4.774445)
        });

        Pin groteKerk = new Pin
        {
            Label = "Grote kerk",
            Location = new Location(51.588833, 4.775278),
            Type = PinType.Generic
        };

        groteKerk.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{groteKerk.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(groteKerk);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Grote kerk",
        //    Location = new Location(51.588833, 4.775278)
        //});

        Pin hetPoortje = new Pin
        {
            Label = "Het poortje",
            Location = new Location(51.587907, 4.775381),
            Type = PinType.Generic
        };

        hetPoortje.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{hetPoortje.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(hetPoortje);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Het poortje", //Op streetview geen poortje te zien?
        //    Location = new Location(51.592496, 4.77975)
        //});

        Pin ridderstraatPin = new Pin
        {
            Label = "Ridderstraat",
            Location = new Location(51.587083, 4.775750),
            Type = PinType.Generic
        };

        ridderstraatPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{ridderstraatPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(ridderstraatPin);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "RidderStraat",
        //    Location = new Location(51.587083, 4.775750)
        //});

        Pin groteMarktPin = new Pin
        {
            Label = "Grote markt",
            Location = new Location(51.587417, 4.776555),
            Type = PinType.Generic
        };

        groteMarktPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{groteMarktPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(groteMarktPin);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Grote markt",
        //    Location = new Location(51.587417, 4.776555)
        //});

        Pin bevrijdingsMonumentPin = new Pin
        {
            Label = "Bevrijdingsmonument",
            Location = new Location(51.588028, 4.776333),
            Type = PinType.Generic
        };

        bevrijdingsMonumentPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{bevrijdingsMonumentPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(bevrijdingsMonumentPin);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Bevrijdingsmonument", //Midden op de markt
        //    Location = new Location(51.588028, 4.776333)
        //});

        Pin oudStadhuisPin = new Pin
        {
            Label = "Het oude stadhuis",
            Location = new Location(51.588750, 4.776112),
            Type = PinType.Generic
        };

        oudStadhuisPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{oudStadhuisPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(oudStadhuisPin);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Oud stadhuis", //Aan de rechterkant
        //    Location = new Location(51.588750, 4.776112)
        //});

        Pin antoniusVanPaduaKerkPin = new Pin
        {
            Label = "De Antonius van Pardua Kerk",
            Location = new Location(51.5876638, 4.777250),
            Type = PinType.Generic
        };

        antoniusVanPaduaKerkPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{antoniusVanPaduaKerkPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(antoniusVanPaduaKerkPin);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Antonius van PaduaKerk", //Aan de rechterkant
        //    Location = new Location(51.5876638, 4.777250)
        //});

        Pin bibliotheekPin = new Pin
        {
            Label = "Bibliotheek",
            Location = new Location(51.588000, 4.778945),
            Type = PinType.Generic
        };

        bibliotheekPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{bibliotheekPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(bibliotheekPin);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Bibliotheek", //Aan de rechterkant
        //    Location = new Location(51.588000, 4.778945)
        //});

        Pin KloosterKazernePin = new Pin
        {
            Label = "Kloosterkazerne",
            Location = new Location(51.587722, 4.781028),
            Type = PinType.Generic
        };

        KloosterKazernePin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{KloosterKazernePin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(KloosterKazernePin);


        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Kloosterkazerne (Holland casino)", //Aan de rechterkant
        //    Location = new Location(51.592496, 4.77975)
        //});

        Pin ChasseTheaterPin = new Pin
        {
            Label = "Chassé theater",
            Location = new Location(51.587750, 4.782000),
            Type = PinType.Generic
        };

        ChasseTheaterPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{ChasseTheaterPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(ChasseTheaterPin);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Chasse theater", //Aan de rechterkant, aangebouwd in kloosterkazerne
        //    Location = new Location(51.587750, 4.782000)
        //});

        Pin BeyerdPin = new Pin
        {
            Label = "Beyerd",
            Location = new Location(51.589667, 4.781000),
            Type = PinType.Generic
        };

        BeyerdPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{BeyerdPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(BeyerdPin);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Beyerd", // Aan de rechterkant
        //    Location = new Location(51.589667, 4.781000)
        //});

        Pin gastHuisPoortPin = new Pin
        {
            Label = "Gasthuispoort",
            Location = new Location(51.589555, 4.780000),
            Type = PinType.Generic
        };

        gastHuisPoortPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{gastHuisPoortPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(gastHuisPoortPin);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Gasthuispoort",
        //    Location = new Location(51.589555, 4.780000)
        //});

        Pin willemMerkxTuin = new Pin
        {
            Label = "Willem Merkxtuin",
            Location = new Location(51.589112, 4.777945),
            Type = PinType.Generic
        };

        willemMerkxTuin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{willemMerkxTuin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(willemMerkxTuin);

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Willen Merkxtuin", //ingang aan de rechterkant
        //    Location = new Location(51.589112, 4.777945)
        //});

        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Begijnenhof",
        //    Location = new Location(51.589695, 4.778362)
        //});

        Pin BegijnenhofPin = new Pin
        {
            Label = "Het begijnenhof",
            Location = new Location(51.589695, 4.778362),
            Type = PinType.Generic
        };

        BegijnenhofPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup());
            Debug.WriteLine($"{BegijnenhofPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(BegijnenhofPin);
        */

    }

    private void MapView_DescendantAdded(object sender, ElementEventArgs e)
    {

    }
}