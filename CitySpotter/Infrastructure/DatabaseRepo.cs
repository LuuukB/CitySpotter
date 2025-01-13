using CitySpotter.Domain.Model;
using SQLite;
using CitySpotter.Locations.Locations;
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
                description = "vvvkantoorbeschrijving.txt",
                imageSource = "vvvkantoor.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.593278,
                latitude = 4.779388,
                name = "Liefdeszuster",
                description = "liefdeszusterbeschrijving.txt",
                imageSource = "liefdeszuster.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.592500,
                latitude = 4.779695,
                name = "Nassau-Baroniemonument",
                description = "nassaubaroniemonumentbeschrijving.txt",
                imageSource = "nassaubaroniemonument.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.592500, latitude = 4.779388, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.592833,
                latitude = 4.778472,
                name = "Vuurtoren",
                description = "vuurtorenbeschrijving.txt",
                imageSource = "vuurtoren.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.592667, latitude = 4.777917, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.590612, latitude = 4.777000, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.590612,
                latitude = 4.776167,
                name = "Kasteel Breda",
                description = "kasteelbeschrijving.txt",
                imageSource = "kasteelbreda.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.589695,
                latitude = 4.776138,
                name = "Stadhouderspoort",
                description = "stadhouderspoortbeschrijving.txt",
                imageSource = "stadhouderspoort.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.590333, latitude = 4.776000, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.590388, latitude = 4.775000, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.590028,
                latitude = 4.774362,
                name = "Huis van Brecht",
                description = "huisvanbrechtbeschrijving.txt",
                imageSource = "huisvanbrecht7.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.590195,
                latitude = 4.773445,
                name = "Spanjaardsgat",
                description = "spanjaardsgatbeschrijving.txt",
                imageSource = "spanjaardsgat.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.589833,
                latitude = 4.773333,
                name = "Vismarkt",
                description = "vismarktbeschrijving.txt",
                imageSource = "vismarktbreda.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.589362,
                latitude = 4.774445,
                name = "Havermarkt",
                description = "havermarktbeschrijving.txt",
                imageSource = "havermarkt.webp",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.588778, latitude = 4.774888, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.588833,
                latitude = 4.775278,
                name = "Grote Kerk Breda",
                description = "grotekerkbeschrijving.txt",
                imageSource = "grotekerk.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.588778, latitude = 4.774888, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.588195,
                latitude = 4.775138,
                name = "Het Poortje",
                description = "poortjebeschrijving.txt",
                imageSource = "hetpoortje.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.587083,
                latitude = 4.775750,
                name = "Ridderstraat",
                description = "ridderstraatbeschrijving.txt",
                imageSource = "ridderstraat.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.587417,
                latitude = 4.776555,
                name = "De Grote Markt",
                description = "grotemarktbeschrijving.txt",
                imageSource = "grotemarktbreda.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.588028,
                latitude = 4.776333,
                name = "Bevrijdingsmonument",
                description = "bevrijdingsmonumentbeschrijving.txt",
                imageSource = "bevrijdingsmonument.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.588750,
                latitude = 4.776112,
                name = "Stadhuis",
                description = "stadhuisbeschrijving.txt",
                imageSource = "stadhuis.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.587972, latitude = 4.776362, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.587500, latitude = 4.776555, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.587638,
                latitude = 4.777250,
                name = "Antonius van Padua Kerk",
                description = "antoniusvanpadaukerkbeschrijving.txt",
                imageSource = "paduakerk.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.588278, latitude = 4.778500, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.588000,
                latitude = 4.778945,
                name = "Bibliotheek",
                description = "bibliotheekbeschrijving.txt",
                imageSource = "bibliotheekbreda.webp",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.587362, latitude = 4.780222, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.587722,
                latitude = 4.781028,
                name = "Kloosterkazerne",
                description = "kloosterkazernebeschrijving.txt",
                imageSource = "kloosterkazerne.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            {
                longitude = 51.587750,
                latitude = 4.782000,
                name = "Chassé theater",
                description = "chassetheaterbeschrijving.txt",
                imageSource = "chassetheater.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.587750, latitude = 4.781250, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.588612, latitude = 4.780888, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.589500, latitude = 4.780445, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.589667,
                latitude = 4.781000,
                name = "Beyerd",
                description = "beyerdbeschrijving.txt",
                imageSource = "beyerd.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.589500, latitude = 4.780445, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.589555,
                latitude = 4.780000,
                name = "Gasthuispoort",
                description = "gasthuispoortbeschrijving.txt",
                imageSource = "gasthuispoort.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.589417, latitude = 4.779862, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.589028, latitude = 4.779695, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.588555, latitude = 4.778333, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.589112,
                latitude = 4.777945,
                name = "Willem Merkxtuin",
                description = "willemmerkxtuinbeschrijving.txt",
                imageSource = "willemmerkxtuin.webp",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.589667, latitude = 4.777805, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            {
                longitude = 51.589695,
                latitude = 4.778362,
                name = "Begijnhof",
                description = "begijnhofbeschrijving.txt",
                imageSource = "begijnhof.jpg",
                routeTag = "historischeKilometer"
            });
            await AddRoute(new RouteLocation
            { longitude = 51.589667, latitude = 4.777805, routeTag = "historischeKilometer" });
            await AddRoute(new RouteLocation
            { longitude = 51.589500, latitude = 4.776250, routeTag = "historischeKilometer" });
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