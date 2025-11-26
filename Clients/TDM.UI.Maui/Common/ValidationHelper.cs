using System.Text.RegularExpressions;

namespace TDM.UI.Maui.Common;

/// <summary>
/// Helper class for client-side validation.
/// </summary>
public static partial class ValidationHelper
{
    /// <summary>
    /// Validates phone number format (Belarus and Russia).
    /// Supports formats: +375 (29) 123-45-67, +7 (999) 999-99-99, etc.
    /// </summary>
    public static bool IsValidPhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return true; // Optional field

        return PhoneRegex().IsMatch(phone);
    }

    /// <summary>
    /// Validates email address format.
    /// </summary>
    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return true; // Optional field

        return EmailRegex().IsMatch(email);
    }

    /// <summary>
    /// Validates string length.
    /// </summary>
    public static bool IsValidLength(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            return true;

        return value.Length <= maxLength;
    }

    /// <summary>
    /// Validates required field.
    /// </summary>
    public static bool IsRequired(string? value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    [GeneratedRegex(@"^(\+?(375|7)|8)?[\s-]?\(?\d{2,3}\)?[\s-]?\d{3}[\s-]?\d{2}[\s-]?\d{2}$")]
    private static partial Regex PhoneRegex();

    [GeneratedRegex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    private static partial Regex EmailRegex();
}

