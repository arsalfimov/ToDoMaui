namespace TDM.UI.Maui.Services;

/// <summary>
/// Implementation of theme management service.
/// </summary>
public class ThemeService : IThemeService
{
    private const string THEME_KEY = "app_theme";

    public AppTheme CurrentTheme { get; private set; }

    public ThemeService()
    {
        LoadSavedTheme();
    }

    public void SetTheme(AppTheme theme)
    {
        CurrentTheme = theme;
        if (Application.Current is not null)
        {
            Application.Current.UserAppTheme = theme;
        }
        Preferences.Set(THEME_KEY, (int)theme);
    }

    public void ToggleTheme()
    {
        var newTheme = CurrentTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;
        SetTheme(newTheme);
    }

    public void LoadSavedTheme()
    {
        var savedTheme = Preferences.Get(THEME_KEY, (int)AppTheme.Dark);
        CurrentTheme = (AppTheme)savedTheme;
        if (Application.Current is not null)
        {
            Application.Current.UserAppTheme = CurrentTheme;
        }
    }
}

