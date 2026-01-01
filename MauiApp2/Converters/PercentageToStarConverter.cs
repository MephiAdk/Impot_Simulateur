using System.Globalization;

namespace MauiApp2.Converters
{
    public class PercentageToStarConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is decimal percentage)
            {
                // Convertit un pourcentage (0.0 à 1.0) en valeur pour GridLength
                // On multiplie par 100 pour avoir une valeur utilisable
                return new GridLength(Math.Max(0.01, (double)percentage * 100), GridUnitType.Star);
            }
            return new GridLength(1, GridUnitType.Star);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

