using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MasavBL;
using System.Collections;
using MasavBL.Models;
using MasavUI.Popups;
using System.Globalization;

namespace MasavUI.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PaymentsHistoryPage : UserControl
    {
        public List<PaymentHistory> payments { get; set; }

        public PaymentsHistoryPage()
        {
            try
            {
                InitializeComponent();

                //initialize comboBox:
                var customersList = new List<Customer>();
                customersList.Add(new Customer { Name = "כל הלקוחות", Id = 0 });
                customersList.AddRange(DB.GetCustomersList());
                cmbCustomers.DisplayMemberPath = "Name";
                cmbCustomers.SelectedValuePath = "Id";
                cmbCustomers.ItemsSource = customersList;
                cmbCustomers.SelectedIndex = 0;

                DataGridLoaded(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("MainPage: {0} --- trace {1}", ex.Message, ex.StackTrace));
                Console.WriteLine("MainPage: " + ex.Message + "---- " + ex.StackTrace);
            }
        }

        private void DataGridLoaded(object sender, RoutedEventArgs e)
        {
            payments = DB.GetPaymentHistory();
            dgReports.ItemsSource = payments;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            var customerId = (int)cmbCustomers.SelectedValue;
            var fromDate = dpFromDate.SelectedDate;
            var toDate = dpToDate.SelectedDate;
            payments = DB.GetPaymentHistory(customerId, fromDate, toDate);
            dgReports.ItemsSource = payments;
            dgReports.Items.Refresh();
        }
    }

}