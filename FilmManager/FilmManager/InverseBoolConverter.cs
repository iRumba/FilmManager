using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FilmManager
{
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return !(bool)value;
            if (value is bool?)
            {
                var val = (bool?)value;
                return val.HasValue ? !val.Value : true;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return !(bool)value;
            if (value is bool?)
            {
                var val = (bool?)value;
                return val.HasValue ? !val.Value : true;
            }
            return value;
        }
    }
}
