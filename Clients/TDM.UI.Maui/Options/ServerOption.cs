using System.ComponentModel.DataAnnotations;

namespace TDM.UI.Maui.Options;

/// <summary>
/// Configuration options for the API server connection.
/// </summary>
public class ServerOption
{
    /// <summary>
    /// Base URL of the API server.
    /// </summary>
    [Required(ErrorMessage = "API URL is required")]
    [Url(ErrorMessage = "API URL must be a valid URL")]
    public string ApiUrl { get; set; } = string.Empty;

    /// <summary>
    /// Base URL of the API server for mobile devices (when localhost is not accessible).
    /// Falls back to ApiUrl if not specified.
    /// </summary>
    [Url(ErrorMessage = "Mobile API URL must be a valid URL")]
    public string? ApiUrlMobile { get; set; }

    /// <summary>
    /// Gets the appropriate API URL based on the platform.
    /// Returns ApiUrlMobile for mobile platforms, otherwise ApiUrl.
    /// </summary>
    public string GetApiUrl()
    {
#if ANDROID || IOS
        return !string.IsNullOrEmpty(ApiUrlMobile) ? ApiUrlMobile : ApiUrl;
#else
        return ApiUrl;
#endif
    }
}

