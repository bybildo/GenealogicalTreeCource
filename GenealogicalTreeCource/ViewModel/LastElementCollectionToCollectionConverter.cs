using GenealogicalTreeCource.Class;
using System.Windows.Data;

namespace GenealogicalTreeCource.ViewModel
{
    public class LastElementCollectionToCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is List<Person> persons && persons.Count != 0)
            {
                return new List<string> { persons[persons.Count - 1].ForSearch() };
            }

            return new List<string> { "" };
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is List<string> list && list.Count > 0)
            {
                string forSearch = list[0];
                return new List<Person> { MainWindow.myPersonTree.GetPersonFromSearch(forSearch) };
            }

            return null;
        }
    }
}