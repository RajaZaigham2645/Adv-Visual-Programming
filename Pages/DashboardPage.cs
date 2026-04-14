using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CarShowroom
{
    public class DashboardPage : Panel
    {
        private FlowLayoutPanel _cards;
        private DataGridView _recentGrid;

        public DashboardPage()
        {
            Dock = DockStyle.Fill; BackColor = Theme.Dark2; Padding = new Padding(24);

            var title = UIHelper.MakeLabel("Dashboard Overview", Theme.Title, Theme.Gold);
            title.Location = new Point(24, 24);

            _cards = new FlowLayoutPanel
            {
                Left = 24, Top = 74, Width = 900, Height = 120,
                BackColor = System.Drawing.Color.Transparent, FlowDirection = FlowDirection.LeftToRight
            };
            _cards.Controls.Add(new StatCard("Total Cars", "0", Theme.Accent));
            _cards.Controls.Add(new StatCard("Available", "0", Theme.Success));
            _cards.Controls.Add(new StatCard("Sold", "0", Theme.Gold));
            _cards.Controls.Add(new StatCard("Revenue (PKR)", "0", Theme.Info));

            var recentLabel = UIHelper.MakeLabel("Recent Sales", Theme.H2, Theme.TextLight);
            recentLabel.Location = new Point(24, 210);

            var gridPanel = new Panel { Left = 24, Top = 244, Width = 900, Height = 310, BackColor = Theme.Dark2 };
            _recentGrid = UIHelper.MakeGrid();
            _recentGrid.Columns.Add("Date",    "Date");
            _recentGrid.Columns.Add("Car",     "Car");
            _recentGrid.Columns.Add("Customer","Customer");
            _recentGrid.Columns.Add("Price",   "Price (PKR)");
            _recentGrid.Columns.Add("Payment", "Payment");
            gridPanel.Controls.Add(_recentGrid);

            Controls.AddRange(new Control[] { title, _cards, recentLabel, gridPanel });
            Refresh();
        }

        public new void Refresh()
        {
            _cards.Controls.Clear();
            _cards.Controls.Add(new StatCard("Total Cars",     DataStore.Cars.Count.ToString(), Theme.Accent));
            _cards.Controls.Add(new StatCard("Available",      DataStore.Cars.Count(c => c.Status == CarStatus.Available).ToString(), Theme.Success));
            _cards.Controls.Add(new StatCard("Sold",           DataStore.Cars.Count(c => c.Status == CarStatus.Sold).ToString(), Theme.Gold));
            _cards.Controls.Add(new StatCard("Revenue (PKR)",  "PKR " + DataStore.Sales.Sum(s => s.SalePrice).ToString("N0"), Theme.Info));

            _recentGrid.Rows.Clear();
            foreach (var s in DataStore.Sales.OrderByDescending(x => x.SaleDate).Take(20))
                _recentGrid.Rows.Add(s.SaleDate.ToString("dd-MMM-yy"), s.Car.ToString(), s.Customer.Name, s.SalePrice.ToString("N0"), s.PaymentMode);
        }
    }
}
