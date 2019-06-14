using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cosmos
{
    /// <summary>
    /// Interaction logic for CPickerWindow.xaml
    /// </summary>
    public partial class CPickerWindow : Window
    {

        public System.Drawing.Color Color
        {
            get
            {
                Color colX = CP_main.GetColor().Color;
                System.Drawing.Color col = System.Drawing.Color.FromArgb(colX.A, colX.R, colX.G, colX.B);
                return col;
            }
            set { CP_main.SetColor(value.A, value.R, value.G, value.B); }
        }


        public bool UseAlphaValues
        {
            get { return CP_main.UseAlphaValues; }
            set
            {
                CP_main.UseAlphaValues = value;
            }
        }

        public CPickerWindow()
        {
            InitializeComponent();
        }

        public CPickerWindow(byte r, byte g, byte b):this()
        {
            CP_main.SetColor(r, g, b);
        }

        private void B_select_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void B_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
