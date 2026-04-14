using System.Drawing;
using System.Windows.Forms;

namespace CarShowroom
{
    public class SettingsPage : Panel
    {
        public SettingsPage()
        {
            Dock = DockStyle.Fill; BackColor = Theme.Dark2; Padding = new Padding(40);

            var title = UIHelper.MakeLabel("⚙️  Settings & About", Theme.Title, Theme.Gold);
            title.Location = new Point(40, 24);

            var info = new Label
            {
                Left = 40, Top = 100, AutoSize = false, Width = 700, Height = 500,
                Font = Theme.Body, ForeColor = Theme.TextLight, BackColor = System.Drawing.Color.Transparent,
                Text =
                    "AutoElite Car Showroom Management System v2.0\n" +
                    "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n\n" +
                    "Version   : 2.0.0 (Enhanced)\n" +
                    "Platform  : .NET 6 / Windows Forms\n" +
                    "Language  : C#\n" +
                    "Database  : In-Memory\n\n" +
                    "✨ Core Features:\n" +
                    "  📦 Inventory Management (40+ vehicles)\n" +
                    "  👥 Customer Database Management\n" +
                    "  💼 Sales Recording & Tracking\n" +
                    "  📊 Real-time Dashboard Statistics\n" +
                    "  🔍 Advanced Search with Filters\n" +
                    "  💰 Revenue Tracking & Analytics\n" +
                    "  ⚡ Multiple Filtering Options\n\n" +
                    "🔧 Professional Features:\n" +
                    "  • Price Range Filtering\n" +
                    "  • Fuel Type & Status Filters\n" +
                    "  • Advanced Search Engine\n" +
                    "  • Sales Statistics & Reports\n" +
                    "  • Data Validation\n" +
                    "  • Professional UI/UX\n\n" +
                    "📝 Developer Notes:\n" +
                    "  Data is held in memory (no database).\n" +
                    "  To persist data, integrate SQL Server/SQLite.\n\n" +
                    "© 2024 AutoElite Showroom. All rights reserved."
            };

            Controls.AddRange(new Control[] { title, info });
        }
    }
}
