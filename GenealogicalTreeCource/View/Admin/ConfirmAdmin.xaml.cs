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

namespace GenealogicalTreeCource.View.Admin
{
    /// <summary>
    /// Interaction logic for ConfirmAdmin.xaml
    /// </summary>
    public partial class ConfirmAdmin : Page
    {
        public ConfirmAdmin()
        {
            InitializeComponent();
        }
<<<<<<< HEAD
        private void PlaceholderTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox != null)
                {
                    textBox.Text = "";
                }
            }
            else if (sender is PasswordBox passwordBox)
            {
                if (passwordBox != null)
                {
                    passwordBox.Password = "";
                }
            }
=======

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

>>>>>>> 47438618e4d58e9c8a112d910552f3f44b96e690
        }
    }
}
