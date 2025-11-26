using TDM.UI.Maui.Views;

namespace TDM.UI.Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		// Register routes for navigation
		Routing.RegisterRoute(nameof(ContactDetailsPage), typeof(ContactDetailsPage));
		Routing.RegisterRoute(nameof(CreateContactPage), typeof(CreateContactPage));
		Routing.RegisterRoute("create-todo-item", typeof(CreateTodoItemPage));
		Routing.RegisterRoute("todo-item-details", typeof(TodoItemDetailsPage));
	}
}
