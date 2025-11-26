using System.Globalization;
using TDM.Api.Contracts.Contacts;

namespace TDM.UI.Maui.Converters;

/// <summary>
/// Converts a ContactResponse to a consistent color for avatar background
/// </summary>
public class NameToColorConverter : IValueConverter
{
    private static readonly Color[] AvatarColors = new[]
    {
        Color.FromRgb(156, 39, 176),  // Purple
        Color.FromRgb(63, 81, 181),   // Indigo
        Color.FromRgb(33, 150, 243),  // Blue
        Color.FromRgb(0, 150, 136),   // Teal
        Color.FromRgb(76, 175, 80),   // Green
        Color.FromRgb(255, 152, 0),   // Orange
        Color.FromRgb(233, 30, 99),   // Pink
        Color.FromRgb(121, 85, 72),   // Brown
    };

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ContactResponse contact)
            return AvatarColors[0];

        // Генерируем индекс на основе хэш-кода полного имени
        var fullName = $"{contact.FirstName} {contact.LastName}";
        var hash = Math.Abs(fullName.GetHashCode());
        var index = hash % AvatarColors.Length;
        
        return AvatarColors[index];
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
