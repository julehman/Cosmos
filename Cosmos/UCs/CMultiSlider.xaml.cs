using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Cosmos
{
    public class Thumb
    {
        public Rectangle Shape { get; set; }
    }

    public partial class CMultiSlider : UserControl
    {
        //public int changedEventDelayMS = 100;

        public event EventHandler OnValueChanged;

        List<Thumb> thumbs = new List<Thumb>();
        List<Rectangle> bars = new List<Rectangle>();

        List<Thumb> thumbs_a = new List<Thumb>();
        List<Rectangle> bars_a = new List<Rectangle>();

        List<byte[]> colorValues = new List<byte[]>();
        /// <summary>
        /// byte[] with Position, r, g, b. 0 - 255 based
        /// </summary>
        public List<byte[]> ColorValues
        {
            get
            {
                ExportColor();
                return colorValues;
            }
            private set { }
        }

        List<byte[]> transparencyValues = new List<byte[]>();
        /// <summary>
        /// byte[] with Position, transparency. 0 - 255 based
        /// </summary>
        public List<byte[]> TransparencyValues
        {
            get
            {
                ExportTransparency();
                return transparencyValues;
            }
            private set { }
        }

        List<double[]> colorValuesNormalized = new List<double[]>();
        /// <summary>
        /// byte[] with Position, r, g, b. 0 - 1 based
        /// </summary>
        public List<double[]> ColorValuesNormalized
        {
            get
            {
                return colorValuesNormalized;
            }
            set
            {
                colorValuesNormalized = value;
                ImportColor();
            }
        }

        List<double[]> transparencyValuesNormalized = new List<double[]>();
        /// <summary>
        /// byte[] with Position, transparency. 0 - 1 based
        /// </summary>
        public List<double[]> TransparencyValuesNormalized
        {
            get
            {
                return transparencyValuesNormalized;
            }
            set
            {
                transparencyValuesNormalized = value;
                ImportTransparency();
            }
        }

        bool leftMouseThumb = false;
        int thumbWidth = 12;

        Rectangle currentThumb;
        Rectangle CurrentThumb
        {
            get
            {
                return currentThumb;
            }
            set
            {
                currentThumb = value;
                ResetStrokes();
                currentThumb.StrokeThickness = 3;
            }
        }

        private void ImportTransparency()
        {
            thumbs_a.Clear();
            GRD_a_thumbs.Children.Clear();

            for (int i = 0; i < TransparencyValuesNormalized.Count; i++)
            {
                Thumb newThumb = new Thumb()
                {
                    Shape = new Rectangle()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        SnapsToDevicePixels = true,

                        Margin = new Thickness(GRD_a_thumbs.ActualWidth * TransparencyValuesNormalized[i][0],
                                               GRD_a_thumbs.Margin.Top - ((GRD_a_slider.ActualHeight - GRD_a_thumbs.ActualHeight) / 2), 0, 0),

                        Height = GRD_slider.ActualHeight,
                        Width = thumbWidth,

                        StrokeThickness = 1,

                        Fill = new SolidColorBrush(new Color() { R = 0, G = 0, B = 0, A = Convert.ToByte(TransparencyValuesNormalized[i][1] * 255) }),
                    }
                };

                //would not show if it is 1
                if (TransparencyValuesNormalized[i][0] == 1)
                    newThumb.Shape.Margin = new Thickness(newThumb.Shape.Margin.Left - 12, newThumb.Shape.Margin.Top, 0, 0);

                if (Convert.ToInt32(TransparencyValuesNormalized[i][1]) > 125)
                    newThumb.Shape.Stroke = Brushes.White;
                else
                    newThumb.Shape.Stroke = Brushes.Black;

                newThumb.Shape.MouseLeftButtonDown += new MouseButtonEventHandler(ThumbAMouseLeftButtonDown);
                newThumb.Shape.MouseLeftButtonUp += new MouseButtonEventHandler(ThumbAMouseLeftButtonUp);
                newThumb.Shape.MouseMove += new MouseEventHandler(ThumbAMouseMove);
                newThumb.Shape.MouseRightButtonDown += new MouseButtonEventHandler(ThumbAMouseRightButtonDown);

                thumbs_a.Add(newThumb);
                GRD_a_thumbs.Children.Add(thumbs_a.Last().Shape);
            }

            GenerateARectangles();
        }

        private void ImportColor()
        {
            thumbs.Clear();
            GRD_thumbs.Children.Clear();

            for (int i = 0; i < ColorValuesNormalized.Count; i++)
            {
                Thumb newThumb = new Thumb()
                {
                    Shape = new Rectangle()
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        SnapsToDevicePixels = true,

                        Margin = new Thickness((GRD_thumbs.ActualWidth * ColorValuesNormalized[i][0]),
                                               GRD_thumbs.Margin.Top - ((GRD_slider.ActualHeight - GRD_thumbs.ActualHeight) / 2), 0, 0),

                        Height = GRD_slider.ActualHeight,
                        Width = thumbWidth,

                        StrokeThickness = 1,
                        Stroke = Brushes.Black,

                        Fill = new SolidColorBrush(new Color() { R = Convert.ToByte(ColorValuesNormalized[i][1] * 255), G = Convert.ToByte(ColorValuesNormalized[i][2] * 255), B = Convert.ToByte(ColorValuesNormalized[i][3] * 255), A = 255 }),
                    }
                };

                //would not show if it is 1
                if (ColorValuesNormalized[i][0] == 1)
                    newThumb.Shape.Margin = new Thickness(newThumb.Shape.Margin.Left - 12, newThumb.Shape.Margin.Top, 0, 0);

                newThumb.Shape.MouseLeftButtonDown += new MouseButtonEventHandler(ThumbMouseLeftButtonDown);
                newThumb.Shape.MouseLeftButtonUp += new MouseButtonEventHandler(ThumbMouseLeftButtonUp);
                newThumb.Shape.MouseMove += new MouseEventHandler(ThumbMouseMove);
                newThumb.Shape.MouseRightButtonDown += new MouseButtonEventHandler(ThumbMouseRightButtonDown);

                thumbs.Add(newThumb);
                GRD_thumbs.Children.Add(thumbs.Last().Shape);
            }

            GenerateRectangles();
        }
        
        private void ExportTransparency()
        {
            DelayedChangedEvent();

            transparencyValues.Clear();
            transparencyValuesNormalized.Clear();

            for (int i = 0; i < thumbs_a.Count; i++)
            {
                transparencyValues.Add(new byte[] { Convert.ToByte(thumbs_a[i].Shape.Margin.Left / (GRD_slider.ActualWidth /*- thumbWidth*/) * 255),
                                                    (thumbs_a[i].Shape.Fill as SolidColorBrush).Color.A});

                transparencyValuesNormalized.Add(new double[] { Math.Round(thumbs_a[i].Shape.Margin.Left / (GRD_slider.ActualWidth /*- thumbWidth*/), 2),
                                                    Math.Round(Convert.ToDouble((thumbs_a[i].Shape.Fill as SolidColorBrush).Color.A / 255.0), 3) });

            }
        }

        private void DelayedChangedEvent()
        {
            //dont call this every time, check if it was called a few ms before
            OnValueChanged(null, null);
        }

        private void ExportColor()
        {
            DelayedChangedEvent();

            colorValues.Clear();
            colorValuesNormalized.Clear();

            for (int i = 0; i < thumbs.Count; i++)
            {
                colorValuesNormalized.Add(new double[] { Math.Round(thumbs[i].Shape.Margin.Left / (GRD_slider.ActualWidth /*- thumbWidth*/), 2),
                                                Math.Round(Convert.ToDouble((thumbs[i].Shape.Fill as SolidColorBrush).Color.R / 255.0), 3),
                                                Math.Round(Convert.ToDouble((thumbs[i].Shape.Fill as SolidColorBrush).Color.G / 255.0), 3),
                                                Math.Round(Convert.ToDouble((thumbs[i].Shape.Fill as SolidColorBrush).Color.B / 255.0), 3)
                });


                colorValues.Add(new byte[] { Convert.ToByte(thumbs[i].Shape.Margin.Left / (GRD_slider.ActualWidth /*- thumbWidth*/) * 255),
                                                (thumbs[i].Shape.Fill as SolidColorBrush).Color.R,
                                                (thumbs[i].Shape.Fill as SolidColorBrush).Color.G,
                                                (thumbs[i].Shape.Fill as SolidColorBrush).Color.B});
            }
        }

        private void ResetStrokes()
        {
            for (int i = 0; i < thumbs.Count; i++)
                thumbs[i].Shape.StrokeThickness = 1;

            for (int i = 0; i < thumbs_a.Count; i++)
                thumbs_a[i].Shape.StrokeThickness = 1;
        }


        public CMultiSlider()
        {
            InitializeComponent();
            CP_main.C_SelectionChanged += new EventHandler(CP_SelectionChanged);
        }

        private void CP_SelectionChanged(object sender, EventArgs e)
        {
            if (CurrentThumb == null)
                return;

            for (int i = 0; i < thumbs.Count; i++)
            {
                if (thumbs[i].Shape == CurrentThumb)
                {
                    thumbs[i].Shape.Fill = CP_main.GetColorWOTransparency();
                    GenerateRectangles();
                }
            }

            for (int i = 0; i < thumbs_a.Count; i++)
            {
                if (thumbs_a[i].Shape == CurrentThumb)
                {
                    thumbs_a[i].Shape.Fill = new SolidColorBrush(new Color { R = 0, G = 0, B = 0, A = CP_main.GetTransparency() });

                    if (Convert.ToInt32(CP_main.GetTransparency()) > 125)
                        thumbs_a[i].Shape.Stroke = Brushes.White;
                    else
                        thumbs_a[i].Shape.Stroke = Brushes.Black;
                    GenerateARectangles();
                }
            }
        }

        private Point GetMousePosRelative(Rectangle control)
        {
            Window parentWindow = Window.GetWindow(this);
            Point controlPosition = control.PointToScreen(new Point(0d, 0d));
            Point mousePos = TotalMousePosition();

            Point newPos = new Point(mousePos.X - controlPosition.X,
                                     mousePos.Y - controlPosition.Y);

            return newPos;
        }

        private Point GetMousePosRelative(Grid control)
        {
            Window parentWindow = Window.GetWindow(this);
            Point controlPosition = control.PointToScreen(new Point(0d, 0d));
            Point mousePos = TotalMousePosition();

            Point newPos = new Point(mousePos.X - controlPosition.X,
                                     mousePos.Y - controlPosition.Y);

            return newPos;
        }

        private Point GetControlPosInWindow(Rectangle control)
        {
            Window parentWindow = Window.GetWindow(this);
            Point controlPosition = control.PointToScreen(new Point(0d, 0d));

            Point relativeControl = new Point(controlPosition.X - parentWindow.Left - 8,
                             controlPosition.Y - parentWindow.Top - 31);

            return relativeControl;
        }

        private System.Windows.Point TotalMousePosition()
        {
            Window parentWindow = Window.GetWindow(this);
            return parentWindow.PointToScreen(Mouse.GetPosition(parentWindow));
        }

        private SolidColorBrush GetRandomColor()
        {
            Random r = new Random();

            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255),
              (byte)r.Next(1, 255), (byte)r.Next(1, 233)));
            return brush;
        }

        private System.Windows.Point WindowMousePosition()
        {
            Window parentWindow = Window.GetWindow(this);

            Point mousePoint = parentWindow.PointToScreen(Mouse.GetPosition(parentWindow));
            double x = Convert.ToInt32(mousePoint.X - parentWindow.Left) - 8;
            double y = Convert.ToInt32(mousePoint.Y - parentWindow.Top) - 31;
            return new Point(x, y);
        }

        private List<Thumb> GetAThumbsSorted()
        {
            List<Thumb> sorted = new List<Thumb>();

            for (int i = 0; i < thumbs_a.Count; i++)
                sorted.Add(thumbs_a[i]);


            Thumb temp = new Thumb();

            for (int write = 0; write < sorted.Count; write++)
            {
                for (int sort = 0; sort < sorted.Count - 1; sort++)
                {
                    if (sorted[sort].Shape.Margin.Left > sorted[sort + 1].Shape.Margin.Left)
                    {
                        temp = sorted[sort + 1];
                        sorted[sort + 1] = sorted[sort];
                        sorted[sort] = temp;
                    }
                }
            }

            return sorted;
        }

        private List<Thumb> GetThumbsSorted()
        {
            List<Thumb> sorted = new List<Thumb>();

            for (int i = 0; i < thumbs.Count; i++)
                sorted.Add(thumbs[i]);


            Thumb temp = new Thumb();

            for (int write = 0; write < sorted.Count; write++)
            {
                for (int sort = 0; sort < sorted.Count - 1; sort++)
                {
                    if (sorted[sort].Shape.Margin.Left > sorted[sort + 1].Shape.Margin.Left)
                    {
                        temp = sorted[sort + 1];
                        sorted[sort + 1] = sorted[sort];
                        sorted[sort] = temp;
                    }
                }
            }

            return sorted;
        }

        private void GenerateRectangles()
        {
            GRD_bars.Children.Clear();
            bars = new List<Rectangle>();

            List<Thumb> sortedThumbs = GetThumbsSorted();

            for (int i = 0; i < sortedThumbs.Count + 1; i++)
            {
                if (i == 0)
                {
                    //first
                    Rectangle firstBar = new Rectangle()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        SnapsToDevicePixels = true,

                        Width = sortedThumbs[i].Shape.Margin.Left + (thumbWidth / 2),
                        Height = GRD_slider.ActualHeight - 10,
                        Margin = new Thickness(thumbWidth / 2, 5, 0, 0),

                        StrokeThickness = 0,
                        Fill = sortedThumbs[i].Shape.Fill,
                    };
                    firstBar.MouseLeftButtonDown += new MouseButtonEventHandler(BarMouseLeftButtonDown);
                    firstBar.MouseLeftButtonUp += new MouseButtonEventHandler(BarMouseLeftButtonUp);
                    firstBar.MouseMove += new MouseEventHandler(BarMouseMove);

                    bars.Add(firstBar);
                    GRD_bars.Children.Add(bars.Last());
                }
                else if (i == sortedThumbs.Count)
                {
                    //last
                    Rectangle lastBar = new Rectangle()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        SnapsToDevicePixels = true,

                        Width = GRD_slider.ActualWidth - (sortedThumbs[i - 1].Shape.Margin.Left/* + sortedThumbs[i - 1].Shape.Width*/),
                        Height = GRD_slider.ActualHeight - 10,
                        Margin = new Thickness(sortedThumbs[i - 1].Shape.Margin.Left + sortedThumbs[i - 1].Shape.Width, 5, 0, 0),

                        StrokeThickness = 0,
                        Fill = sortedThumbs[i - 1].Shape.Fill,
                    };
                    lastBar.MouseLeftButtonDown += new MouseButtonEventHandler(BarMouseLeftButtonDown);
                    lastBar.MouseLeftButtonUp += new MouseButtonEventHandler(BarMouseLeftButtonUp);
                    lastBar.MouseMove += new MouseEventHandler(BarMouseMove);

                    bars.Add(lastBar);
                    GRD_bars.Children.Add(bars.Last());
                }
                else
                {
                    //in between
                    Rectangle bar = new Rectangle()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        SnapsToDevicePixels = true,

                        Width = (sortedThumbs[i].Shape.Margin.Left + (thumbWidth / 2)) - (sortedThumbs[i - 1].Shape.Margin.Left + (thumbWidth / 2)),
                        Height = GRD_slider.ActualHeight - 10,
                        Margin = new Thickness(sortedThumbs[i - 1].Shape.Margin.Left + (thumbWidth / 2), 5, 0, 0),

                        StrokeThickness = 0,
                        Fill = GetGradientByColor(((sortedThumbs[i - 1].Shape.Fill) as SolidColorBrush).Color, ((sortedThumbs[i].Shape.Fill) as SolidColorBrush).Color),
                    };
                    bar.MouseLeftButtonDown += new MouseButtonEventHandler(BarMouseLeftButtonDown);
                    bar.MouseLeftButtonUp += new MouseButtonEventHandler(BarMouseLeftButtonUp);
                    bar.MouseMove += new MouseEventHandler(BarMouseMove);

                    bars.Add(bar);
                    GRD_bars.Children.Add(bars.Last());
                }
            }

            ExportColor();
        }

        private void GenerateARectangles()
        {
            GRD_a_bars.Children.Clear();
            bars_a = new List<Rectangle>();

            List<Thumb> sortedThumbs = GetAThumbsSorted();

            for (int i = 0; i < sortedThumbs.Count + 1; i++)
            {
                if (i == 0)
                {
                    //first
                    Rectangle firstBar = new Rectangle()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        SnapsToDevicePixels = true,

                        Width = sortedThumbs[i].Shape.Margin.Left + (thumbWidth / 2),
                        Height = GRD_slider.ActualHeight - 10,
                        Margin = new Thickness(thumbWidth / 2, 5, 0, 0),

                        StrokeThickness = 0,
                        Fill = sortedThumbs[i].Shape.Fill,
                    };
                    firstBar.MouseLeftButtonDown += new MouseButtonEventHandler(BarAMouseLeftButtonDown);
                    firstBar.MouseLeftButtonUp += new MouseButtonEventHandler(BarAMouseLeftButtonUp);
                    firstBar.MouseMove += new MouseEventHandler(BarAMouseMove);

                    bars_a.Add(firstBar);
                    GRD_a_bars.Children.Add(bars_a.Last());
                }
                else if (i == sortedThumbs.Count)
                {
                    //last
                    Rectangle lastBar = new Rectangle()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        SnapsToDevicePixels = true,

                        Width = GRD_slider.ActualWidth - (sortedThumbs[i - 1].Shape.Margin.Left /*+ sortedThumbs[i - 1].Shape.Width*/),
                        Height = GRD_slider.ActualHeight - 10,
                        Margin = new Thickness(sortedThumbs[i - 1].Shape.Margin.Left + sortedThumbs[i - 1].Shape.Width, 5, 0, 0),

                        StrokeThickness = 0,
                        Fill = sortedThumbs[i - 1].Shape.Fill,
                    };
                    lastBar.MouseLeftButtonDown += new MouseButtonEventHandler(BarAMouseLeftButtonDown);
                    lastBar.MouseLeftButtonUp += new MouseButtonEventHandler(BarAMouseLeftButtonUp);
                    lastBar.MouseMove += new MouseEventHandler(BarAMouseMove);

                    bars_a.Add(lastBar);
                    GRD_a_bars.Children.Add(bars_a.Last());
                }
                else
                {
                    //in between
                    Rectangle bar = new Rectangle()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        SnapsToDevicePixels = true,

                        Width = (sortedThumbs[i].Shape.Margin.Left + (thumbWidth / 2)) - (sortedThumbs[i - 1].Shape.Margin.Left + (thumbWidth / 2)),
                        Height = GRD_slider.ActualHeight - 10,
                        Margin = new Thickness(sortedThumbs[i - 1].Shape.Margin.Left + (thumbWidth / 2), 5, 0, 0),

                        StrokeThickness = 0,
                        Fill = GetGradientByColor(((sortedThumbs[i - 1].Shape.Fill) as SolidColorBrush).Color, ((sortedThumbs[i].Shape.Fill) as SolidColorBrush).Color),
                    };
                    bar.MouseLeftButtonDown += new MouseButtonEventHandler(BarAMouseLeftButtonDown);
                    bar.MouseLeftButtonUp += new MouseButtonEventHandler(BarAMouseLeftButtonUp);
                    bar.MouseMove += new MouseEventHandler(BarAMouseMove);

                    bars_a.Add(bar);
                    GRD_a_bars.Children.Add(bars_a.Last());
                }
            }

            ExportTransparency();
        }

        private void BarMouseMove(object sender, MouseEventArgs e)
        {
            if (leftMouseThumb)
            {
                for (int i = 0; i < thumbs.Count; i++)
                {
                    if (thumbs[i].Shape == currentThumb)
                    {
                        Point relativePos = GetMousePosRelative(GRD_slider);
                        thumbs[i].Shape.Margin = new Thickness(relativePos.X - (thumbWidth / 2), 0, 0, 0);
                        GenerateRectangles();
                        return;
                    }
                }
            }

        }

        private void BarMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (leftMouseThumb)
            {
                leftMouseThumb = false;
                System.Windows.Forms.Cursor.Clip = System.Drawing.Rectangle.Empty;
            }
        }

        private void BarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //create
            Point newPos = GetMousePosRelative(bars[0]);

            Thumb newThumb = new Thumb()
            {
                Shape = new Rectangle()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    SnapsToDevicePixels = true,

                    Margin = new Thickness(newPos.X,
                                           bars[0].Margin.Top - ((GRD_slider.ActualHeight - bars[0].Height) / 2), 0, 0),

                    Height = GRD_slider.ActualHeight,
                    Width = thumbWidth,

                    StrokeThickness = 1,
                    Stroke = Brushes.Black,

                    Fill = CP_main.GetColorWOTransparency(),
                }
            };
            newThumb.Shape.MouseLeftButtonDown += new MouseButtonEventHandler(ThumbMouseLeftButtonDown);
            newThumb.Shape.MouseLeftButtonUp += new MouseButtonEventHandler(ThumbMouseLeftButtonUp);
            newThumb.Shape.MouseMove += new MouseEventHandler(ThumbMouseMove);
            newThumb.Shape.MouseRightButtonDown += new MouseButtonEventHandler(ThumbMouseRightButtonDown);

            thumbs.Add(newThumb);
            GRD_thumbs.Children.Add(thumbs.Last().Shape);
            GenerateRectangles();

            SelectThumb(thumbs.Last().Shape);
        }

        private void ThumbMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (GRD_thumbs.Children.Count > 2)
            {
                //remove thumb
                for (int i = 0; i < thumbs.Count; i++)
                {
                    if (thumbs[i].Shape == sender as Rectangle)
                    {
                        GRD_thumbs.Children.Remove(thumbs[i].Shape);
                        thumbs.RemoveAt(i);
                        GenerateRectangles();
                        return;
                    }
                }
            }
        }

        private void ThumbMouseMove(object sender, MouseEventArgs e)
        {
            //is over right thumb -> can move
            if (leftMouseThumb && sender as Rectangle == CurrentThumb)
            {
                for (int i = 0; i < thumbs.Count; i++)
                {
                    if (thumbs[i].Shape == sender as Rectangle)
                    {
                        Point relativePos = GetMousePosRelative(GRD_slider);
                        thumbs[i].Shape.Margin = new Thickness(relativePos.X - (thumbWidth / 2), 0, 0, 0);
                        GenerateRectangles();
                        return;
                    }
                }
            }
            //is over wrong thumb and probably cant slide further because the wrong thumb overlapping the moving thumb
            //reverse sort children, fixes the problem
            else if (leftMouseThumb && sender as Rectangle != CurrentThumb)
            {
                List<Rectangle> children = new List<Rectangle>();
                for (int i = 0; i < GRD_thumbs.Children.Count; i++)
                    children.Add(GRD_thumbs.Children[i] as Rectangle);

                GRD_thumbs.Children.Clear();

                for (int i = children.Count - 1; i >= 0; i--)
                    GRD_thumbs.Children.Add(children[i]);
            }
        }

        private void ThumbMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            leftMouseThumb = false;
            System.Windows.Forms.Cursor.Clip = System.Drawing.Rectangle.Empty;
        }

        private void ThumbMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectThumb(sender);
        }

        private void BarAMouseMove(object sender, MouseEventArgs e)
        {
            if (leftMouseThumb)
            {
                for (int i = 0; i < thumbs_a.Count; i++)
                {
                    if (thumbs_a[i].Shape == currentThumb)
                    {
                        Point relativePos = GetMousePosRelative(GRD_a_slider);
                        thumbs_a[i].Shape.Margin = new Thickness(relativePos.X - (thumbWidth / 2), 0, 0, 0);
                        GenerateARectangles();
                        return;
                    }
                }
            }

        }

        private void BarAMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (leftMouseThumb)
            {
                leftMouseThumb = false;
                System.Windows.Forms.Cursor.Clip = System.Drawing.Rectangle.Empty;
            }
        }

        private void BarAMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //create
            Point newPos = GetMousePosRelative(bars_a[0]);

            Thumb newThumb = new Thumb()
            {
                Shape = new Rectangle()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    SnapsToDevicePixels = true,

                    Margin = new Thickness(newPos.X,
                                           bars_a[0].Margin.Top - ((GRD_a_slider.ActualHeight - bars_a[0].Height) / 2), 0, 0),

                    Height = GRD_a_slider.ActualHeight,
                    Width = thumbWidth,

                    StrokeThickness = 1,

                    Fill = new SolidColorBrush(new Color() { R = 0, G = 0, B = 0, A = CP_main.GetTransparency() }),
                }
            };

            if (Convert.ToInt32(CP_main.GetTransparency()) > 125)
                newThumb.Shape.Stroke = Brushes.White;
            else
                newThumb.Shape.Stroke = Brushes.Black;

            newThumb.Shape.MouseLeftButtonDown += new MouseButtonEventHandler(ThumbAMouseLeftButtonDown);
            newThumb.Shape.MouseLeftButtonUp += new MouseButtonEventHandler(ThumbAMouseLeftButtonUp);
            newThumb.Shape.MouseMove += new MouseEventHandler(ThumbAMouseMove);
            newThumb.Shape.MouseRightButtonDown += new MouseButtonEventHandler(ThumbAMouseRightButtonDown);

            thumbs_a.Add(newThumb);
            GRD_a_thumbs.Children.Add(thumbs_a.Last().Shape);
            GenerateARectangles();

            SelectAThumb(thumbs_a.Last().Shape);
        }

        private void ThumbAMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (GRD_a_thumbs.Children.Count > 2)
            {
                //remove thumb
                for (int i = 0; i < thumbs_a.Count; i++)
                {
                    if (thumbs_a[i].Shape == sender as Rectangle)
                    {
                        GRD_a_thumbs.Children.Remove(thumbs_a[i].Shape);
                        thumbs_a.RemoveAt(i);
                        GenerateARectangles();
                        return;
                    }
                }
            }
        }

        private void ThumbAMouseMove(object sender, MouseEventArgs e)
        {
            //is over right thumb -> can move
            if (leftMouseThumb && sender as Rectangle == CurrentThumb)
            {
                for (int i = 0; i < thumbs_a.Count; i++)
                {
                    if (thumbs_a[i].Shape == sender as Rectangle)
                    {
                        Point relativePos = GetMousePosRelative(GRD_a_slider);
                        thumbs_a[i].Shape.Margin = new Thickness(relativePos.X - (thumbWidth / 2), 0, 0, 0);
                        GenerateARectangles();
                        return;
                    }
                }
            }
            //is over wrong thumb and probably cant slide further because the wrong thumb overlapping the moving thumb
            //reverse sort children, fixes the problem
            else if (leftMouseThumb && sender as Rectangle != CurrentThumb)
            {
                List<Rectangle> children = new List<Rectangle>();
                for (int i = 0; i < GRD_a_thumbs.Children.Count; i++)
                    children.Add(GRD_a_thumbs.Children[i] as Rectangle);

                GRD_a_thumbs.Children.Clear();

                for (int i = children.Count - 1; i >= 0; i--)
                    GRD_a_thumbs.Children.Add(children[i]);
            }
        }

        private void ThumbAMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            leftMouseThumb = false;
            System.Windows.Forms.Cursor.Clip = System.Drawing.Rectangle.Empty;
        }

        private void ThumbAMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectAThumb(sender);
        }


        private void SelectThumb(object sender)
        {
            leftMouseThumb = true;
            CurrentThumb = sender as Rectangle;

            //Window parentWindow = Window.GetWindow(this);
            Point controlPosition = GRD_slider.PointToScreen(new Point(0d, 0d));

            System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle(new System.Drawing.Point()
            {
                X = Convert.ToInt32(controlPosition.X + (thumbWidth / 2)),
                Y = Convert.ToInt32(controlPosition.Y + (GRD_slider.ActualHeight / 2))
            },
            new System.Drawing.Size(Convert.ToInt32(GRD_slider.ActualWidth - (thumbWidth) + 1), 0));

            CP_main.SetColor(CurrentThumb.Fill as SolidColorBrush);
        }

        private void SelectAThumb(object sender)
        {
            leftMouseThumb = true;
            CurrentThumb = sender as Rectangle;

            //Window parentWindow = Window.GetWindow(this);
            Point controlPosition = GRD_a_slider.PointToScreen(new Point(0d, 0d));

            System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle(new System.Drawing.Point()
            {
                X = Convert.ToInt32(controlPosition.X + (thumbWidth / 2)),
                Y = Convert.ToInt32(controlPosition.Y + (GRD_a_slider.ActualHeight / 2))
            },
            new System.Drawing.Size(Convert.ToInt32(GRD_a_slider.ActualWidth - (thumbWidth) + 1), 0));

            CP_main.SetColor(CurrentThumb.Fill as SolidColorBrush);
        }





        private LinearGradientBrush GetGradientByColor(Color colorLeft, Color colorRight)
        {
            LinearGradientBrush gradient = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0)
            };
            gradient.GradientStops.Add(new GradientStop(colorLeft, 0));
            gradient.GradientStops.Add(new GradientStop(colorRight, 1));
            return gradient;
        }

        private void CreateStartThumbs()
        {
            //first
            Thumb firstThumb = new Thumb()
            {
                Shape = new Rectangle()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    SnapsToDevicePixels = true,

                    Margin = new Thickness(0, 0, 0, 0),
                    Height = GRD_thumbs.ActualHeight,
                    Width = thumbWidth,

                    StrokeThickness = 1,
                    Stroke = Brushes.Black,

                    Fill = new SolidColorBrush() { Color = new Color() { A = 255, R = 0, G = 0, B = 0 } },
                }
            };
            firstThumb.Shape.MouseLeftButtonDown += new MouseButtonEventHandler(ThumbMouseLeftButtonDown);
            firstThumb.Shape.MouseLeftButtonUp += new MouseButtonEventHandler(ThumbMouseLeftButtonUp);
            firstThumb.Shape.MouseMove += new MouseEventHandler(ThumbMouseMove);
            firstThumb.Shape.MouseRightButtonDown += new MouseButtonEventHandler(ThumbMouseRightButtonDown);

            thumbs.Add(firstThumb);

            GRD_thumbs.Children.Add(thumbs.Last().Shape);

            //last
            Thumb lastThumb = new Thumb()
            {
                Shape = new Rectangle()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    SnapsToDevicePixels = true,

                    Margin = new Thickness(GRD_thumbs.ActualWidth - thumbWidth, 0, 0, 0),
                    Height = GRD_thumbs.ActualHeight,
                    Width = thumbWidth,

                    StrokeThickness = 1,
                    Stroke = Brushes.Black,

                    Fill = new SolidColorBrush() { Color = new Color() { A = 255, R = 255, G = 255, B = 255 } },
                }
            };
            lastThumb.Shape.MouseLeftButtonDown += new MouseButtonEventHandler(ThumbMouseLeftButtonDown);
            lastThumb.Shape.MouseLeftButtonUp += new MouseButtonEventHandler(ThumbMouseLeftButtonUp);
            lastThumb.Shape.MouseMove += new MouseEventHandler(ThumbMouseMove);
            lastThumb.Shape.MouseRightButtonDown += new MouseButtonEventHandler(ThumbMouseRightButtonDown);

            thumbs.Add(lastThumb);
            GRD_thumbs.Children.Add(thumbs.Last().Shape);

            GenerateRectangles();





            //first A
            Thumb firstAThumb = new Thumb()
            {
                Shape = new Rectangle()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    SnapsToDevicePixels = true,

                    Margin = new Thickness(0, 0, 0, 0),
                    Height = GRD_a_thumbs.ActualHeight,
                    Width = thumbWidth,

                    StrokeThickness = 1,
                    Stroke = Brushes.White,

                    Fill = new SolidColorBrush() { Color = new Color() { A = 255, R = 0, G = 0, B = 0 } },
                }
            };
            firstAThumb.Shape.MouseLeftButtonDown += new MouseButtonEventHandler(ThumbAMouseLeftButtonDown);
            firstAThumb.Shape.MouseLeftButtonUp += new MouseButtonEventHandler(ThumbAMouseLeftButtonUp);
            firstAThumb.Shape.MouseMove += new MouseEventHandler(ThumbAMouseMove);
            firstAThumb.Shape.MouseRightButtonDown += new MouseButtonEventHandler(ThumbAMouseRightButtonDown);

            thumbs_a.Add(firstAThumb);

            GRD_a_thumbs.Children.Add(thumbs_a.Last().Shape);

            //last A
            Thumb lastAThumb = new Thumb()
            {
                Shape = new Rectangle()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    SnapsToDevicePixels = true,

                    Margin = new Thickness(GRD_a_thumbs.ActualWidth - thumbWidth, 0, 0, 0),
                    Height = GRD_a_thumbs.ActualHeight,
                    Width = thumbWidth,

                    StrokeThickness = 1,
                    Stroke = Brushes.Black,

                    Fill = new SolidColorBrush() { Color = new Color() { A = 0, R = 0, G = 0, B = 0 } },
                }
            };
            lastAThumb.Shape.MouseLeftButtonDown += new MouseButtonEventHandler(ThumbAMouseLeftButtonDown);
            lastAThumb.Shape.MouseLeftButtonUp += new MouseButtonEventHandler(ThumbAMouseLeftButtonUp);
            lastAThumb.Shape.MouseMove += new MouseEventHandler(ThumbAMouseMove);
            lastAThumb.Shape.MouseRightButtonDown += new MouseButtonEventHandler(ThumbAMouseRightButtonDown);

            thumbs_a.Add(lastAThumb);
            GRD_a_thumbs.Children.Add(thumbs_a.Last().Shape);

            GenerateARectangles();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CreateStartThumbs();
        }
    }
}
