using CitySpotter.Domain.Services;

namespace CitySpotter;

public partial class InfoPointPopup
{
	public InfoPointPopup(InfoPopupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}