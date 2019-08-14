using System.Windows;

namespace Cosmos
{
    public class CTextBoxBlock : CTextBox
    {
        static CTextBoxBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CTextBoxBlock), new FrameworkPropertyMetadata(typeof(CTextBoxBlock)));
        }

    }

}
