using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CarShowroom
{
    public class AddCarPage : Panel
    {
        public event Action CarAdded;

        private TextBox tbBrand, tbModel, tbYear, tbColor, tbPrice, tbMileage, tbDesc;
        private ComboBox cbFuel, cbTrans, cbStatus;

        public AddCarPage()
        {
            Dock = DockStyle.Fill; BackColor = Theme.Dark2; Padding = new Padding(30);

            var title = UIHelper.MakeLabel("➕  Add New Car", Theme.Title, Theme.Gold);
            title.Location = new Point(30, 20);

            var form = new Panel { Left = 30, Top = 80, Width = 700, Height = 500, BackColor = System.Drawing.Color.Transparent };

            tbBrand   = UIHelper.MakeTextBox(200); tbModel   = UIHelper.MakeTextBox(200);
            tbYear    = UIHelper.MakeTextBox(100); tbColor   = UIHelper.MakeTextBox(150);
            tbPrice   = UIHelper.MakeTextBox(160); tbMileage = UIHelper.MakeTextBox(120);
            tbDesc    = UIHelper.MakeTextBox(440); tbDesc.Height = 70; tbDesc.Multiline = true;
            cbFuel    = UIHelper.MakeCombo(160); cbTrans = UIHelper.MakeCombo(160); cbStatus = UIHelper.MakeCombo(140);

            cbFuel.Items.AddRange(new object[] { "Petrol", "Diesel", "Hybrid", "Electric", "CNG" });
            cbTrans.Items.AddRange(new object[] { "Automatic", "Manual", "CVT", "DCT" });
            cbStatus.Items.AddRange(new object[] { "Available", "Reserved" });
            cbFuel.SelectedIndex = cbTrans.SelectedIndex = cbStatus.SelectedIndex = 0;

            int y = 0, rowH = 60;
            void Row(string lbl1, Control c1, string lbl2 = null, Control c2 = null)
            {
                var l1 = UIHelper.MakeLabel(lbl1, Theme.Small, Theme.TextDim); l1.Location = new Point(0, y);
                c1.Location = new Point(0, y + 20);
                form.Controls.AddRange(new Control[] { l1, c1 });
                if (lbl2 != null && c2 != null)
                {
                    var l2 = UIHelper.MakeLabel(lbl2, Theme.Small, Theme.TextDim); l2.Location = new Point(260, y);
                    c2.Location = new Point(260, y + 20);
                    form.Controls.AddRange(new Control[] { l2, c2 });
                }
                y += rowH;
            }

            Row("Brand *", tbBrand, "Model *", tbModel);
            Row("Year *",  tbYear,  "Color", tbColor);
            Row("Fuel Type", cbFuel, "Transmission", cbTrans);
            Row("Price (PKR) *", tbPrice, "Mileage (km, 0=New)", tbMileage);
            Row("Status", cbStatus);

            var lDesc = UIHelper.MakeLabel("Description", Theme.Small, Theme.TextDim); lDesc.Location = new Point(0, y);
            tbDesc.Location = new Point(0, y + 20); tbDesc.Width = 440;
            form.Controls.AddRange(new Control[] { lDesc, tbDesc });
            y += 100;

            var btnSave = UIHelper.MakeButton("💾  Save Car", Theme.Success, 160, 42); btnSave.Location = new Point(0, y);
            var btnClear= UIHelper.MakeButton("✕  Clear",     Theme.Dark3,   120, 42); btnClear.Location = new Point(170, y);
            btnSave.Click  += SaveCar;
            btnClear.Click += (s, e) => ClearForm();
            form.Controls.AddRange(new Control[] { btnSave, btnClear });

            Controls.AddRange(new Control[] { title, form });
        }

        private void SaveCar(object s, EventArgs e)
        {
            if (!int.TryParse(tbYear.Text, out int year) || year < 1990 || year > 2030) { Show("Invalid year (1990-2030)."); return; }
            if (!double.TryParse(tbPrice.Text, out double price) || price <= 0) { Show("Invalid price."); return; }
            if (string.IsNullOrWhiteSpace(tbBrand.Text) || string.IsNullOrWhiteSpace(tbModel.Text)) { Show("Brand and Model required."); return; }

            var car = new Car
            {
                Id = DataStore.NextCarId(), Brand = tbBrand.Text.Trim(), Model = tbModel.Text.Trim(),
                Year = year, Color = tbColor.Text.Trim(), FuelType = cbFuel.SelectedItem.ToString(),
                Transmission = cbTrans.SelectedItem.ToString(), Price = price,
                Mileage = int.TryParse(tbMileage.Text, out int mi) ? mi : 0,
                Description = tbDesc.Text.Trim(), Status = Enum.Parse<CarStatus>(cbStatus.SelectedItem.ToString()),
                AddedDate = DateTime.Now
            };
            DataStore.Cars.Add(car);
            MessageBox.Show($"✅  '{car}' added successfully!", "Car Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm();
            CarAdded?.Invoke();
        }

        private void ClearForm()
        {
            foreach (Control c in Controls.Cast<Control>().SelectMany(p => p is Panel pan ? pan.Controls.Cast<Control>() : Enumerable.Empty<Control>()))
                if (c is TextBox tb) tb.Clear();
        }

        private void Show(string msg) => MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
