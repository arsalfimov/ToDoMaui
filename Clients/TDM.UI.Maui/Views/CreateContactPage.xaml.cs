using TDM.UI.Maui.ViewModels;

namespace TDM.UI.Maui.Views;

public partial class CreateContactPage : ContentPage
{
	public CreateContactPage(CreateContactViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
