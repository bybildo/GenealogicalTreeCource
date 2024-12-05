using GenealogicalTreeCource.Class;
using GenealogicalTreeCource.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace GenealogicalTreeCource.View.Admin
{
    /// <summary>
    /// Interaction logic for AddedUsers.xaml
    /// </summary>
    public partial class AddedUsers : Page
    {
        public AddedUsers(TypeOperation typeOperation = TypeOperation.Add)
        {
            InitializeComponent();
            if (typeOperation == TypeOperation.Edit)
            {
                Binding binding = new Binding("EditedPerson")
                {
                    Mode = BindingMode.OneWay,
                    Converter = (IValueConverter)Resources["CollectionToStringCollectionConverter"]
                };

                 // методи
                itemsControl.SetBinding(ListBox.ItemsSourceProperty, binding);

            }
        }
    }
}
