using System;
using System.Windows;
using System.Windows.Controls;
using static Cosmos.CColor;

namespace Cosmos
{
    public class CCheckBox : CheckBox
    {
        public static readonly DependencyProperty FrameThemeProperty = DependencyProperty.Register("C_FrameTheme", typeof(Theme), typeof(CCheckBox), new PropertyMetadata(new PropertyChangedCallback(FrameValueChanged)));
        public static readonly DependencyProperty CheckedThemeProperty = DependencyProperty.Register("C_CheckedTheme", typeof(Theme), typeof(CCheckBox), new PropertyMetadata(new PropertyChangedCallback(CheckedValueChanged)));
        public static readonly DependencyProperty ForegroundThemeProperty = DependencyProperty.Register("C_ForegroundTheme", typeof(Theme), typeof(CCheckBox), new PropertyMetadata(new PropertyChangedCallback(ForegroundValueChanged)));
        
        static CCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CCheckBox), new FrameworkPropertyMetadata(typeof(CCheckBox)));
        }

        public Theme C_FrameTheme
        {
            get
            {
                return (Theme)GetValue(FrameThemeProperty);
            }
            set
            {
                SetValue(FrameThemeProperty, value);
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

        private static void FrameValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CCheckBox)d;

            control.BorderBrush = GetColorBrush((Theme)e.NewValue);
        }

        private static void CheckedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CCheckBox)d;

            control.OpacityMask = GetColorBrush((Theme)e.NewValue);
        }

        private static void ForegroundValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CCheckBox)d;

            control.Foreground = GetColorBrush((Theme)e.NewValue);
        }
    }
}
