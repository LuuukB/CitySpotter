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
        public void AddRoute(RouteLocation location)
        {
            using SQLiteConnection sQLiteConnection = new(_dpPath);
            sQLiteConnection.Insert(location);
        }
        public void Delete(int id)
        {
            using SQLiteConnection sQLiteConnection = new(_dpPath);
            sQLiteConnection.Delete<RouteLocation>(id);
        }
        public List<RouteLocation> GetAllRoutes()
        {
            using SQLiteConnection sQLiteConnection = new(_dpPath);
            return sQLiteConnection.Table<RouteLocation>().ToList();
        }
        public void Init()
        {
            using SQLiteConnection sQLiteConnection = new(_dpPath);
            //   sQLiteConnection.CreateTable<Route>();
            sQLiteConnection.CreateTable<RouteLocation>();
        }
        public void Drop()
        {
            using SQLiteConnection sQLiteConnection = new SQLiteConnection(_dpPath);
            sQLiteConnection.DropTable<RouteLocation>();
        }
        public List<RouteLocation> GetPointsSpecificRoute(string tagRoute) 
        {
            using (var db = new SQLiteConnection(_dpPath)) 
            {
                return db.Table<RouteLocation>().Where(x => x.routeTag == tagRoute).ToList();
            }
        }
        public List<string> GetAllNamesRoutes() 
        {
            using (var db = new SQLiteConnection(_dpPath))
            {
                return db.Table<RouteLocation>()
                         .Select(p => p.routeTag)
                         .Distinct()
                         .ToList();
            }
        }
    }
}
