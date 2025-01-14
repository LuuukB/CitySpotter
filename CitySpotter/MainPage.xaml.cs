using CitySpotter.Domain.Services;
using System.Diagnostics;

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
