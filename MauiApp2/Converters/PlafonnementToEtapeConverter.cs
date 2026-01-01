using System.Globalization;

namespace MauiApp2.Converters
{
    public class PlafonnementToEtapeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isPlafonne)
            {
                return isPlafonne ? "Étape 5 : Application de la décote" : "Étape 4 : Application de la décote";
            }
            return "Étape 4 : Application de la décote";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

