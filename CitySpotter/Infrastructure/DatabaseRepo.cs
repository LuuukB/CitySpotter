using CitySpotter.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using CitySpotter.Locations.Locations;


namespace CitySpotter.Infrastructure
{
    public class DatabaseRepo : IDatabaseRepo
    {
        private string _dpPath;
        public DatabaseRepo(string dbPath) { _dpPath = dbPath; }
        public async Task AddRoute(RouteLocation location)
        {
            SQLiteAsyncConnection sQLiteConnection = new(_dpPath);
            await sQLiteConnection.InsertAsync(location);
        }
        public async Task Delete(int id)
        {
            SQLiteAsyncConnection sQLiteConnection = new(_dpPath);
            await sQLiteConnection.DeleteAsync<RouteLocation>(id);
        }
        public async Task<List<RouteLocation>> GetAllRoutes()
        {
            SQLiteAsyncConnection sQLiteConnection = new(_dpPath);
            return await sQLiteConnection.Table<RouteLocation>().ToListAsync();
        }
        public async Task Init()
        {
            SQLiteAsyncConnection sQLiteConnection = new(_dpPath);
            //   sQLiteConnection.CreateTable<Route>();
           await sQLiteConnection.CreateTableAsync<RouteLocation>();
        }
        public async Task Drop()
        {
            SQLiteAsyncConnection sQLiteConnection = new (_dpPath);
            await sQLiteConnection.CreateTableAsync<RouteLocation>();
        }
        public async Task<List<RouteLocation>> GetPointsSpecificRoute(string tagRoute) 
        {
            SQLiteAsyncConnection sQLiteConnection = new SQLiteAsyncConnection(_dpPath);
            return await sQLiteConnection.Table<RouteLocation>().Where(x => x.routeTag == tagRoute).ToListAsync();
            
        }
        public async Task<List<string>> GetAllNamesRoutes() 
        {
            SQLiteAsyncConnection sQLiteConnection = new(_dpPath);

            var allData = await sQLiteConnection.Table<RouteLocation>().ToListAsync();
            var uniqueRouteTags = allData
                .Select(p => p.routeTag) // Selecteer de gewenste kolom
                .Distinct() // Filter de unieke waarden
                .ToList();

            return uniqueRouteTags;

        }
    }
}
