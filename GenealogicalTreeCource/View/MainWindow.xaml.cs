using GenealogicalTreeCource.Model.Enum;
using GenealogicalTreeCource.Xaml;
using System.Text;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            SetWindow.Navigate(new OperationWithPerson(TypeOperation.Add));
        }

        private void TreeButton_Click(object sender, RoutedEventArgs e)
        {
            SetWindow.Navigate(new PersonTree());
        }
        private void AdministratorButton_Click(object sender, RoutedEventArgs e)
        {
            SetWindow.Navigate(new AdministrationPage());
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SetWindow.Navigate(null);
        }
    }
}