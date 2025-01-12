using CitySpotter.Domain.Services;
using CitySpotter.Locations.Locations;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Maps;
using Mopups.Services;
using System.Diagnostics;

namespace CitySpotter;
[QueryProperty(nameof(RouteName), "routeName")]
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

    public string RouteName
    {
        get => _routeName;
        set
        {
            _routeName = value;
            if (BindingContext is MapViewModel viewModel && !string.IsNullOrEmpty(value))
            {
                viewModel.LoadRoute(value);
            }
        }
    }
    private string _routeName;
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
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.592833, latitude = 4.779975, name = "Vuurtoren", description = "vuurtorenbeschrijving.txt", imageSource = "vuurtoren.jpg" })));
            Debug.WriteLine($"{vuurtorenPin.Label}");
        };
        MapView.Pins.Add(vuurtorenPin);

        Pin KasteelPin = new Pin
        {
            Label = "Kasteel van Breda",
            Location = new Location(51.590612, 4.776167),
            Type = PinType.Generic
        };

        KasteelPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation {  longitude = 51.590612, latitude = 4.776167, name = "Kasteel van Breda", description = "kasteelbeschrijving.txt",imageSource = "kasteelbreda.jpg"})));
            Debug.WriteLine($"{KasteelPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(KasteelPin);

        Pin stadhoudersPoortPin = new Pin
        {
            Label = "Stadhouderspoort",
            Location = new Location(51.592496, 4.77975),
            Type = PinType.Generic
        };

        stadhoudersPoortPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.589695, latitude = 4.776138, name = "Stadhouderspoort", description = "stadhouderspoortbeschrijving.txt", imageSource = "stadhouderspoort.jpg", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{stadhoudersPoortPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(stadhoudersPoortPin);

        Pin huisVanBrecht = new Pin
        {
            Label = "Huis van Brecht",
            Location = new Location(51.590028, 4.774362),
            Type = PinType.Generic
        };

        huisVanBrecht.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.590028, latitude = 4.774362, name = "Huis van Brecht", description = "huisvanbrechtbeschrijving.txt", imageSource = "huisvanbrecht7.jpg", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{huisVanBrecht.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(huisVanBrecht);

        Pin spanjaardsgat = new Pin
        {
            Label = "Spanjaardsgat",
            Location = new Location(51.590195, 4.773445),
            Type = PinType.Generic
        };

        spanjaardsgat.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.590195, latitude = 4.773445, name = "Spanjaardsgat", description = "spanjaardsgatbeschrijving.txt", imageSource = "spanjaardsgat.jpg", routeTag = "historischeKilometer" })));

            Debug.WriteLine($"{spanjaardsgat.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(spanjaardsgat);

        Pin vismarkt = new Pin
        {
            Label = "Vismarkt",
            Location = new Location(51.589833, 4.773333),
            Type = PinType.Generic
        };

        vismarkt.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.589833, latitude = 4.773333, name = "Vismarkt", description = "vismarktbeschrijving.txt", imageSource = "vismarktbreda.jpg", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{vismarkt.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(vismarkt);

        Pin havermarktPin = new Pin
        {
            Label = "Havermarkt",
            Location = new Location(51.589362, 4.774445),
            Type = PinType.Generic
        };
        havermarktPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.589362, latitude = 4.774445, name = "Havermarkt", description = "havermarktbeschrijving.txt", imageSource = "havermarkt.webp", routeTag = "historischeKilometer" })));
        };
        MapView.Pins.Add(havermarktPin);


        Pin groteKerk = new Pin
        {
            Label = "Grote kerk",
            Location = new Location(51.588833, 4.775278),
            Type = PinType.Generic
        };

        groteKerk.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.588833, latitude = 4.775278, name = "Grote Kerk Breda", description = "grotekerkbeschrijving.txt", imageSource = "grotekerk.jpg", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{groteKerk.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(groteKerk);

        Pin hetPoortje = new Pin
        {
            Label = "Het poortje",
            Location = new Location(51.587907, 4.775381),
            Type = PinType.Generic
        };

        hetPoortje.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.588195, latitude = 4.775138, name = "Het Poortje", description = "poortjebeschrijving.txt", imageSource = "hetpoortje.jpg", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{hetPoortje.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(hetPoortje);

        Pin ridderstraatPin = new Pin
        {
            Label = "Ridderstraat",
            Location = new Location(51.587083, 4.775750),
            Type = PinType.Generic
        };

        ridderstraatPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.587083, latitude = 4.775750, name = "Ridderstraat", description = "ridderstraatbeschrijving.txt", imageSource = "ridderstraat.jpg", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{ridderstraatPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(ridderstraatPin);

        Pin groteMarktPin = new Pin
        {
            Label = "Grote markt",
            Location = new Location(51.587417, 4.776555),
            Type = PinType.Generic
        };

        groteMarktPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.587417, latitude = 4.776555, name = "De Grote Markt", description = "grotemarktbeschrijving.txt", imageSource = "grotemarktbreda.jpg", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{groteMarktPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(groteMarktPin);

        Pin bevrijdingsMonumentPin = new Pin
        {
            Label = "Bevrijdingsmonument",
            Location = new Location(51.588028, 4.776333),
            Type = PinType.Generic
        };

        bevrijdingsMonumentPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.588028, latitude = 4.776333, name = "Bevrijdingsmonument", description = "bevrijdingmonumentbeschrijving.txt", imageSource = "bevrijdingsmonument.jpg", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{bevrijdingsMonumentPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(bevrijdingsMonumentPin);

        Pin oudStadhuisPin = new Pin
        {
            Label = "Het oude stadhuis",
            Location = new Location(51.588750, 4.776112),
            Type = PinType.Generic
        };

        oudStadhuisPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.588750, latitude = 4.776112, name = "Stadhuis", description = "stadhuisbeschrijving.txt", imageSource = "stadhuis.jpg", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{oudStadhuisPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(oudStadhuisPin);

        Pin antoniusVanPaduaKerkPin = new Pin
        {
            Label = "De Antonius van Padua Kerk",
            Location = new Location(51.5876638, 4.777250),
            Type = PinType.Generic
        };

        antoniusVanPaduaKerkPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.587638, latitude = 4.777250, name = "Antonius van Padua Kerk", description = "antoniusvanpadaukerkbeschrijving.txt", imageSource = "paduakerk.jpg", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{antoniusVanPaduaKerkPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(antoniusVanPaduaKerkPin);

        Pin bibliotheekPin = new Pin
        {
            Label = "Bibliotheek",
            Location = new Location(51.588000, 4.778945),
            Type = PinType.Generic
        };

        bibliotheekPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.588000, latitude = 4.778945, name = "Bibliotheek", description = "bibliotheekbeschrijving.txt", imageSource = "bibliotheekbreda.webp", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{bibliotheekPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(bibliotheekPin);

        Pin KloosterKazernePin = new Pin
        {
            Label = "Kloosterkazerne",
            Location = new Location(51.587722, 4.781028),
            Type = PinType.Generic
        };

        KloosterKazernePin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.587722, latitude = 4.781028, name = "Kloosterkazerne", description = "kloosterkazernebeschrijving.txt", imageSource = "kloosterkazerne.jpg", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{KloosterKazernePin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(KloosterKazernePin);

        Pin ChasseTheaterPin = new Pin
        {
            Label = "Chassé theater",
            Location = new Location(51.587750, 4.782000),
            Type = PinType.Generic
        };

        ChasseTheaterPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.587750, latitude = 4.782000, name = "Chassé theater", description = "chassetheaterbeschrijving.txt", imageSource = "chassetheater.jpg", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{ChasseTheaterPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(ChasseTheaterPin);

        Pin BeyerdPin = new Pin
        {
            Label = "Beyerd",
            Location = new Location(51.589667, 4.781000),
            Type = PinType.Generic
        };

        BeyerdPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.589667, latitude = 4.781000, name = "Beyerd", description = "beyerdbeschrijving.txt", imageSource = "beyerd.jpg", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{BeyerdPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(BeyerdPin);

        Pin gastHuisPoortPin = new Pin
        {
            Label = "Gasthuispoort",
            Location = new Location(51.589555, 4.780000),
            Type = PinType.Generic
        };

        gastHuisPoortPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.589555, latitude = 4.780000, name = "Gasthuispoort", description = "gasthuispoortbeschrijving.txt", imageSource = "gasthuispoort.jpg", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{gastHuisPoortPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(gastHuisPoortPin);

        Pin willemMerkxTuin = new Pin
        {
            Label = "Willem Merkxtuin",
            Location = new Location(51.589112, 4.777945),
            Type = PinType.Generic
        };

        willemMerkxTuin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.589112, latitude = 4.777945, name = "Willem Merkxtuin", description = "willemmerkxtuinbeschrijving.txt", imageSource = "willemmerkxtuin.webp", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{willemMerkxTuin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(willemMerkxTuin);

        //Om aan te geven dat de code zo eerst was opgesteld met deze modules, wij hebben dit nu opgeschaald naar meteen een actie afhandeling eraan te koppelen.
        //MapView.Pins.Add(new Pin
        //{
        //    Label = "Begijnenhof",
        //    Location = new Location(51.589695, 4.778362)
        //});

        Pin BegijnenhofPin = new Pin
        {
            Label = "Het begijnhof",
            Location = new Location(51.589695, 4.778362),
            Type = PinType.Generic
        };

        BegijnenhofPin.MarkerClicked += async (s, args) =>
        {
            await MopupService.Instance.PushAsync(new InfoPointPopup(new InfoPopupViewModel(new RouteLocation { longitude = 51.589695, latitude = 4.778362, name = "Begijnhof", description = "begijnhofbeschrijving.txt", imageSource = "begijnhof.jpg", routeTag = "historischeKilometer" })));
            Debug.WriteLine($"{BegijnenhofPin.Label} is ingedrukt: ");
        };
        MapView.Pins.Add(BegijnenhofPin);

    }

    private void MapView_DescendantAdded(object sender, ElementEventArgs e)
    {

    }
}