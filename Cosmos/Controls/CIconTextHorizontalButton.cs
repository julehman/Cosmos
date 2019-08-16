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

        /// <summary>
        /// a button consisting of an icon and text aligned beside each other
        /// </summary>
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
            if (sender is CIconTextHorizontalButton control)
                control.UpdateIsEnabled();

            if (!(args.NewValue as bool?).Value)
                (sender as CIconTextHorizontalButton).Background = CColor.GetColorBrush(CColor.Theme.Disabled);
        }

        public void UpdateIsEnabled()
        {
            if (Content is Control content)
                content.IsEnabled = IsEnabled;
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

            Image imgBrush = new Image
            {
                Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute))
            };

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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!this.IsEnabled)
                return;

            base.OnMouseLeftButtonDown(e);

            string backcolor = "#55000000";

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
            if (!this.IsEnabled)
                return;

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

        protected override void OnClick()
        {
            base.OnClick();
        }
    }
}
