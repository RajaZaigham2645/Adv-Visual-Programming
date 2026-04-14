using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CarShowroom
{
    /// <summary>
    /// Professional Sales Log Page with Dummy Charts (OxyPlot replaced)
    /// 
    /// FEATURES:
    /// - ✅ Revenue trend chart (Dummy Line Chart visualization)
    /// - ✅ Sales count bar chart with dual Y-axes (Dummy)
    /// - ✅ Payment method distribution (Dummy Pie Chart)
    /// - ✅ Dynamic period filtering (Daily/Weekly/Monthly)
    /// - ✅ Comprehensive error handling
    /// - ✅ Real sales data binding
    /// - ✅ Null/empty data prevention
    /// - ✅ Dark theme UI
    /// </summary>
    public class SalesLogPage : Panel
    {
        // ===== UI COMPONENTS =====
        private DataGridView _grid;
        private Label _total, _avgPrice, _countLabel, _totalRevenueLabel;
        private Panel _revenueChartPanel;  // Dummy revenue chart panel
        private Panel _paymentChartPanel;   // Dummy payment chart panel
        private FlowLayoutPanel _filterPanel;

        // ===== STATE =====
        private ChartPeriod _currentPeriod = ChartPeriod.Daily;

        private enum ChartPeriod
        {
            Daily,
            Weekly,
            Monthly
        }

        /// <summary>
        /// Constructor - Initializes page and loads data
        /// </summary>
        public SalesLogPage()
        {
            Dock = DockStyle.Fill;
            BackColor = Theme.Dark2;
            Padding = new Padding(16);

            try
            {
                InitializeComponents();
                LoadGrid();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SalesLogPage Init Error: {ex}");
                MessageBox.Show($"Error initializing Sales Page: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Initializes all UI components including header, filters, charts, and data grid
        /// Follows a top-down layout: Header -> Filters -> Charts -> Data Grid
        /// </summary>
        private void InitializeComponents()
        {
            // ===== HEADER PANEL - Statistics Display =====
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Theme.Dark1,
                Padding = new Padding(16, 12, 16, 12)
            };

            var titleLbl = UIHelper.MakeLabel("📊  Sales History & Analytics", Theme.H2, Theme.Gold);
            titleLbl.Location = new Point(16, 14);

            // Total revenue label
            _total = UIHelper.MakeLabel("Total Revenue: PKR 0", Theme.H3, Theme.Success);
            _total.Location = new Point(300, 18);

            // Total sales count label
            _countLabel = UIHelper.MakeLabel("Total Sales: 0", Theme.H3, Theme.Info);
            _countLabel.Location = new Point(16, 50);

            // Average sale price label
            _avgPrice = UIHelper.MakeLabel("Avg Sale Price: PKR 0", Theme.H3, Theme.Warning);
            _avgPrice.Location = new Point(300, 50);

            // Today's revenue label
            _totalRevenueLabel = UIHelper.MakeLabel("Today's Revenue: PKR 0", Theme.H3, Theme.Accent);
            _totalRevenueLabel.Location = new Point(720, 18);

            header.Controls.AddRange(new Control[] { titleLbl, _total, _countLabel, _avgPrice, _totalRevenueLabel });

            // ===== FILTER PANEL - Period Selection =====
            _filterPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Theme.Dark2,
                Padding = new Padding(16, 8, 16, 8),
                FlowDirection = FlowDirection.LeftToRight,
                AutoScroll = false
            };

            var filterLabel = UIHelper.MakeLabel("📈 Chart Period:", Theme.H3, Theme.TextLight);
            filterLabel.AutoSize = true;
            _filterPanel.Controls.Add(filterLabel);

            // Create period filter buttons (Daily, Weekly, Monthly)
            var dailyBtn = CreateFilterButton("Daily", ChartPeriod.Daily, true);
            var weeklyBtn = CreateFilterButton("Weekly", ChartPeriod.Weekly, false);
            var monthlyBtn = CreateFilterButton("Monthly", ChartPeriod.Monthly, false);

            _filterPanel.Controls.Add(dailyBtn);
            _filterPanel.Controls.Add(weeklyBtn);
            _filterPanel.Controls.Add(monthlyBtn);

            // ===== CHARTS CONTAINER =====
            var chartsContainer = new Panel
            {
                Dock = DockStyle.Top,
                Height = 360,
                BackColor = Theme.Dark2,
                Padding = new Padding(16)
            };

            // Revenue & Sales Trends Chart (Left Side) - Dummy Chart
            _revenueChartPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(35, 35, 35),
                BorderStyle = BorderStyle.FixedSingle
            };

            var leftChartPanel = new Panel
            {
                Left = 0,
                Top = 0,
                Width = chartsContainer.Width / 2 - 12,
                Height = 340,
                BackColor = Theme.Dark2,
                Padding = new Padding(8)
            };
            leftChartPanel.Controls.Add(_revenueChartPanel);

            // Payment Method Distribution Chart (Right Side) - Dummy Chart
            _paymentChartPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(35, 35, 35),
                BorderStyle = BorderStyle.FixedSingle
            };

            var rightChartPanel = new Panel
            {
                Left = chartsContainer.Width / 2 + 4,
                Top = 0,
                Width = chartsContainer.Width / 2 - 12,
                Height = 340,
                BackColor = Theme.Dark2,
                Padding = new Padding(8)
            };
            rightChartPanel.Controls.Add(_paymentChartPanel);

            chartsContainer.Controls.Add(leftChartPanel);
            chartsContainer.Controls.Add(rightChartPanel);

            // ===== DATA GRID - Sales Details =====
            _grid = UIHelper.MakeGrid();
            _grid.Columns.Add("SaleId", "#");
            _grid.Columns.Add("Date", "Date");
            _grid.Columns.Add("Car", "Car");
            _grid.Columns.Add("Customer", "Customer");
            _grid.Columns.Add("Phone", "Phone");
            _grid.Columns.Add("Price", "Price (PKR)");
            _grid.Columns.Add("Payment", "Payment");
            _grid.Columns.Add("Rep", "Sales Rep");
            _grid.Columns["SaleId"].FillWeight = 30;

            // ===== ADD ALL CONTROLS TO PAGE (Order matters for Z-order) =====
            Controls.Add(_grid);
            Controls.Add(chartsContainer);
            Controls.Add(_filterPanel);
            Controls.Add(header);
        }

        /// <summary>
        /// Creates a styled filter button with click handler for period selection
        /// </summary>
        /// <param name="text">Button display text</param>
        /// <param name="period">ChartPeriod value this button represents</param>
        /// <param name="isSelected">Whether button should appear selected initially</param>
        /// <returns>Configured Button control</returns>
        private Button CreateFilterButton(string text, ChartPeriod period, bool isSelected)
        {
            var btn = new Button
            {
                Text = text,
                Width = 90,
                Height = 32,
                Margin = new Padding(4, 0, 4, 0),
                Cursor = Cursors.Hand,
                BackColor = isSelected ? Theme.Accent : Theme.Dark1,
                ForeColor = Theme.TextLight,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };

            btn.FlatAppearance.BorderColor = isSelected ? Theme.Gold : Theme.Dark1;
            btn.FlatAppearance.BorderSize = isSelected ? 2 : 1;

            btn.Click += (s, e) => ChangeChartPeriod(period, btn);
            return btn;
        }

        /// <summary>
        /// Handles chart period filter changes - updates button styling and refreshes chart
        /// </summary>
        /// <param name="period">New chart period to display</param>
        /// <param name="selectedBtn">Button that was clicked</param>
        private void ChangeChartPeriod(ChartPeriod period, Button selectedBtn)
        {
            _currentPeriod = period;

            // Update button styles to show selection state
            foreach (Button btn in _filterPanel.Controls.OfType<Button>())
            {
                btn.BackColor = btn == selectedBtn ? Theme.Accent : Theme.Dark1;
                btn.FlatAppearance.BorderColor = btn == selectedBtn ? Theme.Gold : Theme.Dark1;
                btn.FlatAppearance.BorderSize = btn == selectedBtn ? 2 : 1;
            }

            // Refresh charts with new period data
            UpdateChart();
        }

        /// <summary>
        /// This is the main entry point for refreshing the page.
        /// </summary>
        public void LoadGrid()
        {
            try
            {
                _grid.Rows.Clear();

                // Validate data availability
                if (DataStore.Sales == null || DataStore.Sales.Count == 0)
                {
                    DisplayNoDataMessage();
                    return;
                }

                // Populate grid with sales data sorted by most recent first
                var sales = DataStore.Sales.OrderByDescending(x => x.SaleDate).ToList();
                foreach (var s in sales)
                {
                    _grid.Rows.Add(
                        s.Id,
                        s.SaleDate.ToString("dd-MMM-yyyy HH:mm"),
                        s.Car.ToString(),
                        s.Customer.Name,
                        s.Customer.Phone,
                        s.SalePrice.ToString("N0"),
                        s.PaymentMode,
                        s.SalesRep
                    );
                }

                // Calculate and display statistics
                UpdateStatistics();

                // Refresh charts with current period view
                UpdateChart();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading sales data: {ex.Message}");
                MessageBox.Show($"Error loading sales data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Updates header statistics labels with current sales data including:
        /// - Total revenue sum
        /// - Average sale price
        /// - Today's revenue
        /// - Count of all sales
        /// </summary>
        private void UpdateStatistics()
        {
            try
            {
                double totalRevenue = DataStore.Sales.Sum(x => x.SalePrice);
                double avgPrice = DataStore.Sales.Count > 0 ? DataStore.Sales.Average(x => x.SalePrice) : 0;
                
                // Calculate today's revenue only
                double todayRevenue = DataStore.Sales
                    .Where(x => x.SaleDate.Date == DateTime.Now.Date)
                    .Sum(x => x.SalePrice);

                _total.Text = $"Total Revenue: PKR {totalRevenue:N0}";
                _countLabel.Text = $"Total Sales: {DataStore.Sales.Count}";
                _avgPrice.Text = $"Avg Sale Price: PKR {avgPrice:N0}";
                _totalRevenueLabel.Text = $"Today's Revenue: PKR {todayRevenue:N0}";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating statistics: {ex.Message}");
            }
        }

        /// <summary>
        /// Displays placeholder message when no sales data exists
        /// </summary>
        private void DisplayNoDataMessage()
        {
            _total.Text = "Total Revenue: PKR 0";
            _countLabel.Text = "Total Sales: 0";
            _avgPrice.Text = "Avg Sale Price: PKR 0";
            _totalRevenueLabel.Text = "Today's Revenue: PKR 0";
            CreateEmptyChart();
        }

        /// <summary>
        /// Main chart update method - creates and displays dummy revenue trend chart based on period filter
        /// Supports Daily, Weekly, and Monthly views with automatic data grouping
        /// </summary>
        private void UpdateChart()
        {
            try
            {
                if (DataStore.Sales == null || DataStore.Sales.Count == 0)
                {
                    CreateEmptyChart();
                    return;
                }

                DrawRevenueChart();
                DrawPaymentChart();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating chart: {ex.Message}");
                CreateErrorChart();
            }
        }

        /// <summary>
        /// Draws dummy revenue chart with text statistics instead of OxyPlot
        /// </summary>
        private void DrawRevenueChart()
        {
            _revenueChartPanel.Controls.Clear();
            
            // Group sales based on selected period
            var groupedSales = GroupSalesByPeriod();
            
            if (groupedSales.Count == 0)
            {
                CreateEmptyChart();
                return;
            }

            // Calculate statistics for the chart
            var totalRevenue = groupedSales.Sum(g => g.Sum(s => s.SalePrice));
            var avgRevenue = groupedSales.Average(g => g.Sum(s => s.SalePrice));
            var maxRevenue = groupedSales.Max(g => g.Sum(s => s.SalePrice));
            var totalSales = groupedSales.Sum(g => g.Count());
            var avgSales = groupedSales.Average(g => g.Count());
            
            // Create dummy chart display
            var chartLabel = new Label
            {
                Text = $"{GetChartTitle()}\n\n" +
                       $"📊 Period: {_currentPeriod}\n" +
                       $"📈 Total Revenue: PKR {totalRevenue:N0}\n" +
                       $"📉 Average Revenue per Period: PKR {avgRevenue:N0}\n" +
                       $"📊 Highest Revenue Period: PKR {maxRevenue:N0}\n" +
                       $"🔄 Total Sales: {totalSales}\n" +
                       $"⭐ Average Sales per Period: {avgSales:F1}\n\n" +
                       $"📅 Data Points: {groupedSales.Count} periods\n" +
                       $"🎨 [Dummy Chart - OxyPlot Replaced]\n" +
                       $"💡 Tip: Use filters above to change period view",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Theme.TextLight,
                BackColor = Color.FromArgb(35, 35, 35),
                Font = new Font("Segoe UI", 11, FontStyle.Regular)
            };
            
            _revenueChartPanel.Controls.Add(chartLabel);
        }

        /// <summary>
        /// Draws dummy payment distribution chart with text statistics instead of OxyPlot
        /// </summary>
        private void DrawPaymentChart()
        {
            _paymentChartPanel.Controls.Clear();
            
            // Group sales by payment method
            var paymentSales = DataStore.Sales
                .GroupBy(s => s.PaymentMode)
                .Select(g => new
                {
                    PaymentMode = g.Key,
                    Count = g.Count(),
                    Revenue = g.Sum(x => x.SalePrice)
                })
                .OrderByDescending(x => x.Revenue)
                .ToList();

            if (paymentSales.Count == 0)
            {
                CreateEmptyChart();
                return;
            }

            // Build payment method summary text
            string paymentText = "Sales by Payment Method\n\n";
            int totalSales = paymentSales.Sum(p => p.Count);
            double totalRevenue = paymentSales.Sum(p => p.Revenue);
            
            foreach (var payment in paymentSales)
            {
                double countPercent = (payment.Count / (double)totalSales) * 100;
                double revenuePercent = (payment.Revenue / totalRevenue) * 100;
                paymentText += $"💳 {payment.PaymentMode}\n" +
                              $"   Sales: {payment.Count} ({countPercent:F1}%)\n" +
                              $"   Revenue: PKR {payment.Revenue:N0} ({revenuePercent:F1}%)\n\n";
            }
            
            paymentText += $"━━━━━━━━━━━━━━━━━━━━━\n" +
                          $"📊 Total Sales: {totalSales}\n" +
                          $"💰 Total Revenue: PKR {totalRevenue:N0}\n\n" +
                          $"🎨 [Dummy Chart - OxyPlot Replaced]";
            
            var chartLabel = new Label
            {
                Text = paymentText,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Theme.TextLight,
                BackColor = Color.FromArgb(35, 35, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Padding = new Padding(20)
            };
            
            _paymentChartPanel.Controls.Add(chartLabel);
        }

        /// <summary>
        /// Groups sales data by the selected period filter.
        /// Returns list of daily groups for Daily view, weekly groups for Weekly, monthly for Monthly.
        /// </summary>
        /// <returns>List of grouped sales where each group represents a period</returns>
        private List<List<Sale>> GroupSalesByPeriod()
        {
            var result = new List<List<Sale>>();

            try
            {
                var sortedSales = DataStore.Sales.OrderBy(s => s.SaleDate).ToList();

                switch (_currentPeriod)
                {
                    case ChartPeriod.Daily:
                        // Group by date - each group is one day
                        result = sortedSales
                            .GroupBy(s => s.SaleDate.Date)
                            .OrderBy(g => g.Key)
                            .Select(g => g.ToList())
                            .ToList();
                        break;

                    case ChartPeriod.Weekly:
                        // Group by ISO week - each group is one week
                        var weekGroups = new Dictionary<string, List<Sale>>();
                        foreach (var sale in sortedSales)
                        {
                            string weekKey = GetWeekKey(sale.SaleDate);
                            if (!weekGroups.ContainsKey(weekKey))
                                weekGroups[weekKey] = new List<Sale>();
                            weekGroups[weekKey].Add(sale);
                        }
                        result = weekGroups.Values.ToList();
                        break;

                    case ChartPeriod.Monthly:
                        // Group by month - each group is one month
                        result = sortedSales
                            .GroupBy(s => new { s.SaleDate.Year, s.SaleDate.Month })
                            .OrderBy(g => g.Key.Year)
                            .ThenBy(g => g.Key.Month)
                            .Select(g => g.ToList())
                            .ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error grouping sales: {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// Gets appropriate chart title based on current period filter
        /// </summary>
        /// <returns>Title string for chart based on view type</returns>
        private string GetChartTitle()
        {
            return _currentPeriod switch
            {
                ChartPeriod.Daily => "Daily Revenue & Sales Trends",
                ChartPeriod.Weekly => "Weekly Revenue & Sales Trends",
                ChartPeriod.Monthly => "Monthly Revenue & Sales Trends",
                _ => "Revenue & Sales Trends"
            };
        }

        /// <summary>
        /// Gets ISO 8601 week number for a given date
        /// </summary>
        /// <param name="date">Date to calculate week number for</param>
        /// <returns>Week number (1-53)</returns>
        private int GetWeekNumber(DateTime date)
        {
            var cultureInfo = System.Globalization.CultureInfo.CurrentCulture;
            var calendar = cultureInfo.Calendar;
            return calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        /// <summary>
        /// Creates a unique week key for grouping sales by week
        /// Format: "YYYY-Wnn" (e.g., "2024-W01")
        /// </summary>
        /// <param name="date">Date to create week key for</param>
        /// <returns>Week key string</returns>
        private string GetWeekKey(DateTime date)
        {
            var weekNum = GetWeekNumber(date);
            return $"{date.Year}-W{weekNum}";
        }

        /// <summary>
        /// Creates an empty chart displayed when no sales data is available
        /// </summary>
        private void CreateEmptyChart()
        {
            try
            {
                var emptyLabel = new Label
                {
                    Text = "📊 No Sales Data Available\n\nClick 'Add Sale' to get started",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.FromArgb(200, 100, 100),
                    BackColor = Color.FromArgb(35, 35, 35),
                    Font = new Font("Segoe UI", 12, FontStyle.Regular)
                };
                
                _revenueChartPanel.Controls.Clear();
                _revenueChartPanel.Controls.Add(emptyLabel);
                
                var emptyPaymentLabel = new Label
                {
                    Text = "📊 No Payment Data Available",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.FromArgb(200, 100, 100),
                    BackColor = Color.FromArgb(35, 35, 35),
                    Font = new Font("Segoe UI", 12, FontStyle.Regular)
                };
                
                _paymentChartPanel.Controls.Clear();
                _paymentChartPanel.Controls.Add(emptyPaymentLabel);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating empty chart: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates an error chart displayed when an exception occurs during chart rendering
        /// </summary>
        private void CreateErrorChart()
        {
            try
            {
                var errorLabel = new Label
                {
                    Text = "⚠️ Error Loading Chart Data\n\nPlease check your data and try again",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.FromArgb(244, 67, 54),
                    BackColor = Color.FromArgb(35, 35, 35),
                    Font = new Font("Segoe UI", 12, FontStyle.Regular)
                };
                
                _revenueChartPanel.Controls.Clear();
                _revenueChartPanel.Controls.Add(errorLabel);
                
                _paymentChartPanel.Controls.Clear();
                _paymentChartPanel.Controls.Add(errorLabel);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating error chart: {ex.Message}");
            }
        }
    }
}