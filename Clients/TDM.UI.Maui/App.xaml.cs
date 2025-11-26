using TDM.UI.Maui.Services;

namespace TDM.UI.Maui;

public partial class App
{
	public App(IThemeService themeService)
	{
		ArgumentNullException.ThrowIfNull(themeService);
		
		// Load saved theme before initialization
		themeService.LoadSavedTheme();
		
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var window = new Window(new AppShell());
		
#if WINDOWS
		window.Width = 600;
		window.Height = 800;
#endif

		return window;
	}
}
