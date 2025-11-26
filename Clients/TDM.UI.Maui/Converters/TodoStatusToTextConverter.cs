using System.Globalization;
using TDM.Api.Enum;
namespace TDM.UI.Maui.Converters;
public class TodoStatusToTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int statusValue)
            return "Неизвестно";
        return (TodoStatus)statusValue switch
        {
            TodoStatus.NotStarted => "Не начата",
            TodoStatus.InProgress => "В процессе",
            TodoStatus.Completed => "Завершена",
            TodoStatus.Cancelled => "Отменена",
            _ => "Неизвестно"
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
