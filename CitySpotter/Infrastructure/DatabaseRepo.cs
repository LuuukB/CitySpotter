using CitySpotter.Domain.Model;
using SQLite;
using System.Diagnostics;


namespace CitySpotter.Infrastructure;

public class DatabaseRepo(string dbPath) : IDatabaseRepo
{
    private readonly SQLiteAsyncConnection _sQLiteConnection = new(dbPath);

    public async Task AddRoute(RouteLocation location)
    {
        await _sQLiteConnection.InsertAsync(location);
    }

    public async Task Delete(int id)
    {
        await _sQLiteConnection.DeleteAsync<RouteLocation>(id);
    }

    public async Task<List<RouteLocation>> GetAllRoutes()
    {
        return await _sQLiteConnection.Table<RouteLocation>().ToListAsync();
    }

    public async Task Init()
    {
        await _sQLiteConnection.CreateTableAsync<RouteLocation>();
        await AddHistoricalRoute();
    }

    private async Task AddHistoricalRoute()
    {
        var db = await GetAllRoutes();

        if (db.Count == 0)
        {
            await AddRoute(new RouteLocation
            {
                longitude = 51.594112,
                latitude = 4.779417,
                name = "VVV-Kantoor",
                descriptionNL = "vvvkantoorbeschrijving.txt",
                descriptionENG = "vvvkantoorbeschrijvingENG.txt",
                imageSource = "vvvkantoor.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.593278,
                latitude = 4.779388,
                name = "Liefdeszuster",
                descriptionNL = "liefdeszusterbeschrijving.txt",
                descriptionENG = "liefdeszusterbeschrijvingENG.txt",
                imageSource = "liefdeszuster.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.592500,
                latitude = 4.779695,
                name = "Nassau-Baroniemonument",
                descriptionNL = "nassaubaroniemonumentbeschrijving.txt",
                descriptionENG = "nassaubaroniemonumentbeschrijvingENG.txt",
                imageSource = "nassaubaroniemonument.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.592500, latitude = 4.779388, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.592833,
                latitude = 4.778472,
                name = "Vuurtoren",
                descriptionNL = "vuurtorenbeschrijving.txt",
                descriptionENG = "vuurtorenbeschrijvingENG.txt",
                imageSource = "vuurtoren.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.592667, latitude = 4.777917, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.590612, latitude = 4.777000, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.590612,
                latitude = 4.776167,
                name = "Kasteel Breda",
                descriptionNL = "kasteelbeschrijving.txt",
                descriptionENG = "kasteelbeschrijvingENG.txt",
                imageSource = "kasteelbreda.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.589695,
                latitude = 4.776138,
                name = "Stadhouderspoort",
                descriptionNL = "stadhouderspoortbeschrijving.txt",
                descriptionENG = "stadhouderspoortbeschrijvingENG.txt",
                imageSource = "stadhouderspoort.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.590333, latitude = 4.776000, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.590388, latitude = 4.775000, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.590028,
                latitude = 4.774362,
                name = "Huis van Brecht",
                descriptionNL = "huisvanbrechtbeschrijving.txt",
                descriptionENG = "huisvanbrechtbeschrijvingENG.txt",
                imageSource = "huisvanbrecht7.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.590195,
                latitude = 4.773445,
                name = "Spanjaardsgat",
                descriptionNL = "spanjaardsgatbeschrijving.txt",
                descriptionENG = "spanjaardsgatbeschrijvingENG.txt",
                imageSource = "spanjaardsgat.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.589833,
                latitude = 4.773333,
                name = "Vismarkt",
                descriptionNL = "vismarktbeschrijving.txt",
                descriptionENG = "vismarktbeschrijvingENG.txt",
                imageSource = "vismarktbreda.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.589362,
                latitude = 4.774445,
                name = "Havermarkt",
                descriptionNL = "havermarktbeschrijving.txt",
                descriptionENG = "havermarktbeschrijvingENG.txt",
                imageSource = "havermarkt.webp",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.588778, latitude = 4.774888, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.588833,
                latitude = 4.775278,
                name = "Grote Kerk Breda",
                descriptionNL = "grotekerkbeschrijving.txt",
                descriptionENG = "grotekerkbeschrijvingENG.txt",
                imageSource = "grotekerk.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.588778, latitude = 4.774888, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.588195,
                latitude = 4.775138,
                name = "Het Poortje",
                descriptionNL = "poortjebeschrijving.txt",
                descriptionENG = "poortjebeschrijvingENG.txt",
                imageSource = "hetpoortje.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.587083,
                latitude = 4.775750,
                name = "Ridderstraat",
                descriptionNL = "ridderstraatbeschrijving.txt",
                descriptionENG = "ridderstraatbeschrijvingENG.txt",
                imageSource = "ridderstraat.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.587417,
                latitude = 4.776555,
                name = "De Grote Markt",
                descriptionNL = "grotemarktbeschrijving.txt",
                descriptionENG = "grotemarktbeschrijvingENG.txt",
                imageSource = "grotemarktbreda.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.588028,
                latitude = 4.776333,
                name = "Bevrijdingsmonument",
                descriptionNL = "bevrijdingsmonumentbeschrijving.txt",
                descriptionENG = "bevrijdingsmonumentbeschrijvingENG.txt",
                imageSource = "bevrijdingsmonument.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.588750,
                latitude = 4.776112,
                name = "Stadhuis",
                descriptionNL = "stadhuisbeschrijving.txt",
                descriptionENG = "stadhuisbeschrijvingENG.txt",
                imageSource = "stadhuis.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.587972, latitude = 4.776362, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.587500, latitude = 4.776555, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.587638,
                latitude = 4.777250,
                name = "Antonius van Padua Kerk",
                descriptionNL = "antoniusvanpadaukerkbeschrijving.txt",
                descriptionENG = "antoniusvanpadaukerkbeschrijvingENG.txt",
                imageSource = "paduakerk.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.588278, latitude = 4.778500, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.588000,
                latitude = 4.778945,
                name = "Bibliotheek",
                descriptionNL = "bibliotheekbeschrijving.txt",
                descriptionENG = "bibliotheekbeschrijvingENG.txt",
                imageSource = "bibliotheekbreda.webp",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.587362, latitude = 4.780222, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.587722,
                latitude = 4.781028,
                name = "Kloosterkazerne",
                descriptionNL = "kloosterkazernebeschrijving.txt",
                descriptionENG = "kloosterkazernebeschrijvingENG.txt",
                imageSource = "kloosterkazerne.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.587750,
                latitude = 4.782000,
                name = "Chassé theater",
                descriptionNL = "chassetheaterbeschrijving.txt",
                descriptionENG = "chassetheaterbeschrijvingENG.txt",
                imageSource = "chassetheater.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.587750, latitude = 4.781250, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.588612, latitude = 4.780888, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.589500, latitude = 4.780445, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.589667,
                latitude = 4.781000,
                name = "Beyerd",
                descriptionNL = "beyerdbeschrijving.txt",
                descriptionENG = "beyerdbeschrijvingENG.txt",
                imageSource = "beyerd.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.589500, latitude = 4.780445, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.589555,
                latitude = 4.780000,
                name = "Gasthuispoort",
                descriptionNL = "gasthuispoortbeschrijving.txt",
                descriptionENG = "gasthuispoortbeschrijvingENG.txt",
                imageSource = "gasthuispoort.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.589417, latitude = 4.779862, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.589028, latitude = 4.779695, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.588555, latitude = 4.778333, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.589112,
                latitude = 4.777945,
                name = "Willem Merkxtuin",
                descriptionNL = "willemmerkxtuinbeschrijving.txt",
                descriptionENG = "willemmerkxtuinbeschrijvingENG.txt",
                imageSource = "willemmerkxtuin.webp",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.589667, latitude = 4.777805, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.589695,
                latitude = 4.778362,
                name = "Begijnhof",
                descriptionNL = "begijnhofbeschrijving.txt",
                descriptionENG = "begijnhofbeschrijvingENG.txt",
                imageSource = "begijnhof.jpg",
                routeTag = "Historische Kilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.589667, latitude = 4.777805, routeTag = "Historische Kilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.589500, latitude = 4.776250, routeTag = "Historische Kilometer" });
        }
        Debug.WriteLine("De database heef zoveel punten: " + db.Count);

    }

    public async Task Drop()
    {
        await _sQLiteConnection.DropTableAsync<RouteLocation>();
        
    }

    public async Task<List<RouteLocation>> GetPointsSpecificRoute(string tagRoute)
    {
        return await _sQLiteConnection.Table<RouteLocation>().Where(x => x.routeTag == tagRoute).ToListAsync();
    }

    public async Task<List<string>> GetAllNamesRoutes()
    {
        var allData = await _sQLiteConnection.Table<RouteLocation>().ToListAsync();
        var uniqueRouteTags = allData
            .Select(p => p.routeTag) // Selecteer de gewenste kolom
            .Distinct() // Filter de unieke waarden
            .ToList();

        return uniqueRouteTags;
    }
}