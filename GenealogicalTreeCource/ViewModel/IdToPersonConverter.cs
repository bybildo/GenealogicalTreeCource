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
    public class IdToPersonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int id)
            {
                PersonTree personTree = new PersonTree();
                if (0 <= id && id <= personTree.NumberOfLastElement())
                {
                    return personTree.GetPersonFromID(id);
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
