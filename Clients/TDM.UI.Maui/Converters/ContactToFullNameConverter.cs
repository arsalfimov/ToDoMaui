using System.Globalization;
using TDM.Api.Contracts.Contacts;

namespace TDM.UI.Maui.Converters;

/// <summary>
/// Converts ContactResponse to full name for display in picker
/// </summary>
public class ContactToFullNameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ContactResponse contact)
        {
            return $"{contact.FirstName} {contact.LastName}";
        }

        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}