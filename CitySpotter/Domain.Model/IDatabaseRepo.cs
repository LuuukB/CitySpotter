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
        void Init();
        List<RouteLocation> GetAllRoutes();
        void AddRoute(RouteLocation location);
        void Delete(int id);

        List<RouteLocation> GetPointsSpecificRoute(string routeTag);

        List<string> GetAllNamesRoutes();
    }
}
