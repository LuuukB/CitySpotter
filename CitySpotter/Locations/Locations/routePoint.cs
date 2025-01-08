using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitySpotter.Locations.Locations
{
    class routePoint : ILocation
    {
        private double lon { get; set; }
        private double lat { get; set; }

        public routePoint(double lon, double lat)
        {
            this.lon = lon;
            this.lat = lat;
        }

    }
}
