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
    public class CIconTextHorizontalButton : Button
    {
        public static readonly DependencyProperty ButtonTypeProperty = DependencyProperty.Register("C_ButtonType", typeof(CImage.ImageType), typeof(CIconTextHorizontalButton), new PropertyMetadata(new PropertyChangedCallback(ButtonTypeValueChanged)));
        public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register("C_ButtonText", typeof(string), typeof(CIconTextHorizontalButton), new PropertyMetadata(new PropertyChangedCallback(ButtonTextValueChanged)));
        public static new readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(CIconTextHorizontalButton), new PropertyMetadata(true, OnIsEnabledChanged));


        static CIconTextHorizontalButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CIconTextHorizontalButton), new FrameworkPropertyMetadata(typeof(CIconTextHorizontalButton)));
        }

        public CImage.ImageType C_ButtonType
        {
            get
            {
                return (CImage.ImageType)GetValue(ButtonTypeProperty);
            }
            set
            {
                SetValue(ButtonTypeProperty, value);
            }
        }

        public string C_ButtonText
        {
            get
            {
                return (string)GetValue(ButtonTextProperty);
            }
            set
            {
                SetValue(ButtonTextProperty, value);
            }
        }
        public new bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        public static void OnIsEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var control = sender as CIconTextHorizontalButton;
            if (control != null)
            {
                control.UpdateIsEnabled();
            }
            if (!(args.NewValue as bool?).Value)
                (sender as CIconTextHorizontalButton).Background = CColor.GetColorBrush(CColor.Theme.Disabled);
        }

        public void UpdateIsEnabled()
        {
            var content = Content as Control;
            if (content != null)
            {
                content.IsEnabled = IsEnabled;
            }
        }

        private static void ButtonTextValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CIconTextHorizontalButton)d;

            string newText = (string)e.NewValue;

            control.ToolTip = newText;
        }

        private static void ButtonTypeValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CIconTextHorizontalButton)d;

            CImage.ImageType newType = (CImage.ImageType)e.NewValue;

            string imagePath = CImage.GetImagePath(newType);

            Image imgBrush = new Image();
            imgBrush.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));

            control.Content = imgBrush;

        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (!this.IsEnabled)
                return;

            string backcolor = "#44000000";

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
            if (!this.IsEnabled)
                return;

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
            if (!this.IsEnabled)
                return;

            base.OnMouseLeftButtonDown(e);

            string backcolor = "#55000000";

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
            if (!this.IsEnabled)
                return;

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
