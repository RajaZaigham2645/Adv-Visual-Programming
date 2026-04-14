using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace CarShowroom
{
    public class SideNav : Panel
    {
        private readonly string[] _items = { "🏠  Dashboard", "🚗  Inventory", "➕  Add Car", "👤  Customers", "💰  New Sale", "📊  Sales Log", "🔍  Search", "⚙️  Settings" };
        private int _selected = 0;
        public event Action<int> PageChanged;

        public SideNav()
        {
            Width = 210; Dock = DockStyle.Left;
            BackColor = Theme.Dark1; Padding = new Padding(0, 10, 0, 0);

            for (int i = 0; i < _items.Length; i++)
            {
                int idx = i;
                var btn = new Panel
                {
                    Height = 46, Dock = DockStyle.Top,
                    Tag = i, BackColor = i == 0 ? Theme.Accent : Color.Transparent,
                    Cursor = Cursors.Hand
                };
                var lbl = new Label
                {
                    Text = _items[i], AutoSize = false,
                    Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft,
                    Font = Theme.H3, ForeColor = Theme.TextLight,
                    BackColor = Color.Transparent, Padding = new Padding(18, 0, 0, 0)
                };
                btn.Controls.Add(lbl);
                btn.Click += (s, e) => Select(idx);
                lbl.Click += (s, e) => Select(idx);
                Controls.Add(btn);
                Controls.SetChildIndex(btn, 0);
            }

            var btns = Controls.Cast<Control>().Reverse().ToList();
            Controls.Clear();
            btns.ForEach(b => Controls.Add(b));

            var logo = new Panel { Height = 70, Dock = DockStyle.Top, BackColor = Theme.Dark1 };
            logo.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.FillEllipse(new SolidBrush(Theme.Accent), 14, 18, 34, 34);
                e.Graphics.DrawString("🚘", new Font("Segoe UI Emoji", 16), Brushes.White, 14, 18);
                e.Graphics.DrawString("AutoElite", Theme.H2, new SolidBrush(Theme.TextLight), 56, 22);
                e.Graphics.DrawString("Showroom", Theme.Small, new SolidBrush(Theme.TextDim), 58, 44);
            };
            Controls.Add(logo);
            Controls.SetChildIndex(logo, 0);
        }

        private void Select(int idx)
        {
            _selected = idx;
            foreach (Control c in Controls)
            {
                if (c is Panel p && p.Tag is int i)
                    p.BackColor = i == idx ? Theme.Accent : Color.Transparent;
            }
            PageChanged?.Invoke(idx);
        }
    }
}
