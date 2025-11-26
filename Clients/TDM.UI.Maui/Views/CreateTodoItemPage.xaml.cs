using TDM.UI.Maui.ViewModels;

namespace TDM.UI.Maui.Views;

public partial class CreateTodoItemPage : ContentPage
{
	private readonly CreateTodoItemViewModel _viewModel;

	public CreateTodoItemPage(CreateTodoItemViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
		BindingContext = _viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.InitializeAsync();
	}
}
