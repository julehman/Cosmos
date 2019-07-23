using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Cosmos
{
    public class CWindowCloseButton : Button
    {
        public enum ButtonType
        {
            None,
            Close,
            Size,
            Down,
        }

        public static readonly DependencyProperty ButtonTypeProperty = DependencyProperty.Register("C_ButtonType", typeof(ButtonType), typeof(CWindowCloseButton), new PropertyMetadata(new PropertyChangedCallback(ButtonTypeValueChanged)));

        static CWindowCloseButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CWindowCloseButton), new FrameworkPropertyMetadata(typeof(CWindowCloseButton)));
        }

        public ButtonType C_ButtonType
        {
            get
            {
                return (ButtonType)GetValue(ButtonTypeProperty);
            }
            set
            {
                SetValue(ButtonTypeProperty, value);
            }
        }

        private static void ButtonTypeValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CWindowCloseButton)d;

            ButtonType newType = (ButtonType)e.NewValue;

            if (newType == ButtonType.Close)
            {
                control.Content = new Image { Source = new BitmapImage(new Uri("/Cosmos;component/Images/close.png", UriKind.Relative)), VerticalAlignment = VerticalAlignment.Center, Stretch = Stretch.Fill };
            }
            else if (newType == ButtonType.Down)
            {
                control.Content = new Image { Source = new BitmapImage(new Uri("/Cosmos;component/Images/down.png", UriKind.Relative)), VerticalAlignment = VerticalAlignment.Center, Stretch = Stretch.Fill };
            }
            else if (newType == ButtonType.Size)
            {
                control.Content = new Image { Source = new BitmapImage(new Uri("/Cosmos;component/Images/size.png", UriKind.Relative)), VerticalAlignment = VerticalAlignment.Center, Stretch = Stretch.Fill };
            }
            else if (newType == ButtonType.None)
            {
                control.Content = null;
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            string backcolor = "";

            if (C_ButtonType == ButtonType.Close)
                backcolor = "#FFE81123";
            else if (C_ButtonType == ButtonType.Down || C_ButtonType == ButtonType.Size)
                backcolor = "#22FFFFFF";

            ColorAnimation colorChangeAnimation = new ColorAnimation
            {
                To = (Color)ColorConverter.ConvertFromString(backcolor),
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100))
            };

            PropertyPath colorTargetPath = new PropertyPath("(Background).(SolidColorBrush.Color)");
            Storyboard CellBackgroundChangeStory = new Storyboard();
            Storyboard.SetTarget(colorChangeAnimation, this);
            Storyboard.SetTargetProperty(colorChangeAnimation, colorTargetPath);
            CellBackgroundChangeStory.Children.Add(colorChangeAnimation);
            CellBackgroundChangeStory.Begin();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            string backcolor = "#00000000";

            ColorAnimation colorChangeAnimation = new ColorAnimation
            {
                To = (Color)ColorConverter.ConvertFromString(backcolor),
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100))
            };

            PropertyPath colorTargetPath = new PropertyPath("(Background).(SolidColorBrush.Color)");
            Storyboard CellBackgroundChangeStory = new Storyboard();
            Storyboard.SetTarget(colorChangeAnimation, this);
            Storyboard.SetTargetProperty(colorChangeAnimation, colorTargetPath);
            CellBackgroundChangeStory.Children.Add(colorChangeAnimation);
            CellBackgroundChangeStory.Begin();
        }

        public static Color ChangeColorBrightness(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            string backcolor = "";

            if (C_ButtonType == ButtonType.Close)
                backcolor = "#FFE81123";
            else if (C_ButtonType == ButtonType.Down || C_ButtonType == ButtonType.Size)
                backcolor = "#22FFFFFF";

            ColorAnimation colorChangeAnimation = new ColorAnimation
            {
                To = ChangeColorBrightness((Color)ColorConverter.ConvertFromString(backcolor), (float)-0.1),
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100))
            };

            PropertyPath colorTargetPath = new PropertyPath("(Background).(SolidColorBrush.Color)");
            Storyboard CellBackgroundChangeStory = new Storyboard();
            Storyboard.SetTarget(colorChangeAnimation, this);
            Storyboard.SetTargetProperty(colorChangeAnimation, colorTargetPath);
            CellBackgroundChangeStory.Children.Add(colorChangeAnimation);
            CellBackgroundChangeStory.Begin();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            ColorAnimation colorChangeAnimation = new ColorAnimation
            {
                To = ChangeColorBrightness((Color)ColorConverter.ConvertFromString("#00000000"), (float)0.15),
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100))
            };

            PropertyPath colorTargetPath = new PropertyPath("(Background).(SolidColorBrush.Color)");
            Storyboard CellBackgroundChangeStory = new Storyboard();
            Storyboard.SetTarget(colorChangeAnimation, this);
            Storyboard.SetTargetProperty(colorChangeAnimation, colorTargetPath);
            CellBackgroundChangeStory.Children.Add(colorChangeAnimation);
            CellBackgroundChangeStory.Begin();
        }

        protected override void OnClick()
        {
            base.OnClick();
        }
    }
}
