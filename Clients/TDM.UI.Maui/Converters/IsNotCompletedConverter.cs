using System.Globalization;
using TDM.Api.Enum;

namespace TDM.UI.Maui.Converters;

/// <summary>
/// Converter that returns true if TodoStatus is not Completed or Cancelled.
/// Used to show action buttons only for active tasks.
/// </summary>
public class IsNotCompletedConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int statusValue)
            return true;

        var status = (TodoStatus)statusValue;
        return status != TodoStatus.Completed && status != TodoStatus.Cancelled;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
