using System.Globalization;
using TDM.Api.Enum;
namespace TDM.UI.Maui.Converters;
public class PriorityToTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int priorityValue)
            return "Не указан";
        return (Priority)priorityValue switch
        {
            Priority.NotSpecified => "Не указан",
            Priority.Low => "Низкий",
            Priority.Medium => "Средний",
            Priority.High => "Высокий",
            Priority.Urgent => "Срочный",
            _ => "Не указан"
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
