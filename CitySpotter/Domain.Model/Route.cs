using SQLite;


namespace CitySpotter.Domain.Model
{
    [Table ("route")]
    public class Route
    {
        public string RouteName {  get; set; }
        [PrimaryKey, AutoIncrement, Column("Index")]
        public int RouteId {  get; set; }
        public List<Location> RoutePoints { get; set; }
    }
}
