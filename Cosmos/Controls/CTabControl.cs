using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Cosmos
{
    public class CTabControl : TabControl
    {
        static CTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CTabControl), new FrameworkPropertyMetadata(typeof(CTabControl)));
        }
    }
}
