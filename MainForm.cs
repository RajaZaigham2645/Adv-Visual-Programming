using System;
using System.Drawing;
using System.Windows.Forms;

namespace CarShowroom
{
    public class MainForm : Form
    {
        private SideNav _nav;
        private Panel   _content;

        private DashboardPage _dashboard;
        private InventoryPage _inventory;
        private AddCarPage    _addCar;
        private CustomersPage _customers;
        private NewSalePage   _newSale;
        private SalesLogPage  _salesLog;
        private SearchPage    _search;
        private SettingsPage  _settings;

        public MainForm()
        {
            Text = "AutoElite Car Showroom - v2.0 Professional";
            Size = new System.Drawing.Size(1400, 800);
            MinimumSize = new System.Drawing.Size(1100, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Theme.Dark2;
            Icon = SystemIcons.Application;

            DataStore.Seed();

            _nav = new SideNav();
            _nav.PageChanged += ShowPage;

            _content = new Panel { Dock = DockStyle.Fill, BackColor = Theme.Dark2 };

            _dashboard = new DashboardPage();
            _inventory  = new InventoryPage();
            _addCar     = new AddCarPage();
            _customers  = new CustomersPage();
            _newSale    = new NewSalePage();
            _salesLog   = new SalesLogPage();
            _search     = new SearchPage();
            _settings   = new SettingsPage();

            _addCar.CarAdded += () => { _inventory.LoadGrid(); _dashboard.Refresh(); };
            _newSale.SaleCompleted += () => { _inventory.LoadGrid(); _dashboard.Refresh(); _salesLog.LoadGrid(); };

            foreach (var p in new Panel[] { _dashboard, _inventory, _addCar, _customers, _newSale, _salesLog, _search, _settings })
            { p.Visible = false; _content.Controls.Add(p); }

            _dashboard.Visible = true;

            Controls.Add(_content);
            Controls.Add(_nav);

            _newSale.Reload();
        }

        private void ShowPage(int idx)
        {
            foreach (Control c in _content.Controls) c.Visible = false;
            var pages = new Panel[] { _dashboard, _inventory, _addCar, _customers, _newSale, _salesLog, _search, _settings };
            if (idx < pages.Length) pages[idx].Visible = true;

            if (idx == 0) _dashboard.Refresh();
            if (idx == 1) _inventory.LoadGrid();
            if (idx == 3) _customers.LoadGrid();
            if (idx == 4) _newSale.Reload();
            if (idx == 5) _salesLog.LoadGrid();
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
