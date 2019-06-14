using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Cosmos.CColor;

namespace Cosmos
{
    public class CComboBox : ComboBox
    {
        public static readonly DependencyProperty ForegroundThemeProperty = DependencyProperty.Register("C_ForegroundTheme", typeof(Theme), typeof(CComboBox), new PropertyMetadata(new PropertyChangedCallback(ForegroundThemeValueChanged)));

        static CComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CComboBox), new FrameworkPropertyMetadata(typeof(CComboBox)));
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
            var control = (CComboBox)d;

            control.Foreground = GetColorBrush((Theme)e.NewValue);
        }
    }
}
