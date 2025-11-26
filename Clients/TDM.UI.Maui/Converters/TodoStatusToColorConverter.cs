using System.Globalization;
using TDM.Api.Enum;
namespace TDM.UI.Maui.Converters;
public class TodoStatusToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int statusValue)
            return Colors.Gray;
        return (TodoStatus)statusValue switch
        {
            TodoStatus.NotStarted => Color.FromArgb("#9E9E9E"),
            TodoStatus.InProgress => Color.FromArgb("#2196F3"),
            TodoStatus.Completed => Color.FromArgb("#4CAF50"),
            TodoStatus.Cancelled => Color.FromArgb("#F44336"),
            _ => Colors.Gray
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
