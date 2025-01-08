using CitySpotter.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CitySpotter.Infrastructure
{
    public class DatabaseRepo : IDatabaseRepo
    {
        private string _dpPath;
        public DatabaseRepo(string dbPath) { _dpPath = dbPath; }
        public void AddRoute(Route route)
        {
            using SQLiteConnection sQLiteConnection = new(_dpPath);
            sQLiteConnection.Insert(route);
        }

        public void Delete(int id)
        {
            using SQLiteConnection sQLiteConnection = new(_dpPath);
            sQLiteConnection.Delete<Route>(id);
        }

        public List<Route> GetAllRoutes()
        {
            using SQLiteConnection sQLiteConnection = new(_dpPath);
            return sQLiteConnection.Table<Route>().ToList();
        }

        public void Init()
        {
            using SQLiteConnection sQLiteConnection = new(_dpPath);
            sQLiteConnection.CreateTable<Route>();
        }
    }
}
