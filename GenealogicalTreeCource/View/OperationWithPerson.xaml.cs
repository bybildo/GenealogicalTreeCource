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

namespace GenealogicalTreeCource
{
    public partial class OperationWithPerson : Page
    {
        /// <summary>
        /// <para>Add - Вікно додавання</para>
        /// <para>View - Вікно перегляду</para>
        /// <para>Edit - Вікно редагування</para>
        /// </summary>

        public OperationWithPerson(TypeOperation option)
        {
            InitializeComponent();
    
            switch (option)

            {
                case TypeOperation.Add:
                    {
                        break;
                    }
                case TypeOperation.View:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void ViewPerson(object sender, MouseButtonEventArgs e)
        {
            var listBoxItem = sender as ListBoxItem;
            string forSearch = listBoxItem.DataContext as string;

            var selectedPerson = new PersonTree().GetPersonFromSearch(forSearch);

            if (selectedPerson != null)
                NavigationService.Navigate(new OperationWithPerson(TypeOperation.View));
        }
    }
}
