using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitySpotter.Domain.Model
{
    public interface ILocationPermissionsService
    {
        Task<PermissionStatus> CheckAndRequestLocationPermissionAsync();

        Task<bool> ShowSettingsIfPermissionDeniedAsync();
    }
}
