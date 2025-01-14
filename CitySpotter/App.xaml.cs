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

            var theme = Preferences.Get("theme", "System");

            setTheme("NormalMode");
        }


        public void setTheme(string theme)
        {
            if (theme == "System")
            {
                //theme = Current.PlatformAppTheme.ToString();
            }

            ResourceDictionary dictionary = theme switch
            {
                "ColorBlindMode" => new Resources.Styles.ColorBlindMode(),
                "NormalMode" => new Resources.Styles.NormalMode()
            };

            if (dictionary != null)
            {
                Resources.MergedDictionaries.Clear();

                Resources.MergedDictionaries.Add(dictionary);
            }
        }
    }
}
