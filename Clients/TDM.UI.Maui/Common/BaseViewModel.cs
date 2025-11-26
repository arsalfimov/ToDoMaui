using CommunityToolkit.Mvvm.ComponentModel;

namespace TDM.UI.Maui.Common;

/// <summary>
/// Base ViewModel for all ViewModels.
/// Provides common properties for loading states and busy indicators.
/// </summary>
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _loadingText = "Загрузка данных...";

    [ObservableProperty]
    private bool _hasError;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    /// <summary>
    /// Sets the loading state and optional loading text.
    /// </summary>
    protected void SetLoading(bool isLoading, string loadingText = "Загрузка данных...")
    {
        IsLoading = isLoading;
        LoadingText = loadingText;
    }

    /// <summary>
    /// Sets the error state with a message.
    /// </summary>
    protected void SetError(string errorMessage)
    {
        HasError = true;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Clears the error state.
    /// </summary>
    protected void ClearError()
    {
        HasError = false;
        ErrorMessage = string.Empty;
    }
}
