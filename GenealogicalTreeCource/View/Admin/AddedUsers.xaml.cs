using GalaSoft.MvvmLight.Command;
using GenealogicalTreeCource.Class;
using GenealogicalTreeCource.Model.Enum;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;


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
                itemsControl.Visibility = Visibility.Collapsed;
                itemsEditControl.Visibility = Visibility.Visible;
                option.Text = "Перегляд редагування";
            }


        }
    }
}
