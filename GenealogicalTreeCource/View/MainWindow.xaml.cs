using GenealogicalTreeCource.Class;
using System.Windows;
using System.Windows.Input;

namespace GenealogicalTreeCource
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PersonTree personTree = DataContext as PersonTree;
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            AdministratorButton.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            AdministratorButton.Visibility = Visibility.Hidden;
        }
    }
}
