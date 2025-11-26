using System.Reflection;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TDM.UI.Maui.Extensions;

namespace TDM.UI.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		// Load configuration from embedded appsettings.json
		var assembly = Assembly.GetExecutingAssembly();
		using var stream = assembly.GetManifestResourceStream("TDM.UI.Maui.appsettings.json");
		
		if (stream != null)
		{
			builder.Configuration.AddJsonStream(stream);

#if DEBUG
			// Reset stream position for reuse or use a new stream
			using var devStream = assembly.GetManifestResourceStream("TDM.UI.Maui.appsettings.Development.json");
			if (devStream != null)
			{
				builder.Configuration.AddJsonStream(devStream);
			}
#endif
		}

		// Register core services (includes Refit clients)
		builder.Services.AddCoreServices(builder.Configuration);

		// Register ViewModels
		builder.Services.AddTransient<ViewModels.ContactsListViewModel>();
		builder.Services.AddTransient<ViewModels.ContactDetailsViewModel>();
		builder.Services.AddTransient<ViewModels.CreateContactViewModel>();
		builder.Services.AddTransient<ViewModels.TodoListViewModel>();
		builder.Services.AddTransient<ViewModels.CreateTodoItemViewModel>();
		builder.Services.AddTransient<ViewModels.TodoItemDetailsViewModel>();
		builder.Services.AddTransient<ViewModels.SettingsViewModel>();

		// Register Pages
		builder.Services.AddTransient<Views.ContactsListPage>();
		builder.Services.AddTransient<Views.ContactDetailsPage>();
		builder.Services.AddTransient<Views.CreateContactPage>();
		builder.Services.AddTransient<Views.TodoListPage>();
		builder.Services.AddTransient<Views.CreateTodoItemPage>();
		builder.Services.AddTransient<Views.TodoItemDetailsPage>();
		builder.Services.AddTransient<Views.SettingsPage>();

		return builder.Build();
	}
}
