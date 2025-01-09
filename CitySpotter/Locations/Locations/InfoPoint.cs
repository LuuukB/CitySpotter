using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CitySpotter.Locations.Locations
{
    class InfoPoint : ILocation
    {
        private string name { get; set; }
        private string description { get; set; }
        private double latitude { get; set; }
        private double longitude { get; set; }
        private string imageSource { get; set; }
        private bool isVisited { get; set; }

        public InfoPoint(double lon, double lat, string name, string info, string imageSource, bool isVisited = false) {
            this.latitude = lon;
            this.longitude = lat;
            this.name = name;
            this.description = info;
            this.imageSource = imageSource;
            this.isVisited = isVisited;
        }

        public void setVisited()
        {
            this.isVisited = true;
        }
    }

    
}
