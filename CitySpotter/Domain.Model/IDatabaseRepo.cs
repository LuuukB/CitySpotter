using CitySpotter.Domain.Model;

namespace CitySpotter.Domain.Model;

public interface IDatabaseRepo
{
    Task Init();
    Task<List<RouteLocation>> GetAllRoutes();
    Task AddRoute(RouteLocation location);
    Task Delete(int id);
    Task Drop();
    Task<List<RouteLocation>> GetPointsSpecificRoute(string routeTag);
    Task<List<string>> GetAllNamesRoutes();
    Task updateDatabase(int locationInRoute);
}
