using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static Cosmos.Classes.CColor;

namespace Cosmos
{
    /// <summary>
    /// customized button in style, no extra features
    /// </summary>
    public class CButton : Button
    {
        public static readonly DependencyProperty BackgroundThemeProperty = DependencyProperty.Register("C_BackgroundTheme", typeof(Theme), typeof(CButton), new PropertyMetadata(new PropertyChangedCallback(BackgroundValueChanged)));
        public static readonly DependencyProperty ForegroundThemeProperty = DependencyProperty.Register("C_ForegroundTheme", typeof(Theme), typeof(CButton), new PropertyMetadata(new PropertyChangedCallback(ForegroundValueChanged)));

        public static bool Backgroundchanged = false;

        private static string backcolor;
        private static string Backcolor
        {
            get { return backcolor; }
            set
            {
                backcolor = value;
                Backgroundchanged = true;
            }
        }

        static CButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CButton), new FrameworkPropertyMetadata(typeof(CButton)));
        }

        public Theme C_BackgroundTheme
        {
            get
            {
                return (Theme)GetValue(BackgroundThemeProperty);
            }
            set
            {
                SetValue(BackgroundThemeProperty, value);
            }
        }

        public Theme C_ForegroundTheme
        {
            get
            {
                return (Theme)GetValue(ForegroundThemeProperty);
            }
            set
            {
                SetValue(ForegroundThemeProperty, value);
            }
        }

        private static void BackgroundValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CButton)d;

            Backcolor = GetColorString((Theme)e.NewValue);

            control.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(Backcolor));
            control.OpacityMask = (SolidColorBrush)(new BrushConverter().ConvertFrom(ChangeColorBrightness((Color)ColorConverter.ConvertFromString(Backcolor), (float)-0.3).ToString()));
        }

        private static void ForegroundValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CButton)d;
            control.Foreground = GetColorBrush((Theme)e.NewValue);
        }

        //temporarily commented because there is an issue with the change of the background color when button calls a window.. very strange

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            //Backcolor = this.Background.ToString();

            //ColorAnimation colorChangeAnimation = new ColorAnimation
            //{
            //    To = ChangeColorBrightness((Color)ColorConverter.ConvertFromString(Backcolor), (float)0.15),
            //    Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100))
            //};

            //PropertyPath colorTargetPath = new PropertyPath("(Background).(SolidColorBrush.Color)");
            //Storyboard CellBackgroundChangeStory = new Storyboard();
            //Storyboard.SetTarget(colorChangeAnimation, this);
            //Storyboard.SetTargetProperty(colorChangeAnimation, colorTargetPath);
            //CellBackgroundChangeStory.Children.Add(colorChangeAnimation);
            //CellBackgroundChangeStory.Begin();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            //ColorAnimation colorChangeAnimation = new ColorAnimation
            //{
            //    To = (Color)ColorConverter.ConvertFromString(Backcolor),
            //    Duration = new Duration(new TimeSpan(0, 0, 0, 0, 100))
            //};

            //PropertyPath colorTargetPath = new PropertyPath("(Background).(SolidColorBrush.Color)");
            //Storyboard CellBackgroundChangeStory = new Storyboard();
            //Storyboard.SetTarget(colorChangeAnimation, this);
            //Storyboard.SetTargetProperty(colorChangeAnimation, colorTargetPath);
            //CellBackgroundChangeStory.Children.Add(colorChangeAnimation);
            //CellBackgroundChangeStory.Begin();
        }

        protected override void OnClick()
        {
            base.OnClick();
        }
    }
}
