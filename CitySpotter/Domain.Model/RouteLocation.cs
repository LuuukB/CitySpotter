using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitySpotter.Locations.Locations
{
    public class RouteLocation
    {
        public string? name { get; set; }
        [PrimaryKey, AutoIncrement, Column("Index")]
        public int locationInRoute { get; set; }
        public string? description { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string? imageSource { get; set; }
        public bool isVisited { get; set; }

        /*public RouteLocation(double lon, double lat, string name, string info, string imageSource, bool isVisited)
        {
            this.latitude = lon;
            this.longitude = lat;
            this.name = name;
            this.description = info;
            this.imageSource = imageSource;
            this.isVisited = isVisited;
        }*/

        public void setVisited()
        {
            this.isVisited = true;
        }
    }
}
