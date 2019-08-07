using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static Cosmos.Classes.CColor;

namespace Cosmos
{
    public class CInfoButton : Button
    {
        public static readonly DependencyProperty BackgroundThemeProperty = DependencyProperty.Register("C_BackgroundTheme", typeof(Theme), typeof(CInfoButton), new PropertyMetadata(new PropertyChangedCallback(BackgroundValueChanged)));

        static CInfoButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CInfoButton), new FrameworkPropertyMetadata(typeof(CInfoButton)));
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
        

        private static void BackgroundValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CInfoButton)d;
            control.Background = (SolidColorBrush)(GetColorBrush((Theme)e.NewValue));
        }
    }
}
