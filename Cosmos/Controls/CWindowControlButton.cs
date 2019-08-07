using Cosmos.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Cosmos
{
    public class CWindowControlButton : Button
    {
        public enum ButtonType
        {
            none,
            close,
            maximize,
            minimize,
        }

        public static readonly DependencyProperty ButtonTypeProperty = DependencyProperty.Register("C_ButtonType", typeof(ButtonType), typeof(CWindowControlButton), new PropertyMetadata(new PropertyChangedCallback(ButtonTypeValueChanged)));

        static CWindowControlButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CWindowControlButton), new FrameworkPropertyMetadata(typeof(CWindowControlButton)));
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
            var control = (CWindowControlButton)d;

            ButtonType newType = (ButtonType)e.NewValue;

            if (newType == ButtonType.none)
                control.Content = null;
            else
                control.Content = new Image { Source = new BitmapImage(new Uri("/Cosmos;component/Images/Windowcontrol/" + newType.ToString() + ".png", UriKind.Relative)), VerticalAlignment = VerticalAlignment.Center, Stretch = Stretch.Fill };

        }
        protected override void OnClick()
        {
            base.OnClick();
            Window parentWindow = Window.GetWindow(this);


            switch (C_ButtonType)
            {
                case ButtonType.none:
                    break;
                case ButtonType.close:
                    parentWindow.Close();
                    break;
                case ButtonType.maximize:
                    this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                    if (parentWindow.WindowState == WindowState.Normal)
                        parentWindow.WindowState = WindowState.Maximized;
                    else
                        parentWindow.WindowState = WindowState.Normal;
                    break;
                case ButtonType.minimize:
                    parentWindow.WindowState = WindowState.Minimized;
                    break;
                default:
                    break;
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            string backcolor = "";

            if (C_ButtonType == ButtonType.close)
                backcolor = "#FFE81123";
            else if (C_ButtonType == ButtonType.minimize || C_ButtonType == ButtonType.maximize)
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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            string backcolor = "";

            if (C_ButtonType == ButtonType.close)
                backcolor = "#FFE81123";
            else if (C_ButtonType == ButtonType.minimize || C_ButtonType == ButtonType.maximize)
                backcolor = "#22FFFFFF";

            ColorAnimation colorChangeAnimation = new ColorAnimation
            {
                To = CColor.ChangeColorBrightness((Color)ColorConverter.ConvertFromString(backcolor), (float)-0.1),
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
                To = CColor.ChangeColorBrightness((Color)ColorConverter.ConvertFromString("#00000000"), (float)0.15),
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100))
            };

            PropertyPath colorTargetPath = new PropertyPath("(Background).(SolidColorBrush.Color)");
            Storyboard CellBackgroundChangeStory = new Storyboard();
            Storyboard.SetTarget(colorChangeAnimation, this);
            Storyboard.SetTargetProperty(colorChangeAnimation, colorTargetPath);
            CellBackgroundChangeStory.Children.Add(colorChangeAnimation);
            CellBackgroundChangeStory.Begin();
        }

    }
}
