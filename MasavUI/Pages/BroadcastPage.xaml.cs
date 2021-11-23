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
    public partial class BroadcastPage : UserControl
    {
        public List<BroadcastHistory> broadcasts { get; set; }

        public BroadcastPage()
        {
            try
            {
                InitializeComponent();

                btnEditCostumer.IsEnabled = false;

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
            broadcasts = DB.GetBroadcastHistoryList();
            dgReports.ItemsSource = broadcasts;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            var customerId = (int)cmbCustomers.SelectedValue;
            broadcasts = DB.GetBroadcastHistoryList(customerId);
            dgReports.ItemsSource = broadcasts;
            dgReports.Items.Refresh();
        }

        private void btnEditBroadcast_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EditBroadcast
            {
                Title = "עדכון דוח שידורים"
            };

            SetDialogEditBroadcastData(dialog);

            Button customSaveButton = new Button() { Content = "שמירה", Margin = new Thickness(5) };
            customSaveButton.Click += (ss, ee) =>
               {
                   if (SaveBC(dialog))
                   {
                       dialog.Close();
                       btnFilter_Click(null, null);
                   }
               };

            Button customCancelButton = new Button() { Content = "ביטול", Margin = new Thickness(5) };
            customCancelButton.Click += (ss, ee) => { dialog.Close(); };

            dialog.Buttons = new Button[] { customSaveButton, customCancelButton };
            dialog.Show();
        }

        private void SetDialogEditBroadcastData(EditBroadcast dialog)
        {
            BroadcastHistory bc = (BroadcastHistory)dgReports.SelectedItem;

            dialog.cmbCustomerName.SelectedValue = bc.CustomerId;
            dialog.txtBroadcastId.Text = bc.Id.ToString();
            dialog.txtBroadcastReference.Text = bc.BroadcastReference;
            dialog.txtBroadcastAmount.Text = bc.BroadcastAmount?.ToString();
            dialog.dpBroadcastDate.SelectedDate = bc.BroadcastDate;
            dialog.dpValueDate.SelectedDate = bc.ValueDate;
            dialog.txtNotes.Text = bc.Notes;
            dialog.txtSumNewRecords.Text = bc.SumNewRecords?.ToString();
            dialog.txtSumRecords.Text = bc.SumRecords?.ToString();
            
        }

        private bool SaveBC(EditBroadcast dialog)
        {
            BroadcastHistory broadcast = new BroadcastHistory();

            broadcast.Id = Int32.Parse(dialog.txtBroadcastId.Text);
            broadcast.CustomerId = (int)dialog.cmbCustomerName.SelectedValue;
            broadcast.BroadcastReference = dialog.txtBroadcastReference.Text;
            Int32.TryParse(dialog.txtBroadcastAmount.Text, out int res);
            broadcast.BroadcastAmount = res;
            broadcast.BroadcastDate = dialog.dpBroadcastDate.SelectedDate;
            broadcast.ValueDate = dialog.dpValueDate.SelectedDate;
            broadcast.Notes = dialog.txtNotes.Text;
            Int32.TryParse(dialog.txtSumRecords.Text, out int res2);
            broadcast.SumRecords = res2;
            Int32.TryParse(dialog.txtSumNewRecords.Text, out int res3);
            broadcast.SumNewRecords = res3;

            DB.UpdateBroadcast(broadcast);
            return true;
        }

        private void dgReports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEditCostumer.IsEnabled = true;
        }
    }

}