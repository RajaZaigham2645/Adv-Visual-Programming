using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CarShowroom
{
    public class CustomersPage : Panel
    {
        private DataGridView _grid;
        private TextBox tbName, tbPhone, tbEmail, tbCNIC, tbAddress;

        public CustomersPage()
        {
            Dock = DockStyle.Fill; BackColor = Theme.Dark2;

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill, Orientation = Orientation.Vertical,
                SplitterDistance = 520, BackColor = Theme.Dark2, Panel1MinSize = 350
            };
            split.SplitterWidth = 4;

            var lp = split.Panel1;
            var hdr = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Theme.Dark1, Padding = new Padding(12, 12, 0, 0) };
            hdr.Controls.Add(UIHelper.MakeLabel("👤  Customers (" + DataStore.Customers.Count + ")", Theme.H2, Theme.Gold));
            _grid = UIHelper.MakeGrid();
            _grid.Columns.Add("Id",      "ID");
            _grid.Columns.Add("Name",    "Name");
            _grid.Columns.Add("Phone",   "Phone");
            _grid.Columns.Add("Email",   "Email");
            _grid.Columns.Add("CNIC",    "CNIC");
            _grid.Columns.Add("Address", "Address");
            _grid.Columns["Id"].FillWeight = 30;
            lp.Controls.Add(_grid); lp.Controls.Add(hdr);

            var rp = split.Panel2; rp.BackColor = Theme.Dark3; rp.Padding = new Padding(20);
            var fl = UIHelper.MakeLabel("Add Customer", Theme.H2, Theme.Gold); fl.Location = new Point(20, 16);

            tbName    = UIHelper.MakeTextBox(240); tbPhone   = UIHelper.MakeTextBox(200);
            tbEmail   = UIHelper.MakeTextBox(240); tbCNIC    = UIHelper.MakeTextBox(200);
            tbAddress = UIHelper.MakeTextBox(240);

            int y = 60;
            void Row(string lbl, TextBox tb) {
                var l = UIHelper.MakeLabel(lbl, Theme.Small, Theme.TextDim); l.Location = new Point(20, y);
                tb.Location = new Point(20, y + 18); rp.Controls.AddRange(new Control[] { l, tb }); y += 56;
            }
            Row("Full Name *", tbName); Row("Phone *", tbPhone); Row("Email", tbEmail);
            Row("CNIC", tbCNIC); Row("Address", tbAddress);

            var btn = UIHelper.MakeButton("💾  Add Customer", Theme.Success, 180, 40); btn.Location = new Point(20, y);
            btn.Click += AddCustomer;
            rp.Controls.AddRange(new Control[] { fl, btn });

            Controls.Add(split);
            LoadGrid();
        }

        public void LoadGrid()
        {
            _grid.Rows.Clear();
            foreach (var c in DataStore.Customers)
                _grid.Rows.Add(c.Id, c.Name, c.Phone, c.Email, c.CNIC, c.Address);
        }

        private void AddCustomer(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName.Text) || string.IsNullOrWhiteSpace(tbPhone.Text))
            { MessageBox.Show("Name and Phone required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (tbPhone.Text.Length < 10)
            { MessageBox.Show("Phone number must be at least 10 digits.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            DataStore.Customers.Add(new Customer
            {
                Id = DataStore.NextCustId(), Name = tbName.Text.Trim(), Phone = tbPhone.Text.Trim(),
                Email = tbEmail.Text.Trim(), CNIC = tbCNIC.Text.Trim(), Address = tbAddress.Text.Trim()
            });
            MessageBox.Show("✅ Customer added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            foreach (Control c in new Control[] { tbName, tbPhone, tbEmail, tbCNIC, tbAddress }) ((TextBox)c).Clear();
            LoadGrid();
        }
    }
}
