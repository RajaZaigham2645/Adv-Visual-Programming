using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CarShowroom
{
    public class InventoryPage : Panel
    {
        private DataGridView _grid;
        private TextBox _filterBrand, _filterPrice;
        private ComboBox _filterStatus, _filterFuel;
        private Label _countLabel;

        public InventoryPage()
        {
            Dock = DockStyle.Fill; BackColor = Theme.Dark2;

            var header = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = Theme.Dark1, Padding = new Padding(16, 12, 16, 0) };
            var title = UIHelper.MakeLabel("🚗  Car Inventory (" + DataStore.Cars.Count + " vehicles)", Theme.H2, Theme.Gold);
            title.Location = new Point(16, 16);

            // Filter controls
            _filterBrand = UIHelper.MakeTextBox(120); _filterBrand.Location = new Point(16, 54);
            _filterStatus = UIHelper.MakeCombo(110); _filterStatus.Location = new Point(146, 54);
            _filterStatus.Items.AddRange(new object[] { "All", "Available", "Sold", "Reserved" });
            _filterStatus.SelectedIndex = 0;
            
            _filterFuel = UIHelper.MakeCombo(110); _filterFuel.Location = new Point(266, 54);
            _filterFuel.Items.AddRange(new object[] { "All", "Petrol", "Diesel", "Hybrid", "Electric", "CNG" });
            _filterFuel.SelectedIndex = 0;

            _filterPrice = UIHelper.MakeTextBox(100); _filterPrice.Location = new Point(386, 54);

            var btnFilter = UIHelper.MakeButton("Apply", Theme.Accent, 80, 30); btnFilter.Location = new Point(496, 54);
            btnFilter.Click += (s, e) => LoadGrid();

            var btnRefresh = UIHelper.MakeButton("↺ Refresh", Theme.Dark3, 100, 30); btnRefresh.Location = new Point(586, 54);
            btnRefresh.Click += (s, e) => { _filterBrand.Text = ""; _filterStatus.SelectedIndex = 0; _filterFuel.SelectedIndex = 0; _filterPrice.Text = ""; LoadGrid(); };

            _countLabel = UIHelper.MakeLabel("Showing: 0", Theme.Small, Theme.TextDim);
            _countLabel.Location = new Point(16, 90);

            header.Controls.AddRange(new Control[] { title, _filterBrand, _filterStatus, _filterFuel, _filterPrice, btnFilter, btnRefresh, _countLabel });

            _grid = UIHelper.MakeGrid();
            _grid.Columns.Add("Id",     "ID");
            _grid.Columns.Add("Brand",  "Brand");
            _grid.Columns.Add("Model",  "Model");
            _grid.Columns.Add("Year",   "Year");
            _grid.Columns.Add("Color",  "Color");
            _grid.Columns.Add("Fuel",   "Fuel");
            _grid.Columns.Add("Trans",  "Transmission");
            _grid.Columns.Add("Miles",  "Mileage");
            _grid.Columns.Add("Price",  "Price (PKR)");
            _grid.Columns.Add("Status", "Status");
            _grid.Columns["Id"].FillWeight = 30;
            _grid.Columns["Year"].FillWeight = 40;

            _grid.CellFormatting += Grid_CellFormatting;

            Controls.Add(_grid);
            Controls.Add(header);
            LoadGrid();
        }

        public void LoadGrid()
        {
            _grid.Rows.Clear();
            var q = DataStore.Cars.AsEnumerable();
            
            if (!string.IsNullOrWhiteSpace(_filterBrand.Text))
                q = q.Where(c => c.Brand.IndexOf(_filterBrand.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            
            if (_filterStatus.SelectedIndex > 0)
                q = q.Where(c => c.Status.ToString() == _filterStatus.SelectedItem.ToString());
            
            if (_filterFuel.SelectedIndex > 0)
                q = q.Where(c => c.FuelType == _filterFuel.SelectedItem.ToString());
            
            if (!string.IsNullOrWhiteSpace(_filterPrice.Text) && double.TryParse(_filterPrice.Text, out double maxPrice))
                q = q.Where(c => c.Price <= maxPrice);
            
            var results = q.OrderByDescending(x => x.AddedDate).ToList();
            _countLabel.Text = $"Showing: {results.Count} of {DataStore.Cars.Count}";
            
            foreach (var c in results)
                _grid.Rows.Add(c.Id, c.Brand, c.Model, c.Year, c.Color, c.FuelType, c.Transmission, c.Mileage == 0 ? "New" : c.Mileage + " km", "PKR " + c.Price.ToString("N0"), c.Status.ToString());
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_grid.Columns[e.ColumnIndex].Name == "Status" && e.Value != null)
            {
                e.CellStyle.ForeColor = e.Value.ToString() switch
                {
                    "Available" => Theme.Success,
                    "Sold"      => Theme.Accent,
                    "Reserved"  => Theme.Warning,
                    _           => Theme.TextLight
                };
                e.CellStyle.Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold);
            }
        }
    }
}
