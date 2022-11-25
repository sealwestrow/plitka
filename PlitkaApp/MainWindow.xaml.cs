using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace PlitkaApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public void ShowHello(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello");
        }

        #region Функция перемещения элементов

        int countZ = 0;
        bool _canMove = false;
        Point _offsetPoint = new Point(0, 0);
        Point posCursor = new Point(0, 0);
        private void FF_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _canMove = true;
            countZ++;

            FrameworkElement ffElement = (FrameworkElement)sender;

            Grid.SetZIndex(ffElement, countZ);


            posCursor = e.MouseDevice.GetPosition(this);
            e.MouseDevice.Capture(ffElement);
        }

        private void FF_MouseMove(object sender, MouseEventArgs e)
        {
            if (_canMove == true)
            {
                FrameworkElement ffElement = (FrameworkElement)sender;
                Polygon poly = (Polygon)sender;
                var c = Canv;

                var leftPoint = double.MaxValue;
                var rightPoint = double.MinValue;
                var topPoint = double.MaxValue;
                var bottomPoint = double.MinValue;
                for (int i = 0; i < poly.Points.Count; i++)
                {
                    var point = poly.Points[i];

                    if (point.X < leftPoint)
                        leftPoint = point.X;
                    if (point.X > rightPoint)
                        rightPoint = point.X;
                    if (point.Y < topPoint)
                        topPoint = point.Y;
                    if (point.Y > bottomPoint)
                        bottomPoint = point.Y;
                }

                bool allowMoving = (leftPoint + 1) > 0 && (rightPoint + 1) < Canv.ActualWidth && (topPoint + 1) > 0 && (bottomPoint + 1) < Canv.ActualHeight;

                if (e.MouseDevice.Captured == ffElement)
                {
                    Point p = e.MouseDevice.GetPosition(this);
                    var px = p.X - posCursor.X;
                    var py = p.Y - posCursor.Y;

                    if ((leftPoint + 1) + px > 0 && (rightPoint + 1) + px < Canv.ActualWidth && (topPoint + 1) + py > 0 && (bottomPoint + 1) + py < Canv.ActualHeight)
                    {
                        for (int i = 0; i < poly.Points.Count; i++)
                        {
                            poly.Points[i] = new Point(poly.Points[i].X + px, poly.Points[i].Y + py);
                        }
                        posCursor = e.MouseDevice.GetPosition(this);
                    }
                }
            }
        }

        private void FF_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _canMove = false;
            e.MouseDevice.Capture(null);
        }

        #endregion


        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Polygon ffElement = (Polygon)sender;
            ffElement.StrokeThickness = 5;
        }

        private void CopyF_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                ContextMenu cm = mi.CommandParameter as ContextMenu;
                if (cm != null)
                {
                    var element = cm.PlacementTarget as Polygon;
                    if (element != null)
                    {
                        var newel = new Polygon();
                        foreach(var p in element.Points)
                        {
                            newel.Points.Add(p);
                        }

                        newel.Fill = element.Fill;
                        newel.Stroke = Brushes.Black;
                        newel.MouseLeftButtonDown += FF_MouseLeftButtonDown;
                        newel.MouseLeftButtonUp += FF_MouseLeftButtonUp;
                        newel.MouseMove += FF_MouseMove;

                        newel.MouseRightButtonDown += OnMouseRightButtonDown;


                        Canv.Children.Add(newel);
                        Canvas.SetLeft(newel, 1);
                        Canvas.SetTop(newel, 1);

                        newel.ContextMenu = ConMenu;
                        newel.RenderTransformOrigin = new Point(0.5, 0.5);
                    }
                }
            }

            for (int i = 0; i < Canv.Children.Count; i++)
            {
                var UIElement = (Polygon)Canv.Children[i];
                UIElement.StrokeThickness = 1;
                Canv.Children[i] = UIElement;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                ContextMenu cm = mi.CommandParameter as ContextMenu;
                if (cm != null)
                {
                    var element = cm.PlacementTarget as UIElement;
                    Canv.Children.Remove(element); 

                }
            }
            for (int i = 0; i < Canv.Children.Count; i++)
            {
                var UIElement = (Polygon)Canv.Children[i];
                UIElement.StrokeThickness = 1;
                Canv.Children[i] = UIElement;
            }
        }

        double angleUIElement = 90;
        private void Rotate_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                ContextMenu cm = mi.CommandParameter as ContextMenu;
                if (cm != null)
                {
                    var element = cm.PlacementTarget as UIElement;
                    var rotateRender = new RotateTransform(angleUIElement);
                    element.RenderTransform = rotateRender; 
                    angleUIElement += 90;

                }
            }

            for (int i = 0; i < Canv.Children.Count; i++)
            {
                var UIElement = (Polygon)Canv.Children[i];
                UIElement.StrokeThickness = 1;
                Canv.Children[i] = UIElement;
            }
        }

        private void SwapColor_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                ContextMenu cm = mi.CommandParameter as ContextMenu;
                if(cm != null)
                {
                    var element = cm.PlacementTarget as UIElement;
                    int indexF = Canv.Children.IndexOf(element);

                    SolidColorBrush bs = new SolidColorBrush(colorPicker.Color);
                    Polygon newel = (Polygon)Canv.Children[indexF];
                    newel.Fill = bs;
                    newel.Stroke = Brushes.Black; 
                    Canv.Children.Remove(element);
                    Canv.Children.Insert(indexF, newel);

                }
            }

            for (int i = 0; i < Canv.Children.Count; i++)
            {
                var UIElement = (Polygon)Canv.Children[i];
                UIElement.StrokeThickness = 1;
                Canv.Children[i] = UIElement;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if(sfd.ShowDialog() == true)
            {
                if(sfd.FileName != "")
                {
                    ToImageSource(Canv, sfd.FileName + ".png");
                    ToTextSource(Canv, sfd.FileName + ".txt");
                }
            }
        }

        private void ToTextSource(Canvas c, string fileName)
        {
            using (var sw = new StreamWriter(fileName))
            {
                for (int i = 0; i < Canv.Children.Count; i++)
                {
                    var polygon = Canv.Children[i] as Polygon;
                    foreach (var p in polygon.Points)
                    {
                        sw.Write("{0} {1};", p.X, p.Y);
                    }
                    sw.Write(polygon.Fill.ToString());
                    sw.WriteLine();
                }
            }
        }
        //private void Open_Click(object sender, RoutedEventArgs e)
        //{
        //    SaveFileDialog sfd = new SaveFileDialog();
        //    if (sfd.ShowDialog() == true)
        //    {
        //        if (sfd.FileName != "")
        //        {
                    
        //        }
        //    }
        //}

        private static void ToImageSource(Canvas canvas, string filename)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)canvas.ActualWidth, (int)canvas.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            canvas.Measure(new Size((int)canvas.ActualWidth, (int)canvas.ActualHeight));
            canvas.Arrange(new Rect(new Size((int)canvas.ActualWidth, (int)canvas.ActualHeight)));
            bmp.Render(canvas);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (FileStream file = File.Create(filename))
            {
                encoder.Save(file);
            }
        }

        private void Rectangle_Click(object sender, RoutedEventArgs e)
        {
            var Rectangle = new Polygon();
            Rectangle.Points = new PointCollection() { new Point(0, 0), new Point(100, 0), new Point(100, 100), new Point(0, 100) };
            SolidColorBrush bs = new SolidColorBrush(colorPicker.Color);
            Rectangle.Fill = bs.ToString() == "#00FFFFFF" ? Brushes.White : bs;
            Rectangle.Stroke = Brushes.Black;
            Rectangle.MouseLeftButtonDown += FF_MouseLeftButtonDown;
            Rectangle.MouseLeftButtonUp += FF_MouseLeftButtonUp;
            Rectangle.MouseMove += FF_MouseMove;
            Rectangle.MouseRightButtonDown += OnMouseRightButtonDown;

            Canv.Children.Add(Rectangle);
            Canvas.SetLeft(Rectangle, 1);
            Canvas.SetTop(Rectangle, 1);

            Rectangle.ContextMenu = ConMenu;
            Rectangle.RenderTransformOrigin = new Point(0.5, 0.5);
        }
       
        private void Triangle_Click(object sender, RoutedEventArgs e)
        {
            var Triangle = new Polygon();
            Triangle.Points = new PointCollection() { new Point(0, 0), new Point(100, 0), new Point(50, 86.6) };
            SolidColorBrush bs = new SolidColorBrush(colorPicker.Color);
            Triangle.Fill = bs.ToString() == "#00FFFFFF" ? Brushes.White : bs;
            Triangle.Stroke = Brushes.Black;
            Triangle.MouseLeftButtonDown += FF_MouseLeftButtonDown;
            Triangle.MouseLeftButtonUp += FF_MouseLeftButtonUp;
            Triangle.MouseMove += FF_MouseMove;

            Triangle.MouseRightButtonDown += OnMouseRightButtonDown;

            Canv.Children.Add(Triangle);
            Canvas.SetLeft(Triangle, 1);
            Canvas.SetTop(Triangle, 1);

            Triangle.ContextMenu = ConMenu;
            Triangle.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        private void Hexagon_Click(object sender, RoutedEventArgs e)
        {
            var Hexagon = new Polygon();
            Hexagon.Points = new PointCollection() { new Point(0,50), new Point(0, 150), new Point(86.6,200), new Point(173.21,150), new Point(173.21, 50), new Point(86.6,0) };
            SolidColorBrush bs = new SolidColorBrush(colorPicker.Color);
            Hexagon.Fill = bs.ToString() == "#00FFFFFF" ? Brushes.White : bs;
            Hexagon.Stroke = Brushes.Black;
            Hexagon.MouseLeftButtonDown += FF_MouseLeftButtonDown;
            Hexagon.MouseLeftButtonUp += FF_MouseLeftButtonUp;
            Hexagon.MouseMove += FF_MouseMove;

            Hexagon.MouseRightButtonDown += OnMouseRightButtonDown;


            Canv.Children.Add(Hexagon);
            Canvas.SetLeft(Hexagon, 1);
            Canvas.SetTop(Hexagon, 1);

            Hexagon.ContextMenu = ConMenu;
            Hexagon.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        private void Octagon_Click(object sender, RoutedEventArgs e)
        {
            var Octagon = new Polygon();
            Octagon.Points = new PointCollection() { new Point(0, 100), new Point(0, 200), new Point(70.71,270.71), new Point(170.71, 270.71), new Point(241.42, 200), new Point(241.42, 100), new Point(170.71,29.29), new Point(70.71, 29.29)  };
            SolidColorBrush bs = new SolidColorBrush(colorPicker.Color);
            Octagon.Fill = bs.ToString() == "#00FFFFFF" ? Brushes.White : bs;
            Octagon.Stroke = Brushes.Black;
            Octagon.MouseLeftButtonDown += FF_MouseLeftButtonDown;
            Octagon.MouseLeftButtonUp += FF_MouseLeftButtonUp;
            Octagon.MouseMove += FF_MouseMove;

            Octagon.MouseRightButtonDown += OnMouseRightButtonDown;


            Canv.Children.Add(Octagon);
            Canvas.SetLeft(Octagon, 1);
            Canvas.SetTop(Octagon, 1);

            Octagon.ContextMenu = ConMenu;
            Octagon.RenderTransformOrigin = new Point(0.5, 0.5);
        }
        private void OnCanvasClick(object sender, MouseButtonEventArgs e)
        {

            for (int i = 0; i < Canv.Children.Count; i++)
            {
                var p=  Canv.Children[i] as Polygon;
                p.StrokeThickness = 1;
                Canv.Children[i] = p as UIElement;
            }
        }

        private void Delete_Click_1(object sender, RoutedEventArgs e)
        {
            Canv.Children.Clear();
        }
    }
}
