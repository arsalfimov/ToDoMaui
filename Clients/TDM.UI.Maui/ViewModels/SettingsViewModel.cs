using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TDM.UI.Maui.Common;
using TDM.UI.Maui.Services;

namespace TDM.UI.Maui.ViewModels;

/// <summary>
/// ViewModel for application settings.
/// </summary>
public partial class SettingsViewModel : BaseViewModel
{
    private readonly IThemeService _themeService;

    [ObservableProperty]
    private bool _isDarkTheme;

    public SettingsViewModel(IThemeService themeService)
    {
        _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
        IsDarkTheme = _themeService.CurrentTheme == AppTheme.Dark;
    }

    /// <summary>
    /// Toggles the application theme.
    /// </summary>
    [RelayCommand]
    private void ToggleTheme()
    {
        _themeService.ToggleTheme();
        IsDarkTheme = _themeService.CurrentTheme == AppTheme.Dark;
    }

    /// <summary>
    /// Sets the theme to dark.
    /// </summary>
    [RelayCommand]
    private void SetDarkTheme()
    {
        _themeService.SetTheme(AppTheme.Dark);
        IsDarkTheme = true;
    }

    /// <summary>
    /// Sets the theme to light.
    /// </summary>
    [RelayCommand]
    private void SetLightTheme()
    {
        _themeService.SetTheme(AppTheme.Light);
        IsDarkTheme = false;
    }
}
