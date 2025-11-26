using TDM.UI.Maui.ViewModels;

namespace TDM.UI.Maui.Views;

public partial class ContactsListPage : ContentPage
{
	public ContactsListPage(ContactsListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		
		if (BindingContext is ContactsListViewModel viewModel)
		{
			await viewModel.LoadContactsCommand.ExecuteAsync(null);
		}
	}
}
