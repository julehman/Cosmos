using System.Windows.Media;

namespace Cosmos
{
    static public class CColor
    {
        public enum Theme
        {
            Default,
            
            Teal,
            Red,
            Blue,
            HKS41,
            Yellow,
            Green,

            Black,
            White,
            Gray,
        }

        public static Brush GetColorBrush(Theme theme)
        {
            return (SolidColorBrush)(new BrushConverter().ConvertFrom(GetColorString(theme)));
        }

        public static string GetColorString(Theme theme)
        {
            switch (theme)
            {
                case Theme.Default:
                    return "#FF111111";
                    
                case Theme.Teal:
                    return "#FF2c736b";
                case Theme.Red:
                    return "#FFCB2030";
                case Theme.Blue:
                    return "#FF007ACC";
                case Theme.HKS41:
                    return "#FF00305d";
                case Theme.Yellow:
                    return "#FFE7AD00";
                case Theme.Green:
                    return "#FF11A244";

                case Theme.Black:
                    return "#FF111111";
                case Theme.White:
                    return "#FFEEEEEE";
                case Theme.Gray:
                    return "#FF999999";
                default:
                    return string.Empty;
            }
        }
    }
}
