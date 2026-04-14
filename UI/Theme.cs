using System.Drawing;
using System.Windows.Forms;

namespace CarShowroom
{
    public static class Theme
    {
        public static Color Dark1     = Color.FromArgb(10,  10,  20);
        public static Color Dark2     = Color.FromArgb(18,  22,  38);
        public static Color Dark3     = Color.FromArgb(26,  32,  55);
        public static Color Accent    = Color.FromArgb(220, 50,  50);
        public static Color AccentH   = Color.FromArgb(255, 80,  80);
        public static Color Gold      = Color.FromArgb(255, 200, 50);
        public static Color TextLight = Color.FromArgb(230, 230, 245);
        public static Color TextDim   = Color.FromArgb(150, 155, 175);
        public static Color Success   = Color.FromArgb(50,  200, 100);
        public static Color Warning   = Color.FromArgb(255, 165, 0);
        public static Color Border    = Color.FromArgb(40,  48,  80);
        public static Color Info      = Color.FromArgb(100, 180, 255);

        public static Font Title   = new Font("Segoe UI", 22, FontStyle.Bold);
        public static Font H2      = new Font("Segoe UI", 14, FontStyle.Bold);
        public static Font H3      = new Font("Segoe UI", 11, FontStyle.Bold);
        public static Font Body    = new Font("Segoe UI", 10);
        public static Font Small   = new Font("Segoe UI",  9);
        public static Font Mono    = new Font("Consolas",  10);
    }
}
