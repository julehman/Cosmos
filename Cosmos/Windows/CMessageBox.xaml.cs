using Cosmos.Classes;
using System;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Cosmos.Windows
{
    public partial class CMessageBox : Window
    {
        public enum CMessageBoxButton
        {
            OK,
            OK_Cancel,
            Yes_No,
            Yes_No_Cancel,
        }

        public enum CMessageBoxResult
        {
            None,
            OK,
            Yes,
            No,
            Cancel,
        }

        public CMessageBoxResult result = CMessageBoxResult.None;
        private CMessageBoxButton buttonType;

        public CMessageBox(string message, string caption, CColor.Theme theme, CImage.ImageType image, CMessageBoxButton _buttonType)
        {
            InitializeComponent();
            SystemSounds.Beep.Play();

            string imagePath = CImage.GetImagePath(image);

            ImageBrush imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Absolute));

            RCT_image.Fill = imgBrush;

            TBL_message.Text = message;
            L_caption.Content = caption;

            GRD_border.Background = CColor.GetColorBrush(theme);
            GRD_header.Background = CColor.GetColorBrush(theme);

            buttonType = _buttonType;

            switch (buttonType)
            {
                case CMessageBoxButton.OK:
                    B_3.Visibility = Visibility.Hidden;
                    B_2.Visibility = Visibility.Hidden;
                    B_1.Content = "OK";
                    break;
                case CMessageBoxButton.OK_Cancel:
                    B_3.Visibility = Visibility.Hidden;
                    B_2.Content = "OK";
                    B_1.Content = "Abbrechen";
                    break;
                case CMessageBoxButton.Yes_No:
                    B_3.Visibility = Visibility.Hidden;
                    B_2.Content = "Ja";
                    B_1.Content = "Nein";
                    break;
                case CMessageBoxButton.Yes_No_Cancel:
                    B_3.Content = "Ja";
                    B_2.Content = "Nein";
                    B_1.Content = "Abbrechen";
                    break;
                default:
                    break;
            }
        }


        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            
        }

        private void B_1_Click(object sender, RoutedEventArgs e)
        {
            switch (buttonType)
            {
                case CMessageBoxButton.OK:
                    result = CMessageBoxResult.OK;
                    break;
                case CMessageBoxButton.OK_Cancel:
                    result = CMessageBoxResult.Cancel;
                    break;
                case CMessageBoxButton.Yes_No:
                    result = CMessageBoxResult.No;
                    break;
                case CMessageBoxButton.Yes_No_Cancel:
                    result = CMessageBoxResult.Cancel;
                    break;
                default:
                    break;
            }
            Close();
        }

        private void B_2_Click(object sender, RoutedEventArgs e)
        {
            switch (buttonType)
            {
                case CMessageBoxButton.OK:
                    break;
                case CMessageBoxButton.OK_Cancel:
                    result = CMessageBoxResult.OK;
                    break;
                case CMessageBoxButton.Yes_No:
                    result = CMessageBoxResult.Yes;
                    break;
                case CMessageBoxButton.Yes_No_Cancel:
                    result = CMessageBoxResult.No;
                    break;
                default:
                    break;
            }
            Close();
        }

        private void B_3_Click(object sender, RoutedEventArgs e)
        {
            switch (buttonType)
            {
                case CMessageBoxButton.OK:
                    break;
                case CMessageBoxButton.OK_Cancel:
                    break;
                case CMessageBoxButton.Yes_No:
                    break;
                case CMessageBoxButton.Yes_No_Cancel:
                    result = CMessageBoxResult.Yes;
                    break;
                default:
                    break;
            }
            Close();
        }
    }
}
