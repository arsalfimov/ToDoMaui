using System.Globalization;
using TDM.Api.Enum;
namespace TDM.UI.Maui.Converters;
public class PriorityToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int priorityValue)
            return Colors.Gray;
        return (Priority)priorityValue switch
        {
            Priority.NotSpecified => Colors.Gray,
            Priority.Low => Color.FromArgb("#4CAF50"),
            Priority.Medium => Color.FromArgb("#2196F3"),
            Priority.High => Color.FromArgb("#FF9800"),
            Priority.Urgent => Color.FromArgb("#F44336"),
            _ => Colors.Gray
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
