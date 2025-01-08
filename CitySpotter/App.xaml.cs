using CitySpotter.Domain.Model;

namespace CitySpotter
{
    public partial class App : Application
    {
        public static IDatabaseRepo RouteDatabase { get; set; }
        public App(IDatabaseRepo databaseRepo)
        {
            InitializeComponent();

            MainPage = new AppShell();

            RouteDatabase = databaseRepo;
        }
    }
}
