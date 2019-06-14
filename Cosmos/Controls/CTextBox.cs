using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static Cosmos.CColor;

namespace Cosmos
{
    public class CTextBox : TextBox
    {
        public static readonly DependencyProperty BorderThemeProperty = DependencyProperty.Register("C_BorderTheme", typeof(Theme), typeof(CTextBox), new PropertyMetadata(new PropertyChangedCallback(C_BorderThemeValueChanged)));
        public static readonly DependencyProperty ForegroundThemeProperty = DependencyProperty.Register("C_ForegroundTheme", typeof(Theme), typeof(CTextBox), new PropertyMetadata(new PropertyChangedCallback(C_ForegroundThemeValueChanged)));

        public static readonly DependencyProperty NumericProperty = DependencyProperty.Register("C_Numeric", typeof(bool), typeof(CTextBox), new PropertyMetadata(new PropertyChangedCallback(NumericValueChanged)));
        public static readonly DependencyProperty NumericIntegerProperty = DependencyProperty.Register("C_NumericInteger", typeof(bool), typeof(CTextBox), new PropertyMetadata(new PropertyChangedCallback(NumericIntegerValueChanged)));
        public static readonly DependencyProperty NumericDoubleDotProperty = DependencyProperty.Register("C_NumericDoubleDot", typeof(bool), typeof(CTextBox), new PropertyMetadata(new PropertyChangedCallback(NumericDoubleDotValueChanged)));

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("C_Watermark", typeof(string), typeof(CTextBox), new PropertyMetadata(new PropertyChangedCallback(WatermarkValueChanged)));
        public static readonly DependencyProperty SelectOnFocusProperty = DependencyProperty.Register("C_SelectOnFocus", typeof(bool), typeof(CTextBox), new PropertyMetadata(new PropertyChangedCallback(SelectFocusValueChanged)));

        public bool isEmpty = true;
        public bool gotfocus = false;
        public bool settingwatermark = false;

        public double lastValidNum = 0;
        public bool lastNumZero = false;
        
        public CultureInfo culture = new CultureInfo("de");

        //public new string Text
        //{
        //    set
        //    {
        //        base.Text = value;
        //    }
        //    get
        //    {
        //        if (isEmpty && (C_Watermark != "" || C_Watermark != string.Empty || C_Watermark != null))
        //            return string.Empty;
        //        else
        //            return base.Text;
        //    }
        //}

        static CTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CTextBox), new FrameworkPropertyMetadata(typeof(CTextBox)));
        }

        public bool C_SelectOnFocus
        {
            get
            {
                return (bool)GetValue(SelectOnFocusProperty);
            }
            set
            {
                SetValue(SelectOnFocusProperty, value);
            }
        }

        public bool C_Numeric
        {
            get
            {
                return (bool)GetValue(NumericProperty);
            }
            set
            {
                SetValue(NumericProperty, value);
            }
        }

        public bool C_NumericInteger
        {
            get
            {
                return (bool)GetValue(NumericIntegerProperty);
            }
            set
            {
                SetValue(NumericIntegerProperty, value);
            }
        }

        public bool C_NumericDoubleDot
        {
            get
            {
                return (bool)GetValue(NumericDoubleDotProperty);
            }
            set
            {
                SetValue(NumericDoubleDotProperty, value);
            }
        }

        public Theme C_BorderTheme
        {
            get
            {
                return (Theme)GetValue(BorderThemeProperty);
            }
            set
            {
                SetValue(BorderThemeProperty, value);
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

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ChangeWatermarkVisibility();
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            gotfocus = true;
            if (isEmpty)
            {
                this.Text = string.Empty;
                this.Foreground = GetColorBrush(C_ForegroundTheme);
            }
            if (C_SelectOnFocus)
            {
                this.SelectAll();
            }
        }

        protected override void OnGotMouseCapture(MouseEventArgs e)
        {
            base.OnGotMouseCapture(e);
            if (C_SelectOnFocus)
            {
                this.SelectAll();
            }
        }

        private void CheckNumeric()
        {
            if (C_NumericDoubleDot)
                culture = new CultureInfo("en");
            else
                culture = new CultureInfo("de");

            //check if numeric when property is true
            try
            {
                if (C_Numeric && !C_NumericInteger)
                {
                    if (this.Text == "")
                    {
                        isEmpty = true;
                        throw new Exception();
                    }
                    
                    double test_double = double.Parse(this.Text, culture);

                    lastValidNum = test_double;
                    isEmpty = false;

                    if (lastValidNum == 0)
                        lastNumZero = true;

                    this.BorderBrush = GetColorBrush(C_BorderTheme);
                }
                else if (C_Numeric && C_NumericInteger)
                {
                    if (this.Text == "")
                    {
                        isEmpty = true;
                        throw new Exception();
                    }

                    double test_int = Convert.ToInt32(this.Text);
                    double test_double = Convert.ToDouble(this.Text);

                    if (test_int != test_double)
                        throw new Exception();

                    lastValidNum = test_int;
                    isEmpty = false;

                    if (lastValidNum == 0)
                        lastNumZero = true;

                    this.BorderBrush = GetColorBrush(C_BorderTheme);
                }
            }
            catch
            {
                this.BorderBrush = GetColorBrush(Theme.Red);
            }
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);

            CheckNumeric();
            
            if (C_Numeric && !isEmpty)
            {
                if (lastValidNum != 0 || lastNumZero)
                {
                    this.Text = lastValidNum.ToString(culture);
                    this.BorderBrush = GetColorBrush(C_BorderTheme);
                }
                else
                {
                    this.Text = "";
                    this.BorderBrush = GetColorBrush(C_BorderTheme);
                }
            }

            gotfocus = false;
            ChangeWatermarkVisibility();
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (!gotfocus && !settingwatermark)
            {
                if (this.Text == string.Empty)
                {
                    ChangeWatermarkVisibility();
                }
                else
                {
                    isEmpty = false;
                    this.Foreground = GetColorBrush(C_ForegroundTheme);
                }
            }
            else if (gotfocus && !settingwatermark)
            {
                if (this.Text != string.Empty)
                {
                    CheckNumeric();
                }
                else
                {
                    isEmpty = false;
                }
            }
        }

        private void ChangeWatermarkVisibility()
        {
            if (this.Text == string.Empty)
            {
                isEmpty = true;
                settingwatermark = true;
                this.Text = C_Watermark;
                settingwatermark = false;

                this.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ff848484"));
                this.BorderBrush = GetColorBrush(C_BorderTheme);
            }
            else
            {
                isEmpty = false;
            }
        }

        public string C_Watermark
        {
            get
            {
                return (string)GetValue(WatermarkProperty);
            }
            set
            {
                SetValue(WatermarkProperty, value);
            }
        }

        private static void NumericDoubleDotValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Not used YET(!)
        }

        private static void WatermarkValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Not used YET(!)
        }

        private static void SelectFocusValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Not used YET(!)
        }

        private static void NumericValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Not used YET(!)
        }

        private static void NumericIntegerValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Not used YET(!)
        }

        private static void C_BorderThemeValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CTextBox)d;

            control.BorderBrush = GetColorBrush((Theme)e.NewValue);
            control.SelectionBrush = GetColorBrush((Theme)e.NewValue);
        }

        private static void C_ForegroundThemeValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CTextBox)d;

            control.Foreground = GetColorBrush((Theme)e.NewValue);
            control.CaretBrush = GetColorBrush((Theme)e.NewValue);
        }
    }
}
