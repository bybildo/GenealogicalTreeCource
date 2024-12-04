using GenealogicalTreeCource.Class;
using GenealogicalTreeCource.Enum;
using GenealogicalTreeCource.Model.Enum;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace GenealogicalTreeCource
{
    public partial class ViewPersonTree : Page
    {
        private PersonTree myPersonTree;
        private GraphGenerator graphGenerator;

        public ViewPersonTree()
        {
            InitializeComponent();
            myPersonTree = DataContext as PersonTree;
            graphGenerator = new GraphGenerator(GenealogyCanvas);
            ForMove();
        }

        private void ViewPerson(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem listBoxItem)
            {
                var forSearch = listBoxItem.DataContext as string;
                var selectedPerson = myPersonTree.GetPersonFromSearch(forSearch);

                if (selectedPerson != null)
                {
                    myPersonTree.Filter1 = forSearch;
                    myPersonTree.Show1 = Visibility.Hidden;
                    GenealogyCanvas.Children.Clear();
                    graphGenerator.DrawUpTree(selectedPerson, selectedPerson.GetUpKness());
                    graphGenerator.DrawDownTree(selectedPerson, selectedPerson.GetDownKness());
                }
            }
        }

        private void PlaceholderTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                textBox.Text = "";
            }
        }

        #region Переміщення на полотні
        private void ForMove()
        {
            GenealogyCanvas.MouseWheel += GenealogyCanvas_MouseWheel;
            GenealogyCanvas.MouseDown += GenealogyCanvas_MouseDown;
            GenealogyCanvas.MouseMove += GenealogyCanvas_MouseMove;
            GenealogyCanvas.MouseUp += GenealogyCanvas_MouseUp;

            ScaleTransform.ScaleX = 0.5;
            ScaleTransform.ScaleY = 0.5;
        }

        private bool isDragging = false;
        private Point lastMousePosition;

        private void GenealogyCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            const double zoomFactor = 0.1;
            double scale = ScaleTransform.ScaleX;

            if (e.Delta > 0)
                scale += zoomFactor;
            else if (e.Delta < 0)
                scale -= zoomFactor;

            double minScale = 0.15;
            double maxScale = 3;

            scale = Math.Max(minScale, Math.Min(scale, maxScale));

            ScaleTransform.ScaleX = scale;
            ScaleTransform.ScaleY = scale;

            Point mousePosition = e.GetPosition(GenealogyCanvas);

            TranslateTransform.X -= (mousePosition.X * zoomFactor) * (e.Delta > 0 ? 1 : -1);
            TranslateTransform.Y -= (mousePosition.Y * zoomFactor) * (e.Delta > 0 ? 1 : -1);
        }

        private void GenealogyCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDragging = true;
                lastMousePosition = e.GetPosition(this);
                GenealogyCanvas.CaptureMouse();
            }
        }

        private void GenealogyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentMousePosition = e.GetPosition(this);
                double offsetX = currentMousePosition.X - lastMousePosition.X;
                double offsetY = currentMousePosition.Y - lastMousePosition.Y;

                TranslateTransform.X += offsetX;
                TranslateTransform.Y += offsetY;

                lastMousePosition = currentMousePosition;
            }
        }

        private void GenealogyCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                isDragging = false;
                GenealogyCanvas.ReleaseMouseCapture();
            }
        }
        #endregion
    }
}