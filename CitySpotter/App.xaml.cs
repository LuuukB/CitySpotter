using CitySpotter.Domain.Model;
using CommunityToolkit.Mvvm.Messaging;

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

            WeakReferenceMessenger.Default.Register<ThemeChangedMessage>(this, (r, m) =>
            {
                setTheme(m.Value);
            });

            var theme = Preferences.Get("theme", "NormalMode");

            setTheme(theme);
        }


        public void setTheme(string theme)
        {
            if (theme == "System")
            {
                //theme = Current.PlatformAppTheme.ToString();
            }

            ResourceDictionary dictionary = theme switch
            {
                "DeuteranopieMode" => new Resources.Styles.DeuteranopieMode(),
                "NormalMode" => new Resources.Styles.NormalMode(),
                "AnatropieMode" => new Resources.Styles.AnatropieMode()
            };

            if (dictionary != null)
            {
                Resources.MergedDictionaries.Clear();

                Resources.MergedDictionaries.Add(dictionary);
            }
        }
    }
}
