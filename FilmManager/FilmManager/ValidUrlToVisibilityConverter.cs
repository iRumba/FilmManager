using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FilmManager
{
    public class ValidUrlToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = parameter as string;
            var nonVisible = param == "H" || param == "h" ? Visibility.Hidden : Visibility.Collapsed;
            if (value is string)
            {
                Uri url;
                if (Uri.TryCreate((string)value, UriKind.RelativeOrAbsolute, out url))
                    return Visibility.Visible;
            }
            return nonVisible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
