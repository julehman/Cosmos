using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Cosmos.Classes.CColor;

namespace Cosmos
{
    public partial class CWindowHeader : UserControl
    {
        public enum HeaderType
        {
            Close,
            Close_Minimize,
            Close_Minimize_Maximize,
        }


        public static readonly DependencyProperty BackgroundThemeProperty = DependencyProperty.Register("C_BackgroundTheme", typeof(Theme), typeof(CWindowHeader), new PropertyMetadata(new PropertyChangedCallback(BackgroundValueChanged)));
        public static readonly DependencyProperty ForegroundThemeProperty = DependencyProperty.Register("C_ForegroundTheme", typeof(Theme), typeof(CWindowHeader), new PropertyMetadata(new PropertyChangedCallback(ForegroundValueChanged)));
        public static readonly DependencyProperty HeaderTypeProperty = DependencyProperty.Register("C_HeaderType", typeof(HeaderType), typeof(CWindowHeader), new PropertyMetadata(new PropertyChangedCallback(HeaderTypeValueChanged)));
        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("C_HeaderText", typeof(string), typeof(CWindowHeader), new PropertyMetadata(new PropertyChangedCallback(HeaderTextValueChanged)));


        public CWindowHeader()
        {
            InitializeComponent();
        }

        public string C_HeaderText
        {
            get
            {
                return (string)GetValue(HeaderTextProperty);
            }
            set
            {
                SetValue(HeaderTextProperty, value);
            }
        }

        public HeaderType C_HeaderType
        {
            get
            {
                return (HeaderType)GetValue(HeaderTypeProperty);
            }
            set
            {
                SetValue(HeaderTypeProperty, value);
            }
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


        private static void HeaderTextValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CWindowHeader)d;
            control.L_caption.Content = (string)e.NewValue;
        }

        private static void HeaderTypeValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CWindowHeader)d;
            HeaderType newval = (HeaderType)e.NewValue;

            switch (newval)
            {
                case HeaderType.Close:
                    control.B_1.C_ButtonType = CWindowControlButton.ButtonType.close;
                    control.B_1.Visibility = Visibility.Visible;
                    control.B_2.Visibility = Visibility.Hidden;
                    control.B_3.Visibility = Visibility.Hidden;
                    break;
                case HeaderType.Close_Minimize:
                    control.B_1.C_ButtonType = CWindowControlButton.ButtonType.close;
                    control.B_2.C_ButtonType = CWindowControlButton.ButtonType.minimize;
                    control.B_1.Visibility = Visibility.Visible;
                    control.B_2.Visibility = Visibility.Visible;
                    control.B_3.Visibility = Visibility.Hidden;
                    break;
                case HeaderType.Close_Minimize_Maximize:
                    control.B_1.C_ButtonType = CWindowControlButton.ButtonType.close;
                    control.B_2.C_ButtonType = CWindowControlButton.ButtonType.maximize;
                    control.B_3.C_ButtonType = CWindowControlButton.ButtonType.minimize;
                    control.B_1.Visibility = Visibility.Visible;
                    control.B_2.Visibility = Visibility.Visible;
                    control.B_3.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }

        }

        private static void BackgroundValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CWindowHeader)d;
            control.GRD_header.Background = GetColorBrush((Theme)e.NewValue);
        }

        private static void ForegroundValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CWindowHeader)d;
            control.L_caption.Foreground = GetColorBrush((Theme)e.NewValue);
        }

        private void GRD_header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window.GetWindow(this).DragMove();

        }
    }
}
