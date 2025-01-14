using SQLite;

namespace CitySpotter.Domain.Model
{
    public class RouteLocation
    {
        public string? name { get; set; }
        [PrimaryKey, AutoIncrement, Column("Index")]
        public int locationInRoute { get; set; }
        public string? descriptionNL { get; set; }
        public string? descriptionENG { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string? imageSource { get; set; }
        public bool isVisited { get; set; }
        public string routeTag { get; set; }
        public void setVisited()
        {
            this.isVisited = true;
        }
    }
}
