using GenealogicalTreeCource.Class;
using GenealogicalTreeCource.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
                case TypeOperation.View:
                    {
                        OffCheckBox();
                        break;
                    }
                case TypeOperation.Edit:
                    {
                        SetColor();
                        OffCheckBox();
                        break;
                    }
                default: break;
            }
        }

        private void SetColor()
        {
            NameTextBox.Background = new SolidColorBrush(Colors.Yellow);
            SurnameTextBox.Background = new SolidColorBrush(Colors.Yellow);
            DeathDatePicker.Background = new SolidColorBrush(Colors.Yellow);
        }

        private void OffCheckBox()
        {
            DeadCheckBox.IsChecked = true;
            DeadCheckBox.Visibility = Visibility.Hidden;
        }

        private void PlaceholderTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                textBox.Text = "";
            }

            if (textBox.Name == "opt")
            {
                SurnameTextBox.Text = "";
                FathernameTextBox.Text = "";
            }
        }

        private void DeadCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DeathDatePicker.SelectedDate = null;
        }
    }
}
