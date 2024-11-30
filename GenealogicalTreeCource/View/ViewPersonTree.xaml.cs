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
        public ViewPersonTree()
        {
            InitializeComponent();
            DataContext = MainWindow.myPersonTree;

            ForPer();
        }

        #region Методи створення графу
        //Створив окремий клас для цього, але працювало все окрім переміщення, прийшлось перемістити сюди задля коректної роботи створення графа
        private void ViewPerson(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBoxItem listBoxItem)
            {
                var forSearch = listBoxItem.DataContext as string;
                var selectedPerson = MainWindow.myPersonTree.GetPersonFromSearch(forSearch);

                if (selectedPerson != null)
                {
                    GenealogyCanvas.Children.Clear();
                    DrawUpTree(selectedPerson, selectedPerson.GetUpKness());
                    DrawDownTree(selectedPerson, selectedPerson.GetDownKness());
                }
            }
        }

        private bool DrawDownTree(Person person, int NumOfKnees, double posX = 375, double posY = 60)
        {
            if (person == null || NumOfKnees == 0)
                return false;

            DrawRectangle(person.ToString(), posX, posY);

            double horizontalSpacing = 100 * Math.Pow(2, NumOfKnees - 1);
            double verticalSpacing = Math.Min(100 + ((NumOfKnees - 1) * 30), 300);

            if (person.Father != null && !person.Father.Fathername.Contains("*невідомо*"))
            {
                if (DrawDownTree(person.Father, NumOfKnees - 1, posX - horizontalSpacing, posY + verticalSpacing))
                    DrawDownArrow(posX, posY, posX - horizontalSpacing + 110, posY + verticalSpacing);
            }

            if (person.Mother != null && !person.Mother.Fathername.Contains("*невідомо*"))
            {
                if (DrawDownTree(person.Mother, NumOfKnees - 1, posX + horizontalSpacing, posY + verticalSpacing))
                    DrawDownArrow(posX, posY, posX + horizontalSpacing + 110, posY + verticalSpacing);
            }

            return true;
        }

        private void DrawUpTree(Person person, int NumOfKnees, double posX = 375, double posY = 130)
        {
            if (person == null || NumOfKnees == 0)
                return;


            double horizontalSpacing = 250 * Math.Pow(2, NumOfKnees - 1);
            double verticalSpacing = Math.Min((Math.Max(NumOfKnees, 2)) * 80, 480);
            double Xfirst = posX;
            double Yfirst = posY;
            if (Yfirst == 130) Yfirst -= 30;

            posY -= verticalSpacing;

            if (person.GenderPerson == Gender.male)
            {
                int numOfChild = person.Children.Count;
                for (int i = 0; i < numOfChild; i++)
                {
                    DrawUpArrow(Xfirst, Yfirst, posX + 410, posY);
                    DrawRectangle(person.Children[i].ToString(), posX + 250, posY);
                    DrawOneArrow(posX + 110 - 250, posY+60, posX + 110 - 250, posY + 80);
                    DrawRectangle(person.Children[i].Mother.ToString(), posX + 250, posY + 80);
                    DrawUpTree(person.Children[i], NumOfKnees - 1, posX + 250, posY);
                    posX += horizontalSpacing;
                }
            }
            else
            {
                int numOfChild = person.Children.Count;
                for (int i = 0; i < numOfChild; i++)
                {
                    DrawUpArrow(Xfirst, Yfirst, posX - 30, posY);
                    DrawRectangle(person.Children[i].ToString(), posX - 250, posY);
                    DrawOneArrow(posX + 110 -250, posY+60, posX + 110 - 250, posY + 80);
                    DrawRectangle(person.Children[i].Father.ToString(), posX - 250, posY + 80);
                    DrawUpTree(person.Children[i], NumOfKnees - 1, posX - 250, posY);
                    posX -= horizontalSpacing;
                }
            }
        }

        private void DrawRectangle(string text, double x, double y)
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

        private void DrawUpArrow(double Xpoint, double Ypoint, double Xend, double Yend)
        {
            double height = Ypoint - Yend;

            Line line1 = new Line
            {
                X1 = Xpoint + 110,
                Y1 = Ypoint,
                X2 = Xpoint + 110,
                Y2 = Yend + 30,
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

            GenealogyCanvas.Children.Insert(0, line1);
            GenealogyCanvas.Children.Insert(0, line2);
        }

        private void DrawDownArrow(double Xpoint, double Ypoint, double Xend, double Yend)
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

        private void DrawOneArrow(double Xpoint, double Ypoint, double Xend, double Yend)
        {
            Line line1 = new Line
            {
                X1 = Xpoint,
                Y1 = Ypoint,
                X2 = Xend,
                Y2 = Yend,
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                StrokeEndLineCap = PenLineCap.Triangle
            };
            GenealogyCanvas.Children.Add(line1);
        }

        #region Переміщення на полотні
        private void ForPer()
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
        #endregion
    }
}