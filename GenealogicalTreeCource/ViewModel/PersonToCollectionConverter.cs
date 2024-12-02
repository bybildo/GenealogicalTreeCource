using GenealogicalTreeCource.Class;
using System.Windows.Data;

namespace GenealogicalTreeCource.ViewModel
{
    public class PersonToCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Person person)
            {
                return new List<string> { person.ForSearch() };
            }

            return new List<string> { "" };
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is List<string> list && list.Count > 0)
            {
                string forSearch = list[0];
                return new PersonTree().GetPersonFromSearch(forSearch);
            }

            return null;
        }
    }
}

