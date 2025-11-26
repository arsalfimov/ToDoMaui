using TDM.UI.Maui.ViewModels;

namespace TDM.UI.Maui.Views;

public partial class TodoListPage : ContentPage
{
	private readonly TodoListViewModel _viewModel;

	public TodoListPage(TodoListViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
		BindingContext = _viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.LoadTodoItemsCommand.ExecuteAsync(null);
	}
}
