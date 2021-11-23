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
    public partial class EditBroadcast : ModernDialog
    {
       
        public EditBroadcast()
        {
            try
            {
                InitializeComponent();

                var customersList = new List<Customer>();
                customersList.AddRange(DB.GetCustomersList());
                cmbCustomerName.DisplayMemberPath = "Name";
                cmbCustomerName.SelectedValuePath = "Id";
                cmbCustomerName.ItemsSource = customersList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("MainPage: {0} --- trace {1}", ex.Message, ex.StackTrace));
                Console.WriteLine("MainPage: " + ex.Message + "---- " + ex.StackTrace);
            }
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
           this.Close();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
           
        }
    }
}
