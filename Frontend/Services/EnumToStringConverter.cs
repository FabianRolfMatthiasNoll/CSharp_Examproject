using System.Globalization;
using Frontend.Models;

namespace Frontend.Services;

public class EnumToStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is Enum enumValue) {
            switch (value)
            {
                case DamageType._0:
                    return "Street Lamp";
                case DamageType._1:
                    return "Pothole";
                case DamageType._2:
                    return "Other";
            }
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}
