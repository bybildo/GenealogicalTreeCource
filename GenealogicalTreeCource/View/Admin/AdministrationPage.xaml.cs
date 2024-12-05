using GenealogicalTreeCource.View.Admin;
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

namespace GenealogicalTreeCource.Xaml
{
    /// <summary>
    /// Interaction logic for AdministrationPage.xaml
    /// </summary>
    public partial class AdministrationPage : Window
    {
        public AdministrationPage()
        {
            InitializeComponent();
            SetWindow.Navigate(new ConfirmAdmin());
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Ви ввели: ", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
