using Cosmos.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static Cosmos.Classes.CColor;

namespace Cosmos
{
    public class CInfoButton : Button
    {
        public static readonly DependencyProperty BackgroundThemeProperty = DependencyProperty.Register("C_BackgroundTheme", typeof(Theme), typeof(CInfoButton), new PropertyMetadata(new PropertyChangedCallback(BackgroundValueChanged)));

        /// <summary>
        /// a button with a info icon and info-text on hover and click, write the info text in the tooltip-property
        /// </summary>
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

        protected override void OnClick()
        {
            CMessageBox box = new CMessageBox("Information", this.ToolTip.ToString(), Theme.DarkGrey, Classes.CImage.ImageType.info_black, CMessageBox.CMessageBoxButton.OK);
            box.ShowDialog();

            base.OnClick();
        }
    }
}
