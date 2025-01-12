using CitySpotter.Domain.Services;

namespace CitySpotter
{
    public partial class MainPage : ContentPage
    {
     

        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
           BindingContext = viewModel;

        }

    }

}
