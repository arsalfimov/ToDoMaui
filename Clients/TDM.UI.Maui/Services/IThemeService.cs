namespace TDM.UI.Maui.Services;

/// <summary>
/// Service for managing application theme.
/// </summary>
public interface IThemeService
{
    /// <summary>
    /// Gets the current theme.
    /// </summary>
    AppTheme CurrentTheme { get; }

    /// <summary>
    /// Sets the application theme.
    /// </summary>
    void SetTheme(AppTheme theme);

    /// <summary>
    /// Toggles between light and dark theme.
    /// </summary>
    void ToggleTheme();

    /// <summary>
    /// Loads the saved theme from preferences.
    /// </summary>
    void LoadSavedTheme();
}
