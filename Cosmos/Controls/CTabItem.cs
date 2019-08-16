using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Cosmos
{
    public class CTabItem : TabItem
    {
        /// <summary>
        /// flat style tabitem without borders
        /// </summary>
        static CTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CTabItem), new FrameworkPropertyMetadata(typeof(CTabItem)));
        }
    }
}
