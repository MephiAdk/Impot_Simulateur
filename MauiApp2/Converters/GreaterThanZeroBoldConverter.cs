using System.Globalization;

namespace MauiApp2.Converters
{
    public class GreaterThanZeroBoldConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue && decimalValue > 0)
            {
                return FontAttributes.Bold;
            }
            return FontAttributes.None;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

