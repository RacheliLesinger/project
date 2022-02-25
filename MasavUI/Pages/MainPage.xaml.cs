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
using MasavUI.Popups;
using System.Globalization;

namespace MasavUI.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        public List<Paying> payings { get; set; }

        public MainPage()
        {
            try
            {
                InitializeComponent();

                btnEditPaying.IsEnabled = false;

                //initialize comboBox:
                var activitiesList = new List<Activity>();
                activitiesList.Add(new Activity { Name = "כל המצבים", Id = 0 });
                activitiesList.AddRange(DB.GetActivitiesList());
                cmbActivity.ItemsSource = activitiesList;
                cmbActivity.DisplayMemberPath = "Name";
                cmbActivity.SelectedValuePath = "Id";
                cmbActivity.SelectedIndex = 0;

                var customersList = new List<Customer>();
                customersList.Add(new Customer { Name = "כל הלקוחות", Id = 0 });
                customersList.AddRange(DB.GetCustomersList());
                cmbCustomers.DisplayMemberPath = "Name";
                cmbCustomers.SelectedValuePath = "Id";
                cmbCustomers.ItemsSource = customersList;
                cmbCustomers.SelectedIndex = 0;

                var classList = new List<int>();
                classList.AddRange(Enumerable.Range(0, 99));
                cmbClasses.ItemsSource = classList;
                cmbClasses.SelectedIndex = 0;

                DataGridLoaded(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("MainPage: {0} --- trace {1}", ex.Message, ex.StackTrace));
                Console.WriteLine("MainPage: " + ex.Message + "---- " + ex.StackTrace);
            }
        }

        private IEnumerable GetYears()
        {
            var y = DateTime.Now.Year;
            var list = new List<int>();
            for (int i = y - 5; i <= y + 5; i++)
            {
                list.Add(i);
            }
            return list;

        }

        private void DataGridLoaded(object sender, RoutedEventArgs e)
        {
            payings = DB.GetPayings();

            // ... Assign ItemsSource of DataGrid.
            //var grid = sender as DataGrid;

            dgReports.ItemsSource = payings;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            var customerId = (int)cmbCustomers.SelectedValue;
            var activityId = (int)cmbActivity.SelectedValue;
            var classId = (int)cmbClasses.SelectedValue;
            payings = DB.GetPayings(customerId, activityId,classId);
            dgReports.ItemsSource = payings;
            dgReports.Items.Refresh();

        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {

        }



        private void ExportToExcelAndCsv_Click(object sender, RoutedEventArgs e)
        {
            dgReports.SelectAll();
            List<Paying> payingList = dgReports.ItemsSource as List<Paying>;

            var res = ExcelReport.ExportToExcel(payingList);
            if (res.Success && res.FilePath != string.Empty)
            {
                MessageBox.Show("  הקובץ נוצר בהצלחה  \n ונשמר ב: \n" + res.FilePath);
            }
            else
            {
                MessageBox.Show("בעיה ביצירת האקסל");
            }
        }

        private void btnEditPaying_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EditPaying
            {
                Title = "עריכת פרטי משלם"
            };
            SetDialogEditPaymentData(dialog);

            Button customSaveButton = new Button() { Content = "שמירה", Margin = new Thickness(5) };
            customSaveButton.Click += (ss, ee) =>
               {
                   if (SavePaying(dialog))
                   {
                       dialog.Close();
                       btnFilter_Click(null, null);
                   }
               };
            
            Button customCancelButton = new Button() { Content = "ביטול" , Margin = new Thickness(5) };
            customCancelButton.Click += (ss, ee) => { dialog.Close(); };

            dialog.Buttons = new Button[] { customSaveButton , customCancelButton};
            dialog.Show();

            //EditPaying editDialog = new EditPaying();

            //if (editDialog.ShowDialog() == true)
            //    MessageBox.Show(" המשלם נקלט בהצלחה");
        }

        private void SetDialogEditPaymentData(EditPaying dialog)
        {
            Paying paying = (Paying)dgReports.SelectedItem;

            dialog.cmbCustomer.SelectedValue = paying.CustomerId;
            dialog.cmbCustomer.IsEnabled = false;
            dialog.cmbClass.SelectedValue = paying.Class;
            dialog.txtPaying.Text = paying.Name;
            dialog.txtPayingId.Text = paying.Id.ToString();
            dialog.txtPayingCode.Text = paying.IdentityNumber;
            dialog.cmbCurrency.SelectedValue = paying.CurrencyId;
            dialog.cmbActivity.SelectedValue = paying.ActivityId;
            dialog.cmbCodeBank.SelectedValue = paying.CodeBankId;
            dialog.dpStartDate.SelectedDate = paying.StartDate;
            dialog.dpEndDate.SelectedDate = paying.EndDate;
            dialog.cmbPaymentDate.SelectedValue = paying.PaymentDate;
            dialog.txtAmount.Text = paying.Amount.ToString();
            dialog.txtBankAccountNumber.Text = paying.BankAccountNumber;
            dialog.txtBankBranchNumber.Text = paying.BankBranchNumber;
            dialog.txtPaymentSum.Text = paying.PaymentSum.ToString();
        }

        private bool SavePaying(EditPaying dialog)
        {
            if (Validation.GetErrors(dialog.cmbCustomer).Count > 0 || Validation.GetErrors(dialog.txtPaying).Count > 0 ||
                Validation.GetErrors(dialog.txtPayingId).Count > 0 || Validation.GetErrors(dialog.cmbCurrency).Count > 0 ||
                Validation.GetErrors(dialog.dpStartDate).Count > 0 || Validation.GetErrors(dialog.dpEndDate).Count > 0 ||
                Validation.GetErrors(dialog.cmbCodeBank).Count > 0 || Validation.GetErrors(dialog.cmbPaymentDate).Count > 0 ||
                Validation.GetErrors(dialog.txtPayingCode).Count > 0 || Validation.GetErrors(dialog.cmbActivity).Count > 0||
                Validation.GetErrors(dialog.txtBankAccountNumber).Count > 0 || Validation.GetErrors(dialog.txtAmount).Count > 0||
                Validation.GetErrors(dialog.txtBankBranchNumber).Count > 0 || Validation.GetErrors(dialog.txtPaymentSum).Count > 0)
            {
                return false;
            }
            Paying paying = new Paying();

            paying.CustomerId = (int)dialog.cmbCustomer.SelectedValue;
            paying.Class = (int)dialog.cmbClass.SelectedValue;
            paying.Name = dialog.txtPaying.Text;
            paying.Id = Int32.Parse(dialog.txtPayingId.Text);
            paying.IdentityNumber = dialog.txtPayingCode.Text;
            paying.CurrencyId = (int)dialog.cmbCurrency.SelectedValue;
            paying.ActivityId = (int)dialog.cmbActivity.SelectedValue;
            paying.CodeBankId = (int)dialog.cmbCodeBank.SelectedValue;
            paying.StartDate = dialog.dpStartDate.SelectedDate;
            paying.EndDate = dialog.dpEndDate.SelectedDate;
            paying.PaymentDate = (int)dialog.cmbPaymentDate.SelectedValue;
            paying.Amount = Double.Parse(dialog.txtAmount.Text);
            paying.BankAccountNumber = dialog.txtBankAccountNumber.Text;
            paying.BankBranchNumber = dialog.txtBankBranchNumber.Text;
            paying.PaymentSum = Int32.Parse(dialog.txtPaymentSum.Text);

            DB.UpdatePayings(paying);
            return true;
        }

        private void dgReports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEditPaying.IsEnabled = true;
        }

        private void btnNewPaying_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EditPaying
            {
                Title = " יצירת משלם חדש"
            };

            if ((int)cmbCustomers.SelectedValue != 0)
            {
                dialog.cmbCustomer.SelectedValue = (int)cmbCustomers.SelectedValue;
                dialog.cmbPaymentDate.SelectedValue = ((Customer)cmbCustomers.SelectedItem).PaymentDate1;
            }
            dialog.cmbActivity.SelectedValue = 1; //פעיל
            dialog.cmbCurrency.SelectedValue = 1; //שקל
            dialog.cmbClass.SelectedValue = 0;
            dialog.dpStartDate.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dialog.dpEndDate.SelectedDate = DateTime.MaxValue;
            dialog.txtPaymentSum.Text = "-1";

            Button customSaveButton = new Button() { Content = "שמירה", Margin = new Thickness(5) };
            customSaveButton.Click += (ss, ee) =>
            {
                dialog.txtPayingId.Text = "0";
                if (SavePaying(dialog))
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

        private void CmbCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var classList = new List<int>();
            var customerId = (int)cmbCustomers.SelectedValue;
            classList.AddRange(DB.GetClassesToCustomer(customerId));

            cmbClasses.ItemsSource = classList;
            cmbClasses.SelectedIndex = 0;
        }
    }
   
}