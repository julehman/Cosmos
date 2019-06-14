using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Cosmos
{
    public partial class CPicker : UserControl
    {
        SolidColorBrush superColor;

        SolidColorBrush currentColor;
        SolidColorBrush CurrentColor {
            get { return currentColor; }
            set {
                if (currentColor == value)
                    return;
                currentColor = value;
                C_SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        bool useAlphaValues = true;
        public bool UseAlphaValues {
            get { return useAlphaValues; }
            set {
                useAlphaValues = value;
                if (useAlphaValues)
                {
                    L_a.Visibility = Visibility.Visible;
                    TB_a.Visibility = Visibility.Visible;
                }
                else
                {
                    L_a.Visibility = Visibility.Hidden;
                    TB_a.Visibility = Visibility.Hidden;
                }
            }
        }

        public event EventHandler C_SelectionChanged = new EventHandler(CP_SelectionChanged);

        private static void CP_SelectionChanged(object sender, EventArgs e)
        {
        }

        bool leftmouse = false;
        bool superleftmouse = false;
        bool argbSetting = false;

        public CPicker()
        {
            InitializeComponent();
            superColor = new SolidColorBrush();
        }
        
        #region Public Functions (Get/Set)
        public SolidColorBrush GetColor()
        {
            return CurrentColor;
        }

        public SolidColorBrush GetColorWOTransparency()
        {
            return new SolidColorBrush(new Color() { R = CurrentColor.Color.R, G = CurrentColor.Color.G, B = CurrentColor.Color.B, A = 255 });
        }

        public byte GetTransparency()
        {
            return CurrentColor.Color.A;
        }

        public bool SetColor(SolidColorBrush col)
        {
            if (SetColor(col.Color.A) && SetColor(col.Color.R, col.Color.G, col.Color.B))
                return true;
            else
                return false;
        }
        
        public bool SetColor(byte a)
        {
            if (a < 0 || a > 255)
                return false;


            //if first time set rgb then a is 255
            if (currentColor == null)
                currentColor = new SolidColorBrush() { Color = new Color() { R = 0, G = 0, B = 0, A = 255 } };

            //set new currentColor
            CurrentColor = new SolidColorBrush()
            {
                Color = new Color()
                {
                    A = Convert.ToByte(a),
                    R = CurrentColor.Color.R,
                    G = CurrentColor.Color.G,
                    B = CurrentColor.Color.B,
                }
            };

            RCT_color.Fill = CurrentColor;
            E_cursor.Fill = CurrentColor;

            UpdateRGBText();
            return true;
        }

        public bool SetColor(byte r, byte g, byte b)
        {
            if (r < 0 || r > 255 ||
                g < 0 || g > 255 ||
                b < 0 || b > 255)
                return false;

            //set new superColor
            if (!leftmouse && !superleftmouse)
                superColor = GetSupercolor(r, g, b);

            
            //if first time set rgb then a is 255
            if (currentColor == null)
                currentColor = new SolidColorBrush() { Color = new Color() { R = 0, G = 0, B = 0, A = 255 } };

            //set new currentColor
            CurrentColor = new SolidColorBrush(new Color() { A = CurrentColor.Color.A, R = Convert.ToByte(r), G = Convert.ToByte(g), B = Convert.ToByte(b) });
            RCT_color.Fill = CurrentColor;
            E_cursor.Fill = CurrentColor;


            if (!leftmouse && !superleftmouse)
            {
                //update ui element position and gradient
                Point pos = GetRelativeGridPosition(superColor, new SolidColorBrush(new Color() { R = Convert.ToByte(r), G = Convert.ToByte(g), B = Convert.ToByte(b) }));
                E_cursor.Margin = new Thickness((RCT_gradient.Width * (1 - pos.X)) + RCT_gradient.Margin.Left - (E_cursor.Width / 2), (RCT_gradient.Height * (1 - pos.Y)) + RCT_gradient.Margin.Top - (E_cursor.Height / 2), 0, 0);

                SetGradient(superColor.Color);
                SetSuperCursor(superColor);
            }
            UpdateRGBText();

            return true;
        }

        public bool SetColor(byte a, byte r, byte g, byte b)
        {
            if (SetColor(a) && SetColor(r, g, b))
                return true;
            else
                return false;
        }
        #endregion

        #region Subcolor Cursor Control
        private void SubMouseMove()
        {
            if (leftmouse)
            {
                Window parentWindow = Window.GetWindow(this);
                Point RCT1position = RCT_gradient.PointToScreen(new Point(0d, 0d));
                Point Eposition = E_cursor.PointToScreen(new Point(0d, 0d));

                Point mouse = new Point
                {
                    X = MousePosition().X - (RCT1position.X - parentWindow.Left - 8),
                    Y = MousePosition().Y - (RCT1position.Y - parentWindow.Top - 31)
                };

                Point calcMouse = new Point
                {
                    X = mouse.X / (RCT_gradient.Width / 255) / 255,
                    Y = mouse.Y / (RCT_gradient.Height / 255) / 255
                };

                E_cursor.Margin = new Thickness(mouse.X - (Eposition.X - E_cursor.Margin.Left - parentWindow.Left) + (RCT1position.X - parentWindow.Left) - (E_cursor.Width / 2), mouse.Y - (Eposition.Y - E_cursor.Margin.Top - parentWindow.Top) + (RCT1position.Y - parentWindow.Top) - (E_cursor.Height / 2), 0, 0);

                //if mouse is in bounds.. should be but if it glitches, it doesn't crash
                if ((mouse.X <= RCT_gradient.Width && mouse.X >= 0) && (mouse.Y >= 0 && mouse.Y <= RCT_gradient.Height))
                {
                    SolidColorBrush newColor = CalculateColor(calcMouse, superColor);
                    SetColor(newColor.Color.R, newColor.Color.G, newColor.Color.B);
                }


                




            }
        }

        private void SubMouseLeftButtonDown()
        {
            Window parentWindow = Window.GetWindow(this);
            Point position = RCT_gradient.PointToScreen(new Point(0d, 0d));

            System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle(new System.Drawing.Point()
            {
                X = Convert.ToInt32(position.X),
                Y = Convert.ToInt32(position.Y)
            },
            new System.Drawing.Size(Convert.ToInt32(RCT_gradient.Width + 1), Convert.ToInt32(RCT_gradient.Height + 1)));

            leftmouse = true;
            SubMouseMove();
        }

        private void SubMouseLeftButtonUp()
        {
            System.Windows.Forms.Cursor.Clip = System.Drawing.Rectangle.Empty;
            leftmouse = false;
        }

        private void RCT_gradient_MouseMove(object sender, MouseEventArgs e)
        {
            SubMouseMove();
        }

        private void RCT_gradient_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SubMouseLeftButtonDown();
        }

        private void RCT_gradient_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SubMouseLeftButtonUp();
        }

        private void E_cursor_MouseMove(object sender, MouseEventArgs e)
        {
            SubMouseMove();
        }

        private void E_cursor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SubMouseLeftButtonDown();
        }

        private void E_cursor_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SubMouseLeftButtonUp();
        }
        #endregion

        #region Supercolor Cursor Control
        private void SuperMouseLeftButtonDown()
        {
            Window parentWindow = Window.GetWindow(this);
            Point position = RCT_supergradient.PointToScreen(new Point(0d, 0d));

            System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle(new System.Drawing.Point()
            {
                X = Convert.ToInt32(position.X + 1),
                Y = Convert.ToInt32(position.Y + (RCT_supergradient.Height / 2))
            },
            new System.Drawing.Size(Convert.ToInt32(RCT_supergradient.Width + 2), 1));

            superleftmouse = true;
            SuperMouseMove();
        }

        private void SuperMouseLeftButtonUp()
        {
            System.Windows.Forms.Cursor.Clip = System.Drawing.Rectangle.Empty;
            superleftmouse = false;
        }

        private void SuperMouseMove()
        {
            if (superleftmouse)
            {
                Point position = RCT_supergradient.PointToScreen(new Point(0d, 0d));
                Point Eposition = E_supercursor.PointToScreen(new Point(0d, 0d));

                Window parentWindow = Window.GetWindow(this);
                superColor.Color = GetRelativeColor((RCT_supergradient.Fill as LinearGradientBrush).GradientStops, GetGradientOffset(position.X - parentWindow.Left - 7, RCT_supergradient.Width + 1));
                SetGradient(superColor.Color);

                Point mouse = new Point
                {
                    X = MousePosition().X - (position.X - parentWindow.Left - 8),
                    Y = MousePosition().Y - (position.Y - parentWindow.Top - 31)
                };

                E_supercursor.Margin = new Thickness(mouse.X - (Eposition.X - E_supercursor.Margin.Left - parentWindow.Left) + (position.X - parentWindow.Left) - (E_supercursor.Width / 2), mouse.Y - (Eposition.Y - E_supercursor.Margin.Top - parentWindow.Top) + (position.Y - parentWindow.Top) - (E_supercursor.Height / 2), 0, 0);
                E_supercursor.Fill = superColor;

                Point calcPos = new Point
                {
                    X = (E_cursor.Margin.Left - RCT_gradient.Margin.Left + (E_cursor.Width / 2)) / (RCT_gradient.Width / 255) / 255,
                    Y = (E_cursor.Margin.Top - RCT_gradient.Margin.Top + (E_cursor.Height / 2)) / (RCT_gradient.Height / 255) / 255
                };

                SolidColorBrush newColor = CalculateColor(calcPos, superColor);
                SetColor(newColor.Color.R, newColor.Color.G, newColor.Color.B);

                //new System.Drawing.Size(Convert.ToInt32(RCT_gradient.Width + 1), Convert.ToInt32(RCT_gradient.Height + 1));
            }
        }

        private void RCT_supergradient_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SuperMouseLeftButtonDown();
        }

        private void RCT_supergradient_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SuperMouseLeftButtonUp();
        }

        private void RCT_supergradient_MouseMove(object sender, MouseEventArgs e)
        {
            SuperMouseMove();
        }

        private void E_supercursor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SuperMouseLeftButtonDown();
        }

        private void E_supercursor_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SuperMouseLeftButtonUp();
        }

        private void E_supercursor_MouseMove(object sender, MouseEventArgs e)
        {
            SuperMouseMove();
        }

        private void SetSuperCursor(SolidColorBrush supercolor)
        {
            double offset = 0;

            if (supercolor.Color.R == 255)
            {
                if (supercolor.Color.G == 0)
                {
                    offset = 1 - ((supercolor.Color.B * 0.167) / 255);
                }
                else if (supercolor.Color.B == 0)
                {
                    offset = 0 + ((supercolor.Color.G * 0.167) / 255);
                }
            }
            else if (supercolor.Color.G == 255)
            {
                if (supercolor.Color.R == 0)
                {
                    offset = 0.333 + ((supercolor.Color.B * 0.167) / 255);
                }
                else if (supercolor.Color.B == 0)
                {
                    offset = 0.333 - ((supercolor.Color.R * 0.167) / 255);
                }
            }
            else if (supercolor.Color.B == 255)
            {
                if (supercolor.Color.R == 0)
                {
                    offset = 0.667 - ((supercolor.Color.G * 0.167) / 255);
                }
                else if (supercolor.Color.G == 0)
                {
                    offset = 0.667 + ((supercolor.Color.R * 0.167) / 255);
                }
            }

            E_supercursor.Margin = new Thickness(RCT_supergradient.Width * offset - (E_supercursor.Width / 2) + RCT_supergradient.Margin.Left, E_supercursor.Margin.Top,
                                                 E_supercursor.Margin.Right, E_supercursor.Margin.Bottom);
            E_supercursor.Fill = supercolor;
        }
        #endregion

        #region Color/Gradient Calculations
        private SolidColorBrush CalculateColor(System.Windows.Point pos, SolidColorBrush supercolor)
        {
            pos.X = 1 - Math.Round(pos.X, 2);
            pos.Y = 1 - Math.Round(pos.Y, 2);

            int sr = supercolor.Color.R;
            int sg = supercolor.Color.G;
            int sb = supercolor.Color.B;

            double wr = 0;
            double wg = 0;
            double wb = 0;

            int grau = Convert.ToInt32(pos.Y * 255);

            double gr;
            double gg;
            double gb;

            //Weiss + Farbe
            if (sr == 255)
            {
                wr = 255;
                if (sg == 0)
                {
                    wg = Convert.ToInt32(255 * pos.X);
                    wb = Convert.ToInt32(((255 - sb) * pos.X) + sb);
                }
                else if (sb == 0)
                {
                    wb = Convert.ToInt32(255 * pos.X);
                    wg = Convert.ToInt32(((255 - sg) * pos.X) + sg);
                }
            }
            else if (sg == 255)
            {
                wg = 255;
                if (sr == 0)
                {
                    wr = Convert.ToInt32(255 * pos.X);
                    wb = Convert.ToInt32(((255 - sb) * pos.X) + sb);
                }
                else if (sb == 0)
                {
                    wb = Convert.ToInt32(255 * pos.X);
                    wr = Convert.ToInt32(((255 - sr) * pos.X) + sr);
                }
            }
            else if (sb == 255)
            {
                wb = 255;
                if (sr == 0)
                {
                    wr = Convert.ToInt32(255 * pos.X);
                    wg = Convert.ToInt32(((255 - sg) * pos.X) + sg);
                }
                else if (sg == 0)
                {
                    wg = Convert.ToInt32(255 * pos.X);
                    wr = Convert.ToInt32(((255 - sr) * pos.X) + sr);
                }
            }

            //Farben mischen

            gr = (wr / 255) * grau;
            gg = (wg / 255) * grau;
            gb = (wb / 255) * grau;

            SolidColorBrush color = new SolidColorBrush(new System.Windows.Media.Color() { R = Convert.ToByte(gr), G = Convert.ToByte(gg), B = Convert.ToByte(gb) });

            return color;
        }

        private double GetGradientOffset(double left, double width)
        {
            double offset = (MousePosition().X - left) / width;
            return offset;
        }

        private void SetGradient(System.Windows.Media.Color color)
        {
            VisualBrush a = new VisualBrush
            {
                Visual = new Canvas()
                {
                    Background = System.Windows.Media.Brushes.Black,
                    Width = 1,
                    Height = 1,
                    SnapsToDevicePixels = true,
                }
            };

            ((Canvas)a.Visual).Children.Add(new System.Windows.Shapes.Rectangle()
            {
                Width = 1,
                Height = 1,
                SnapsToDevicePixels = true,
                Fill = new LinearGradientBrush()
                {
                    StartPoint = new System.Windows.Point(0, 0),
                    EndPoint = new System.Windows.Point(1, 0),
                },
                OpacityMask = new LinearGradientBrush()
                {
                    StartPoint = new System.Windows.Point(0, 0),
                    EndPoint = new System.Windows.Point(0, 1),
                },
            });

            ((LinearGradientBrush)((System.Windows.Shapes.Rectangle)((Canvas)a.Visual).Children[0]).Fill).GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromRgb(255, 255, 255), 0));
            ((LinearGradientBrush)((System.Windows.Shapes.Rectangle)((Canvas)a.Visual).Children[0]).Fill).GradientStops.Add(new GradientStop(color, 1));

            ((LinearGradientBrush)((System.Windows.Shapes.Rectangle)((Canvas)a.Visual).Children[0]).OpacityMask).GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromArgb(255, 255, 255, 255), 0));
            ((LinearGradientBrush)((System.Windows.Shapes.Rectangle)((Canvas)a.Visual).Children[0]).OpacityMask).GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromArgb(0, 255, 255, 255), 1));

            RCT_gradient.Fill = a;
        }

        private Color GetRelativeColor(GradientStopCollection gsc, double offset)
        {
            GradientStop before = gsc.Where(w => w.Offset == gsc.Min(m => m.Offset)).First();
            GradientStop after = gsc.Where(w => w.Offset == gsc.Max(m => m.Offset)).First();

            foreach (var gs in gsc)
            {
                if (gs.Offset < offset && gs.Offset > before.Offset)
                    before = gs;
                if (gs.Offset > offset && gs.Offset < after.Offset)
                    after = gs;
            }

            var color = new System.Windows.Media.Color
            {
                ScA = (float)((offset - before.Offset) * (after.Color.ScA - before.Color.ScA) / (after.Offset - before.Offset) + before.Color.ScA),
                ScR = (float)((offset - before.Offset) * (after.Color.ScR - before.Color.ScR) / (after.Offset - before.Offset) + before.Color.ScR),
                ScG = (float)((offset - before.Offset) * (after.Color.ScG - before.Color.ScG) / (after.Offset - before.Offset) + before.Color.ScG),
                ScB = (float)((offset - before.Offset) * (after.Color.ScB - before.Color.ScB) / (after.Offset - before.Offset) + before.Color.ScB)
            };

            return color;
        }

        private double GetGrayValueByColor(SolidColorBrush color)
        {
            double grayvalue = 0;

            double cr = color.Color.R;
            double cg = color.Color.G;
            double cb = color.Color.B;

            if (cr >= cg && cr >= cb)
            {
                grayvalue = cr;
            }
            else if (cg >= cr && cg >= cb)
            {
                grayvalue = cg;
            }
            else if (cb >= cr && cb >= cg)
            {
                grayvalue = cb;
            }
            else
            {
                throw new Exception("Something went wrong in GetGrayValueByColor");
            }

            return grayvalue;
        }

        private Point GetRelativeGridPosition(SolidColorBrush supercol, SolidColorBrush color)
        {
            double gridpos = 0;
            double grayvalue = GetGrayValueByColor(color);

            if (grayvalue == 0)
                return new Point() { X = 1, Y = 0 };

            double cr = color.Color.R;
            double cg = color.Color.G;
            double cb = color.Color.B;

            double sr = supercol.Color.R;
            double sg = supercol.Color.G;
            double sb = supercol.Color.B;

            double wr = 0;
            double wg = 0;
            double wb = 0;


            if (sr == 255)
            {
                wr = 255;
                wg = (255 * cg) / grayvalue;
                wb = (255 * cb) / grayvalue;

                if (sg == 0)
                {
                    gridpos = wg / 255;
                }
                else if (sb == 0)
                {
                    gridpos = wb / 255;
                }
            }
            else if (sg == 255)
            {
                wg = 255;
                wr = (255 * cr) / grayvalue;
                wb = (255 * cb) / grayvalue;

                if (sr == 0)
                {
                    gridpos = wr / 255;
                }
                else if (sb == 0)
                {
                    gridpos = wb / 255;
                }
            }
            else if (sb == 255)
            {
                wb = 255;
                wg = (255 * cg) / grayvalue;
                wr = (255 * cr) / grayvalue;

                if (sg == 0)
                {
                    gridpos = wg / 255;
                }
                else if (sr == 0)
                {
                    gridpos = wr / 255;
                }
            }

            return new Point() { X = gridpos, Y = grayvalue / 255 };
        }

        private SolidColorBrush GetSupercolor(int r, int g, int b)
        {
            double[] hsl = RgbToHls(r, g, b);
            int[] rgb = HslToRgb(hsl[0], 1, 0.5);
            return new SolidColorBrush() { Color = new Color() { R = Convert.ToByte(rgb[0]), G = Convert.ToByte(rgb[1]), B = Convert.ToByte(rgb[2]), A = 255 } };
        }
        #endregion

        #region HSR RGB Conversion Functions
        private double[] RgbToHls(int r, int g, int b)
        {
            double h;
            double s;
            double l;

            // Convert RGB to a 0.0 to 1.0 range.
            double double_r = r / 255.0;
            double double_g = g / 255.0;
            double double_b = b / 255.0;

            // Get the maximum and minimum RGB components.
            double max = double_r;
            if (max < double_g) max = double_g;
            if (max < double_b) max = double_b;

            double min = double_r;
            if (min > double_g) min = double_g;
            if (min > double_b) min = double_b;

            double diff = max - min;
            l = (max + min) / 2;
            if (Math.Abs(diff) < 0.00001)
            {
                s = 0;
                h = 0;  // H is really undefined.
            }
            else
            {
                if (l <= 0.5) s = diff / (max + min);
                else s = diff / (2 - max - min);

                double r_dist = (max - double_r) / diff;
                double g_dist = (max - double_g) / diff;
                double b_dist = (max - double_b) / diff;

                if (double_r == max) h = b_dist - g_dist;
                else if (double_g == max) h = 2 + r_dist - b_dist;
                else h = 4 + g_dist - r_dist;

                h = h * 60;
                if (h < 0) h += 360;
            }

            return new double[] { h, s, l };
        }

        private int[] HslToRgb(double h, double s, double l)
        {
            int r;
            int g;
            int b;

            double p2;
            if (l <= 0.5) p2 = l * (1 + s);
            else p2 = l + s - l * s;

            double p1 = 2 * l - p2;
            double double_r, double_g, double_b;
            if (s == 0)
            {
                double_r = l;
                double_g = l;
                double_b = l;
            }
            else
            {
                double_r = QqhToRgb(p1, p2, h + 120);
                double_g = QqhToRgb(p1, p2, h);
                double_b = QqhToRgb(p1, p2, h - 120);
            }

            // Convert RGB to the 0 to 255 range.
            r = (int)(double_r * 255.0);
            g = (int)(double_g * 255.0);
            b = (int)(double_b * 255.0);

            return new int[] { r, g, b };
        }

        private static double QqhToRgb(double q1, double q2, double hue)
        {
            if (hue > 360) hue -= 360;
            else if (hue < 0) hue += 360;

            if (hue < 60) return q1 + (q2 - q1) * hue / 60;
            if (hue < 180) return q2;
            if (hue < 240) return q1 + (q2 - q1) * (240 - hue) / 60;
            return q1;
        }
        #endregion

        #region ARGB TextBox Control
        private void TB_a_TextChanged(object sender, TextChangedEventArgs e)
        {

            byte transp = 0;

            if (!argbSetting)
            {
                try
                {
                    transp = Convert.ToByte(TB_a.Text);
                }
                catch { return; }

                SetColor(transp);
            }
        }

        private void TB_rgb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!argbSetting)
            {
                byte R = 0;
                byte G = 0;
                byte B = 0;

                try
                {
                    R = Convert.ToByte(TB_r.Text);
                    G = Convert.ToByte(TB_g.Text);
                    B = Convert.ToByte(TB_b.Text);
                }
                catch { return; }

                if (leftmouse == false && superleftmouse == false)
                    SetColor(R, G, B);
            }
        }

        private void UpdateRGBText()
        {
            argbSetting = true;
            TB_a.Text = CurrentColor.Color.A.ToString();
            TB_r.Text = CurrentColor.Color.R.ToString();
            TB_g.Text = CurrentColor.Color.G.ToString();
            TB_b.Text = CurrentColor.Color.B.ToString();
            argbSetting = false;

            if (CurrentColor.Color.R + CurrentColor.Color.G + CurrentColor.Color.B < 260 && CurrentColor.Color.A > 100)
            {
                TB_a.C_ForegroundTheme = CColor.Theme.White;
                TB_r.C_ForegroundTheme = CColor.Theme.White;
                TB_g.C_ForegroundTheme = CColor.Theme.White;
                TB_b.C_ForegroundTheme = CColor.Theme.White;

                L_a.C_ForegroundTheme = CColor.Theme.White;
                L_r.C_ForegroundTheme = CColor.Theme.White;
                L_g.C_ForegroundTheme = CColor.Theme.White;
                L_b.C_ForegroundTheme = CColor.Theme.White;

                TB_a.C_BorderTheme = CColor.Theme.White;
                TB_r.C_BorderTheme = CColor.Theme.White;
                TB_g.C_BorderTheme = CColor.Theme.White;
                TB_b.C_BorderTheme = CColor.Theme.White;
            }
            else
            {
                TB_a.C_ForegroundTheme = CColor.Theme.Black;
                TB_r.C_ForegroundTheme = CColor.Theme.Black;
                TB_g.C_ForegroundTheme = CColor.Theme.Black;
                TB_b.C_ForegroundTheme = CColor.Theme.Black;

                L_a.C_ForegroundTheme = CColor.Theme.Black;
                L_r.C_ForegroundTheme = CColor.Theme.Black;
                L_g.C_ForegroundTheme = CColor.Theme.Black;
                L_b.C_ForegroundTheme = CColor.Theme.Black;

                TB_a.C_BorderTheme = CColor.Theme.Black;
                TB_r.C_BorderTheme = CColor.Theme.Black;
                TB_g.C_BorderTheme = CColor.Theme.Black;
                TB_b.C_BorderTheme = CColor.Theme.Black;
            }
        }
        #endregion

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (currentColor == null)
                SetColor(255, 255, 0, 0);
        }

        private System.Windows.Point MousePosition()
        {
            Window parentWindow = Window.GetWindow(this);

            System.Windows.Point mousePoint = parentWindow.PointToScreen(Mouse.GetPosition(parentWindow));
            double x = Convert.ToInt32(mousePoint.X - parentWindow.Left) - 8;
            double y = Convert.ToInt32(mousePoint.Y - parentWindow.Top) - 31;
            return new System.Windows.Point(x, y);
        }
    }
}
