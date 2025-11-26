using TDM.UI.Maui.ViewModels;

namespace TDM.UI.Maui.Views;

public partial class TodoItemDetailsPage : ContentPage
{
	public TodoItemDetailsPage(TodoItemDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
	}
}
