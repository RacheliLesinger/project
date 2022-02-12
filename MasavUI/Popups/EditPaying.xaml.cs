using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MasavBL;
using System.Collections;
using System.Data;
using System.Xml.Linq;
using System.Diagnostics;
using Microsoft.Win32;
using MasavBL.Models;

namespace MasavUI.Popups
{
    public partial class EditPaying : ModernDialog
    {
       
        public EditPaying()
        {
            try
            {
                InitializeComponent();

                var activitiesList = new List<Activity>();
                activitiesList.AddRange(DB.GetActivitiesList());
                cmbActivity.ItemsSource = activitiesList;
                cmbActivity.DisplayMemberPath = "Name";
                cmbActivity.SelectedValuePath = "Id";

                var customersList = new List<Customer>();
                customersList.AddRange(DB.GetCustomersList());
                cmbCustomer.DisplayMemberPath = "Name";
                cmbCustomer.SelectedValuePath = "Id";
                cmbCustomer.ItemsSource = customersList;

                var banksList = new List<CodeBank>();
                banksList.AddRange(DB.GetBanksList());
                cmbCodeBank.DisplayMemberPath = "Name";
                cmbCodeBank.SelectedValuePath = "Id";
                cmbCodeBank.ItemsSource = banksList;

                var currencyList = new List<Currency>();
                currencyList.AddRange(DB.GetCurrencyList());
                cmbCurrency.DisplayMemberPath = "Name";
                cmbCurrency.SelectedValuePath = "Id";
                cmbCurrency.ItemsSource = currencyList;

                var paymentDateList = new List<int>();
                paymentDateList.AddRange(Enumerable.Range(1, 31));
                cmbPaymentDate.ItemsSource = paymentDateList;
                cmbPaymentDate.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("MainPage: {0} --- trace {1}", ex.Message, ex.StackTrace));
                Console.WriteLine("MainPage: " + ex.Message + "---- " + ex.StackTrace);
            }
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
           //this.DialogResult = true;
           this.Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
           
        }
    }
}
