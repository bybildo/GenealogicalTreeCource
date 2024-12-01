using GenealogicalTreeCource.Enum;
using System.Globalization;
using System.Windows.Data;

namespace GenealogicalTreeCource.ViewModel
{
    public class GenderToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var gender = (Gender)value;
            return gender == Gender.male ? "Чоловіча" : "Жіноча";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().ToLower() == "чоловіча" ? Gender.male : Gender.female;
        }
    }
}