namespace CitySpotter.Domain.Services.Internet;

public class InternetHandler(IConnectivity connectivity) : IInternetHandler
{
    public bool HasInternetConnection()
    {
        return connectivity.NetworkAccess == NetworkAccess.Internet;
    }
}