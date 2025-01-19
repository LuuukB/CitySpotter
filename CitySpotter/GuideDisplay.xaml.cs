using CitySpotter.Domain.Services;

namespace CitySpotter;

public partial class GuideDisplay
{
	public GuideDisplay(GuideDisplayViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}