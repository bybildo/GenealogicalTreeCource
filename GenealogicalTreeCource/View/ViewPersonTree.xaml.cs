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
    /// <summary>
    /// Interaction logic for PersonTree.xaml
    /// </summary>
    public partial class ViewPersonTree : Page
    {
        public ViewPersonTree()
        {
            InitializeComponent();
            DataContext = MainWindow.myPersonTree;
        }

        private void ViewPerson(object sender, MouseButtonEventArgs e)
        {
            var listBoxItem = sender as ListBoxItem;
            string forSearch = listBoxItem.DataContext as string;

            var selectedPerson = MainWindow.myPersonTree.GetPersonFromSearch(forSearch);

            if (selectedPerson != null)
                NavigationService.Navigate(new OperationWithPerson(selectedPerson, TypeOperation.View));
        }
    }
}

