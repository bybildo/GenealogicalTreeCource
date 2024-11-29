using GenealogicalTreeCource.Class;
using GenealogicalTreeCource.Enum;
using GenealogicalTreeCource.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GenealogicalTreeCource
{
    public partial class ViewPersonTree : Page
    {
        public ViewPersonTree()
        {
            InitializeComponent();
            DataContext = MainWindow.myPersonTree;

            GenealogyCanvas.MouseWheel += GenealogyCanvas_MouseWheel;
            GenealogyCanvas.MouseDown += GenealogyCanvas_MouseDown;
            GenealogyCanvas.MouseMove += GenealogyCanvas_MouseMove;
            GenealogyCanvas.MouseUp += GenealogyCanvas_MouseUp;

            ScaleTransform.ScaleX = 0.5;
            ScaleTransform.ScaleY = 0.5;
        }

        private void ViewPerson(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem listBoxItem)
            {
                var forSearch = listBoxItem.DataContext as string;
                var selectedPerson = MainWindow.myPersonTree.GetPersonFromSearch(forSearch);

                if (selectedPerson != null)
                {
                    GenealogyCanvas.Children.Clear();
                    DrawDownTree(selectedPerson, selectedPerson.GetKness());
                }
            }
        }

        private bool DrawDownTree(Person person, int NumOfKnees, double posX = 375, double posY = 60)
        {
            if (person == null || NumOfKnees == 0)
                return false;

            DrawRectangle(person.ToString(), NumOfKnees, posX, posY);

            double horizontalSpacing = 100 * Math.Pow(2, NumOfKnees - 1);
            double verticalSpacing = Math.Min(100 + ((NumOfKnees - 1) * 30), 300);

            if (!person.Father.Fathername.Contains("*невідомо*") || person.Father != null)
            {
                if (DrawDownTree(person.Father, NumOfKnees - 1, posX - horizontalSpacing, posY + verticalSpacing))
                    DrawArrow(posX, posY, posX - horizontalSpacing + 110, posY + verticalSpacing);
            }

            if (!person.Mother.Fathername.Contains("*невідомо*") || person.Mother != null)
            {
                if (DrawDownTree(person.Mother, NumOfKnees - 1, posX + horizontalSpacing, posY + verticalSpacing))
                    DrawArrow(posX, posY, posX + horizontalSpacing + 110, posY + verticalSpacing);
            }

            return true;
        }

        private void DrawUpTree(Person person, int NumOfKnees, double posX = 375, double posY = 60)
        {
                      
        }

        private void DrawRectangle(string text, int NumFoKnees, double x, double y)
        {
            Rectangle rect = new Rectangle
            {
                Width = 220,
                Height = 60,
                Stroke = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                Fill = Brushes.LightGray,
            };

            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            GenealogyCanvas.Children.Add(rect);

            TextBlock tb = new TextBlock
            {
                Text = text,
                FontWeight = FontWeights.Bold,
                FontSize = 12,
                Width = rect.Width,
                Height = rect.Height,
                TextAlignment = TextAlignment.Center,
            };

            Canvas.SetLeft(tb, x);
            Canvas.SetTop(tb, y + rect.Height / 4);

            tb.MouseDown += (sender, args) =>
             {
                 if (args.ClickCount == 2)
                 {
                     var selectedPerson = MainWindow.myPersonTree.GetPersonFromToString(tb.Text.ToString());

                     if (selectedPerson != null)
                         NavigationService.Navigate(new OperationWithPerson(selectedPerson, TypeOperation.View));
                 }
             };
            GenealogyCanvas.Children.Add(tb);
        }

        private void DrawArrow(double Xpoint, double Ypoint, double Xend, double Yend)
        {
            double height = Yend - (Ypoint + 60);

            Line line1 = new Line
            {
                X1 = Xpoint + 110,
                Y1 = Ypoint + 60,
                X2 = Xpoint + 110,
                Y2 = Ypoint + 60 + (height / 2),
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                StrokeEndLineCap = PenLineCap.Triangle
            };

            Line line2 = new Line
            {
                X1 = line1.X2,
                Y1 = line1.Y2,
                X2 = Xend,
                Y2 = line1.Y2,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                StrokeEndLineCap = PenLineCap.Triangle
            };

            Line line3 = new Line
            {
                X1 = line2.X2,
                Y1 = line2.Y2,
                X2 = line2.X2,
                Y2 = Yend,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                StrokeEndLineCap = PenLineCap.Triangle
            };

            GenealogyCanvas.Children.Add(line1);
            GenealogyCanvas.Children.Add(line2);
            GenealogyCanvas.Children.Add(line3);
        }

        #region Переміщення на полотні
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
