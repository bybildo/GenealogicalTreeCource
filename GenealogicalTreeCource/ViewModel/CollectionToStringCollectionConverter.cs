using GenealogicalTreeCource.Class;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GenealogicalTreeCource.ViewModel
{
    public class CollectionToStringCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<Person> persons && persons.Count != 0)
            {
                var result = persons.Select(el => el.ForSearch()).ToList();
                return result;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<string> list && list.Count > 0)
            {
                var personList = list.Select(el =>
                {
                    var person = new PersonTree().GetPersonFromSearch(el);
                    if (person == null)
                    {
                        // Логіка обробки, коли особа не знайдена
                        MessageBox.Show($"Person with search term '{el}' not found.");
                    }
                    return person;
                }).ToList();

                return personList;
            }
            return null;
        }
    }
}
