using TDM.UI.Maui.ViewModels;
using TDM.UI.Maui.Services;

namespace TDM.UI.Maui.Views;

public partial class SettingsPage : ContentPage
{
	private readonly IThemeService _themeService;

	public SettingsPage(SettingsViewModel viewModel, IThemeService themeService)
	{
		InitializeComponent();
		_themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
		BindingContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
	}
	
	private void OnDarkThemeChecked(object sender, CheckedChangedEventArgs e)
	{
		if (e.Value)
			_themeService.SetTheme(AppTheme.Dark);
	}

	private void OnLightThemeChecked(object sender, CheckedChangedEventArgs e)
	{
		if (e.Value)
			_themeService.SetTheme(AppTheme.Light);
	}
}
