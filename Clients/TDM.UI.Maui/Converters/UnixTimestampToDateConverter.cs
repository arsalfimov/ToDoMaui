using System.Globalization;

namespace TDM.UI.Maui.Converters;

/// <summary>
/// Converts Unix timestamp (seconds) to DateTime and formats it.
/// </summary>
public class UnixTimestampToDateConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not long timestamp || timestamp <= 0)
            return string.Empty;

        var dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).LocalDateTime;
        return dateTime.ToString("dd.MM.yyyy");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
