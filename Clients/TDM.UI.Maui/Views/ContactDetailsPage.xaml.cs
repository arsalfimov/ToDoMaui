using TDM.UI.Maui.ViewModels;

namespace TDM.UI.Maui.Views;

public partial class ContactDetailsPage : ContentPage
{
	public ContactDetailsPage(ContactDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}

