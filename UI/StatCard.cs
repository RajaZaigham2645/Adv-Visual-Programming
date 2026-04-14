using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CarShowroom
{
    public class StatCard : Panel
    {
        public StatCard(string label, string value, Color accent)
        {
            Width = 200; Height = 100;
            BackColor = Theme.Dark3;
            Padding = new Padding(16);
            Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using var accentBrush = new SolidBrush(Color.FromArgb(60, accent));
                g.FillRectangle(accentBrush, Width - 6, 0, 6, Height);
                g.DrawString(value, new Font("Segoe UI", 22, FontStyle.Bold),
                             new SolidBrush(accent), new RectangleF(16, 14, Width - 22, 50));
                g.DrawString(label, Theme.Small,
                             new SolidBrush(Theme.TextDim), new RectangleF(16, 58, Width - 22, 30));
            };
        }
    }
}
