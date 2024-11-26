using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GenealogicalTreeCource.ViewModel
{
    public class DateOnlyToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly dateOnly)
            {
                return dateOnly.ToDateTime(TimeOnly.MinValue);
            }
            return DateTime.MinValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return DateOnly.FromDateTime(dateTime);
            }
            return null;
        }
    }
}
