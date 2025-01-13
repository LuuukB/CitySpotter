using CitySpotter.Locations.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitySpotter.Domain.Model
{
    public interface IDatabaseRepo
    {
        Task Init();
        Task<List<RouteLocation>> GetAllRoutes();
        Task AddRoute(RouteLocation location);
        Task Delete(int id);
        Task Drop();
        Task<List<RouteLocation>> GetPointsSpecificRoute(string routeTag);
        Task<List<string>> GetAllNamesRoutes();
    }
}
