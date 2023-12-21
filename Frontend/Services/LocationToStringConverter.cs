using System.Globalization;
using Location = Frontend.Models.Location;

namespace Frontend.Services;

public class LocationToStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is Location location) {
            return $"{location.Street} {location.StreetNumber}, {location.ZipCode} {location.City}";
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}