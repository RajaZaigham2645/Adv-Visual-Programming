using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CarShowroom
{
    public static class UIHelper
    {
        public static Button MakeButton(string text, Color bg, int w = 130, int h = 38)
        {
            var b = new Button
            {
                Text = text, Width = w, Height = h,
                BackColor = bg, ForeColor = Theme.TextLight,
                FlatStyle = FlatStyle.Flat, Font = Theme.H3,
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderColor = Color.FromArgb(60, 70, 110);
            b.FlatAppearance.MouseOverBackColor = ControlPaint.Light(bg, 0.2f);
            return b;
        }

        public static TextBox MakeTextBox(int w = 220)
        {
            var tb = new TextBox
            {
                Width = w, Height = 30,
                BackColor = Theme.Dark3, ForeColor = Theme.TextLight,
                BorderStyle = BorderStyle.FixedSingle, Font = Theme.Body
            };
            return tb;
        }

        public static ComboBox MakeCombo(int w = 220)
        {
            var cb = new ComboBox
            {
                Width = w, DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Theme.Dark3, ForeColor = Theme.TextLight,
                FlatStyle = FlatStyle.Flat, Font = Theme.Body
            };
            return cb;
        }

        public static Label MakeLabel(string text, Font font = null, Color? color = null)
        {
            return new Label
            {
                Text = text, AutoSize = true,
                Font = font ?? Theme.Body,
                ForeColor = color ?? Theme.TextLight,
                BackColor = Color.Transparent
            };
        }

        public static DataGridView MakeGrid()
        {
            var g = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Theme.Dark2,
                GridColor = Theme.Border,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = Theme.Body,
                ColumnHeadersHeight = 38
            };
            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersDefaultCellStyle.BackColor = Theme.Dark3;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Theme.Gold;
            g.ColumnHeadersDefaultCellStyle.Font = Theme.H3;
            g.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            g.DefaultCellStyle.BackColor = Theme.Dark2;
            g.DefaultCellStyle.ForeColor = Theme.TextLight;
            g.DefaultCellStyle.SelectionBackColor = Theme.Accent;
            g.DefaultCellStyle.SelectionForeColor = Color.White;
            g.AlternatingRowsDefaultCellStyle.BackColor = Theme.Dark3;
            g.RowTemplate.Height = 30;
            return g;
        }
    }
}
