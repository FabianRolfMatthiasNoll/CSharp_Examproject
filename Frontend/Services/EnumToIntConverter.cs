using System.Globalization;

namespace Frontend.Services;

public class EnumToIntConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return (int)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return (DamageType)value;
    }
}
