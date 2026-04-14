using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CarShowroom
{
    public class SearchPage : Panel
    {
        private TextBox _tbQ;
        private DataGridView _grid;
        private ComboBox _cbSearchBy;
        private Label _resultLabel;
        private NumericUpDown _minPrice, _maxPrice;

        public SearchPage()
        {
            Dock = DockStyle.Fill; BackColor = Theme.Dark2;

            var header = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Theme.Dark1, Padding = new Padding(16, 12, 0, 0) };
            var title = UIHelper.MakeLabel("🔍  Advanced Search", Theme.H2, Theme.Gold);
            title.Location = new Point(16, 14);
            
            var lbl1 = UIHelper.MakeLabel("Search Query:", Theme.Small, Theme.TextDim);
            lbl1.Location = new Point(16, 40);
            _tbQ = UIHelper.MakeTextBox(340); _tbQ.Location = new Point(120, 40);
            
            var lbl2 = UIHelper.MakeLabel("Search In:", Theme.Small, Theme.TextDim);
            lbl2.Location = new Point(16, 70);
            _cbSearchBy = UIHelper.MakeCombo(340); _cbSearchBy.Location = new Point(120, 70);
            _cbSearchBy.Items.AddRange(new object[] { "All Fields", "Brand", "Model", "Color", "Description" });
            _cbSearchBy.SelectedIndex = 0;
            
            var lbl3 = UIHelper.MakeLabel("Price Range (PKR):", Theme.Small, Theme.TextDim);
            lbl3.Location = new Point(470, 40);
            
            _minPrice = new NumericUpDown { Width = 100, Height = 30, Location = new Point(580, 40), BackColor = Theme.Dark3, ForeColor = Theme.TextLight };
            _minPrice.Minimum = 0;
            _minPrice.Maximum = 100000000;
            
            _maxPrice = new NumericUpDown { Width = 100, Height = 30, Location = new Point(690, 40), BackColor = Theme.Dark3, ForeColor = Theme.TextLight };
            _maxPrice.Minimum = 0;
            _maxPrice.Maximum = 100000000;
            _maxPrice.Value = 100000000;
            
            var btn = UIHelper.MakeButton("Search", Theme.Accent, 100, 30); btn.Location = new Point(16, 70);
            btn.Click += (s, e) => DoSearch();
            
            _tbQ.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) DoSearch(); };
            
            _resultLabel = UIHelper.MakeLabel("Ready to search", Theme.Small, Theme.Success);
            _resultLabel.Location = new Point(800, 40);
            
            header.Controls.AddRange(new Control[] { title, lbl1, _tbQ, lbl2, _cbSearchBy, lbl3, _minPrice, _maxPrice, btn, _resultLabel });

            _grid = UIHelper.MakeGrid();
            _grid.Columns.Add("Brand","Brand"); _grid.Columns.Add("Model","Model");
            _grid.Columns.Add("Year","Year"); _grid.Columns.Add("Color","Color");
            _grid.Columns.Add("Fuel","Fuel"); _grid.Columns.Add("Trans","Trans");
            _grid.Columns.Add("Mileage","Mileage (km)"); _grid.Columns.Add("Price","Price (PKR)");
            _grid.Columns.Add("Status","Status"); _grid.Columns.Add("Desc","Description");

            Controls.Add(_grid); Controls.Add(header);
        }

        private void DoSearch()
        {
            var q = _tbQ.Text.Trim().ToLower();
            _grid.Rows.Clear();
            
            var results = DataStore.Cars.Where(c =>
            {
                bool matches = _cbSearchBy.SelectedIndex == 0 ? 
                    (c.Brand.ToLower().Contains(q) || c.Model.ToLower().Contains(q) ||
                     c.Color.ToLower().Contains(q) || c.FuelType.ToLower().Contains(q) ||
                     c.Price.ToString().Contains(q) || c.Description.ToLower().Contains(q)) :
                    _cbSearchBy.SelectedIndex == 1 ? c.Brand.ToLower().Contains(q) :
                    _cbSearchBy.SelectedIndex == 2 ? c.Model.ToLower().Contains(q) :
                    _cbSearchBy.SelectedIndex == 3 ? c.Color.ToLower().Contains(q) :
                    c.Description.ToLower().Contains(q);
                
                return matches && c.Price >= (double)_minPrice.Value && c.Price <= (double)_maxPrice.Value;
            }).ToList();

            foreach (var c in results)
                _grid.Rows.Add(c.Brand, c.Model, c.Year, c.Color, c.FuelType, c.Transmission, 
                              c.Mileage == 0 ? "New" : c.Mileage.ToString(), "PKR " + c.Price.ToString("N0"), 
                              c.Status, c.Description);
            
            _resultLabel.Text = $"Found: {results.Count} vehicles";
        }
    }
}
