using System.Globalization;
using TDM.Api.Contracts.Contacts;

namespace TDM.UI.Maui.Converters;

/// <summary>
/// Converts a ContactResponse to initials (e.g., FirstName="Иван", LastName="Иванов" -> "ИИ")
/// </summary>
public class NameToInitialsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ContactResponse contact)
            return "?";

        if (string.IsNullOrWhiteSpace(contact.FirstName) || string.IsNullOrWhiteSpace(contact.LastName))
            return "?";
        
        // Берем первые буквы имени и фамилии
        return $"{contact.FirstName[0]}{contact.LastName[0]}".ToUpper();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
