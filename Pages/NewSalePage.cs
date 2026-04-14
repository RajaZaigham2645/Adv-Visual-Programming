using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CarShowroom
{
    public class NewSalePage : Panel
    {
        public event Action SaleCompleted;
        private ComboBox cbCar, cbCustomer, cbPayment;
        private TextBox tbPrice, tbRep;

        public NewSalePage()
        {
            Dock = DockStyle.Fill; BackColor = Theme.Dark2; Padding = new Padding(40);

            var title = UIHelper.MakeLabel("💰  Record New Sale", Theme.Title, Theme.Gold);
            title.Location = new Point(40, 24);

            var card = new Panel { Left = 40, Top = 80, Width = 560, Height = 440, BackColor = Theme.Dark3 };
            card.Paint += (s, e) =>
            {
                e.Graphics.DrawRectangle(new System.Drawing.Pen(Theme.Border, 1), 0, 0, card.Width - 1, card.Height - 1);
            };

            cbCar      = UIHelper.MakeCombo(480); cbCustomer = UIHelper.MakeCombo(480);
            cbPayment  = UIHelper.MakeCombo(240); tbPrice    = UIHelper.MakeTextBox(240);
            tbRep      = UIHelper.MakeTextBox(240);

            cbPayment.Items.AddRange(new object[] { "Cash", "Bank Transfer", "Installment", "Cheque", "Online" });
            cbPayment.SelectedIndex = 0;

            int y = 20;
            void Row(string lbl, Control c, int width = 480) {
                var l = UIHelper.MakeLabel(lbl, Theme.Small, Theme.TextDim); l.Location = new Point(24, y);
                c.Location = new Point(24, y + 20); c.Width = width;
                card.Controls.AddRange(new Control[] { l, c }); y += 62;
            }

            Row("Select Car (Available Only)", cbCar);
            Row("Select Customer", cbCustomer);
            Row("Sale Price (PKR) *", tbPrice, 240);
            Row("Payment Mode", cbPayment, 240);
            Row("Sales Representative", tbRep, 240);

            var btnSell = UIHelper.MakeButton("✅  Complete Sale", Theme.Accent, 200, 48);
            btnSell.Location = new Point(24, y + 10); btnSell.Font = Theme.H2;
            btnSell.Click += CompleteSale;
            card.Controls.Add(btnSell);

            Controls.AddRange(new Control[] { title, card });
        }

        public void Reload()
        {
            cbCar.Items.Clear();
            foreach (var c in DataStore.Cars.Where(x => x.Status == CarStatus.Available))
                cbCar.Items.Add(c);
            if (cbCar.Items.Count > 0) cbCar.SelectedIndex = 0;

            cbCustomer.Items.Clear();
            foreach (var c in DataStore.Customers) cbCustomer.Items.Add(c);
            if (cbCustomer.Items.Count > 0) cbCustomer.SelectedIndex = 0;

            cbCar.SelectedIndexChanged -= AutoFillPrice;
            cbCar.SelectedIndexChanged += AutoFillPrice;
            if (cbCar.SelectedItem is Car fc) tbPrice.Text = fc.Price.ToString("F0");
        }

        private void AutoFillPrice(object s, EventArgs e)
        {
            if (cbCar.SelectedItem is Car car) tbPrice.Text = car.Price.ToString("F0");
        }

        private void CompleteSale(object s, EventArgs e)
        {
            if (cbCar.SelectedItem == null || cbCustomer.SelectedItem == null)
            { MessageBox.Show("Select car and customer.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!double.TryParse(tbPrice.Text, out double price) || price <= 0)
            { MessageBox.Show("Invalid price.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            var car  = (Car)cbCar.SelectedItem;
            var cust = (Customer)cbCustomer.SelectedItem;

            car.Status = CarStatus.Sold;
            DataStore.Sales.Add(new Sale
            {
                Id = DataStore.NextSaleId(), Car = car, Customer = cust,
                SalePrice = price, SaleDate = DateTime.Now,
                PaymentMode = cbPayment.SelectedItem.ToString(),
                SalesRep = string.IsNullOrWhiteSpace(tbRep.Text) ? "—" : tbRep.Text.Trim()
            });

            MessageBox.Show($"🎉  Sale completed!\n{car}\nSold to: {cust.Name}\nPrice: PKR {price:N0}",
                            "Sale Recorded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SaleCompleted?.Invoke();
            Reload();
        }
    }
}
