using System.ComponentModel.DataAnnotations;

namespace TDM.UI.Maui.Options;

/// <summary>
/// Environment types for API connection.
/// </summary>
public enum ApiEnvironment
{
    /// <summary>
    /// Local development on same machine (localhost).
    /// </summary>
    LocalDesktop,

    /// <summary>
    /// Local development from mobile device (computer IP in local network).
    /// </summary>
    LocalMobile,

    /// <summary>
    /// Production server.
    /// </summary>
    Production,

    /// <summary>
    /// Custom URL (user-defined).
    /// </summary>
    Custom
}

