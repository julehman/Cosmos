using System.Windows;
using System.Windows.Controls.Primitives;
using static Cosmos.Classes.CColor;

namespace Cosmos
{
    public class CToggle : ToggleButton
    {
        public static readonly DependencyProperty BackgroundThemeProperty = DependencyProperty.Register("C_BackgroundTheme", typeof(Theme), typeof(CToggle), new PropertyMetadata(new PropertyChangedCallback(BackgroundValueChanged)));
        public static readonly DependencyProperty CheckedThemeProperty = DependencyProperty.Register("C_CheckedTheme", typeof(Theme), typeof(CToggle), new PropertyMetadata(new PropertyChangedCallback(CheckedValueChanged)));

        static CToggle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CToggle), new FrameworkPropertyMetadata(typeof(CToggle)));
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

        public Theme C_CheckedTheme
        {
            get
            {
                return (Theme)GetValue(CheckedThemeProperty);
            }
            set
            {
                SetValue(CheckedThemeProperty, value);
            }
        }

        private static void BackgroundValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CToggle)d;

            control.Background = GetColorBrush((Theme)e.NewValue);
        }

        private static void CheckedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CToggle)d;

            control.Foreground = GetColorBrush((Theme)e.NewValue);
        }
    }
}
