using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRTS_Telemetry.Utils
{
    public class BooleanInverterConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return !booleanValue; // Inverte o valor booleano
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return !booleanValue; // Inverte de volta
            }
            return value;
        }
    }
}

