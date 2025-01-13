namespace CitySpotter.Domain.Model;

public interface ILocationPermissionsService
{
    Task<PermissionStatus> CheckAndRequestLocationPermissionAsync();

    Task<bool> ShowSettingsIfPermissionDeniedAsync();
}
