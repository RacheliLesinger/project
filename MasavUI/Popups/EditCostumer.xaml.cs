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
    public partial class EditCustomer : ModernDialog
    {
       
        public EditCustomer()
        {
            try
            {
                InitializeComponent();

                var activitiesList = new List<Activity>();
                activitiesList.AddRange(DB.GetActivitiesList());
                cmbActivity.ItemsSource = activitiesList;
                cmbActivity.DisplayMemberPath = "Name";
                cmbActivity.SelectedValuePath = "Id";

                var institutionList = new List<Institution>();
                institutionList.AddRange(DB.GetInstitutionsList());
                cmbInstitution.ItemsSource = institutionList;
                cmbInstitution.DisplayMemberPath = "Name";
                cmbInstitution.SelectedValuePath = "Id";

                var paymentDateList = new List<int>();
                paymentDateList.AddRange(Enumerable.Range(1, 31));
                cmbPaymentDate1.ItemsSource = paymentDateList;
                cmbPaymentDate1.SelectedIndex = 0;
                cmbPaymentDate2.ItemsSource = paymentDateList;
                cmbPaymentDate2.SelectedIndex = 0;
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
