using System.Windows;
using System.Windows.Controls;
using static Cosmos.Classes.CColor;

namespace Cosmos
{
    public class CTextBlock : TextBlock
    {
        public static readonly DependencyProperty ForegroundThemeProperty = DependencyProperty.Register("C_ForegroundTheme", typeof(Theme), typeof(CTextBlock), new PropertyMetadata(new PropertyChangedCallback(ForegroundThemeValueChanged)));

        /// <summary>
        /// customized textblock in style, no extra features
        /// </summary>
        static CTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CTextBlock), new FrameworkPropertyMetadata(typeof(CTextBlock)));
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

        private static void ForegroundThemeValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CTextBlock)d;

            control.Foreground = GetColorBrush((Theme)e.NewValue);
        }
    }
}
