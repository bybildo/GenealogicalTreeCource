using GenealogicalTreeCource.Enum;
using System.Globalization;
using System.Windows.Data;

namespace GenealogicalTreeCource.ViewModel
{
    public class GenderToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return false;

            var gender = (Gender)value;
            var genderParameter = (string)parameter;

            return gender.ToString().Equals(genderParameter, StringComparison.OrdinalIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isSelected && isSelected)
            {
                var genderParameter = (string)parameter;
                return genderParameter.Equals("Male", StringComparison.OrdinalIgnoreCase) ? Gender.male : Gender.female;
            }
            return Gender.unknown;
        }
    }
}