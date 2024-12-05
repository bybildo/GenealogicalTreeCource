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
            PersonTree myPersonTree = DataContext as PersonTree;
            var elements = (Grid)this.FindName("Elements");

            switch (option)
            {
                case TypeOperation.Add:
                    {
                        var binding = new Binding("ChooseAdd")
                        {
                            Source = myPersonTree
                        };

                        elements.SetBinding(Grid.DataContextProperty, binding);

                        ForAdd(Elements);

                        break;
                    }
                case TypeOperation.View:
                    {
                        var binding = new Binding("ChoosePersonaId")
                        {
                            Source = myPersonTree,
                            Converter = (IValueConverter)Resources["IdToPersonConverter"]
                        };

                        elements.SetBinding(Grid.DataContextProperty, binding);

                        ForView(Elements);
                        break;
                    }
                default:
                    {
                        Back2.Visibility = Visibility.Collapsed;
                        var binding = new Binding("ChooseEdit")
                        {
                            Source = myPersonTree
                        };

                        elements.SetBinding(Grid.DataContextProperty, binding);

                        ForEdit(Elements);
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

        private void ForAdd(UIElement element)
        {
            if (element == null)
                return;

            if (element is Panel panel)
            {
                foreach (UIElement child in panel.Children)
                {
                    ForAdd(child);
                }
            }
            else if (element is ContentControl contentControl && contentControl.Content is UIElement content)
            {
                ForAdd(content);
            }
            else if (element is Decorator decorator && decorator.Child is UIElement childElement)
            {
                ForAdd(childElement);
            }
            else if (element is FrameworkElement frameworkElement)
            {
                if (frameworkElement.Tag?.ToString() == "Add")
                    frameworkElement.Visibility = Visibility.Visible;
                else if (frameworkElement.Tag?.ToString() == "View")
                    frameworkElement.Visibility = Visibility.Collapsed;
                else if (frameworkElement.Tag?.ToString() == "ViewB")
                    frameworkElement.Visibility = Visibility.Collapsed;
                else if (frameworkElement.Tag?.ToString() == "TextBox")
                    frameworkElement.Visibility = Visibility.Visible;
                else if (frameworkElement.Tag?.ToString() == "ListBox")
                    frameworkElement.Visibility = Visibility.Collapsed;
            }
        }

        private void ForView(UIElement element)
        {
            if (element == null)
                return;

            if (element is Panel panel)
            {
                foreach (UIElement child in panel.Children)
                {
                    ForView(child);
                }
            }
            else if (element is ContentControl contentControl && contentControl.Content is UIElement content)
            {
                ForView(content);
            }
            else if (element is Decorator decorator && decorator.Child is UIElement childElement)
            {
                ForView(childElement);
            }
            else if (element is FrameworkElement frameworkElement)
            {
                if (frameworkElement is TextBox textBox)
                {
                    textBox.IsEnabled = false;
                    return;
                }
                else if (frameworkElement is ComboBox comboBox)
                {
                    comboBox.IsEnabled = false;
                    return;
                }
                else if (frameworkElement is DatePicker datePicker)
                {
                    datePicker.IsEnabled = false;
                    return;
                }
            }

            DeadCheckBox.IsChecked = true;
            DeadCheckBox.Visibility = Visibility.Hidden;
        }

        private void ForEdit(UIElement element)
        {

            if (element == null)
                return;

            if (element is Panel panel)
            {
                foreach (UIElement child in panel.Children)
                {
                    ForEdit(child);
                }
            }
            else if (element is ContentControl contentControl && contentControl.Content is UIElement content)
            {
                ForEdit(content);
            }
            else if (element is Decorator decorator && decorator.Child is UIElement childElement)
            {
                ForEdit(childElement);
            }
            else if (element is FrameworkElement frameworkElement)
            {
                if (frameworkElement is TextBox textBox)
                {
                    textBox.IsEnabled = false;
                }
                else if (frameworkElement is ListBox listBox)
                {
                    listBox.IsEnabled = false;
                }
                else if (frameworkElement is ComboBox comboBox)
                {
                    comboBox.IsEnabled = false;
                }
                else if (frameworkElement is DatePicker datePicker)
                {
                    datePicker.IsEnabled = false;
                }
                else if (frameworkElement.Tag?.ToString() == "edit")
                {
                   frameworkElement.Visibility = Visibility.Visible;
                }
            }

            NameTextBox.IsEnabled = true;
            NameTextBox.Background = new SolidColorBrush(Colors.Yellow);
            SurnameTextBox.IsEnabled = true;
            SurnameTextBox.Background = new SolidColorBrush(Colors.Yellow);
            DeadCheckBox.IsChecked = true;
            DeadCheckBox.Visibility = Visibility.Hidden;
            DeathDatePicker.IsEnabled = true;
            DeathDatePicker.Background = new SolidColorBrush(Colors.Yellow);
            Edit.Visibility = Visibility.Collapsed;
        }

        private void PlaceholderTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox.Name == "opt")
            {
                SurnameTextBox.Text = "";
                FathernameTextBox.Text = "";
            }
            if (textBox != null)
            {
                textBox.Text = "";
            }
        }
        private void DeadCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DeathDatePicker.SelectedDate = null;
        }
    }
}
