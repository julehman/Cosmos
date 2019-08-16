using System.Windows;

namespace Cosmos
{
    public class CTextBoxBlock : CTextBox
    {
        /// <summary>
        /// a CTextBox made for long text and wrap with the return-key
        /// </summary>
        static CTextBoxBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CTextBoxBlock), new FrameworkPropertyMetadata(typeof(CTextBoxBlock)));
        }

    }

}
