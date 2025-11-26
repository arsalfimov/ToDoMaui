namespace TDM.Server.Application.Common;

/// <summary>
/// Contains regex patterns for validation across the application.
/// </summary>
public static class RegexPatterns
{
    /// <summary>
    /// Regex pattern for validating phone numbers (Belarus and Russia).
    /// Supports formats: 
    /// - Belarus: +375 (29) 123-45-67, +375291234567, 80291234567
    /// - Russia: +7 (999) 999-99-99, 8 (999) 999-99-99, +79999999999
    /// </summary>
    public const string Phone = @"^(\+?(375|7)|8)?[\s-]?\(?\d{2,3}\)?[\s-]?\d{3}[\s-]?\d{2}[\s-]?\d{2}$";

    /// <summary>
    /// Regex pattern for validating email addresses (RFC 5322 simplified).
    /// </summary>
    public const string Email = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
}
