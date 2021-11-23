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
    public partial class CostumersPage : UserControl
    {
        public List<Customer> customers { get; set; }

        public CostumersPage()
        {
            try
            {
                InitializeComponent();

                btnEditCostumer.IsEnabled = false;

                //initialize comboBox:
                var activitiesList = new List<Activity>();
                activitiesList.Add(new Activity { Name = "כל המצבים", Id = 0 });
                activitiesList.AddRange(DB.GetActivitiesList());
                cmbActivity.ItemsSource = activitiesList;
                cmbActivity.DisplayMemberPath = "Name";
                cmbActivity.SelectedValuePath = "Id";
                cmbActivity.SelectedIndex = 0;

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
            customers = DB.GetCustomers();
            dgReports.ItemsSource = customers;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            var activityId = (int)cmbActivity.SelectedValue;
            customers = DB.GetCustomers(activityId);
            dgReports.ItemsSource = customers;
            dgReports.Items.Refresh();
        }

        private void ExportToExcelAndCsv_Click(object sender, RoutedEventArgs e)
        {
            dgReports.SelectAll();
            var res = ExcelReport.ExportCostumersToExcel();
            if (res.Success && res.FilePath != string.Empty)
            {
                MessageBox.Show("  הקובץ נוצר בהצלחה  \n ונשמר ב: \n" + res.FilePath);
            }
            else
            {
                MessageBox.Show("בעיה ביצירת האקסל");
            }
        }

        private void btnEditCostumer_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EditCustomer
            {
                Title = "עריכת פרטי לקוח"
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
        }

        private void SetDialogEditPaymentData(EditCustomer dialog)
        {
            Customer customer = (Customer)dgReports.SelectedItem;

            dialog.txtCustomerName.Text = customer.Name;
            dialog.txtCustomerId.Text = customer.Id.ToString();
            dialog.txtCustomerCode.Text = customer.Code;
            dialog.cmbActivity.SelectedValue = customer.ActivityId;
            dialog.dpCreatedDate.SelectedDate = customer.CreatedDate;
            dialog.cmbPaymentDate1.SelectedValue = customer.PaymentDate1;
            dialog.cmbPaymentDate2.SelectedValue = customer.PaymentDate2;
            dialog.txtContact.Text = customer.Contact;
            dialog.txtAddress.Text = customer.Address;
            dialog.txtPhone.Text = customer.Phone;
            dialog.txtEmail.Text = customer.Email;
        }

        private bool SavePaying(EditCustomer dialog)
        {
            if(Validation.GetErrors(dialog.txtCustomerName).Count > 0 || Validation.GetErrors(dialog.txtAddress).Count > 0 ||
                Validation.GetErrors(dialog.cmbPaymentDate1).Count > 0 || Validation.GetErrors(dialog.dpCreatedDate).Count > 0 ||
                Validation.GetErrors(dialog.cmbActivity).Count > 0 || Validation.GetErrors(dialog.txtCustomerCode).Count > 0)
            {
                return false;
            }
            Customer customer = new Customer();

            customer.Id = Int32.Parse(dialog.txtCustomerId.Text);
            customer.Name = dialog.txtCustomerName.Text;
            customer.Code = dialog.txtCustomerCode.Text;
            customer.ActivityId = (int)dialog.cmbActivity.SelectedValue;

            customer.PaymentDate1 = (int)dialog.cmbPaymentDate1.SelectedValue;
            Int32.TryParse(dialog.cmbPaymentDate2.SelectedValue?.ToString(), out int res2);
            customer.PaymentDate2 = res2;
            customer.CreatedDate = dialog.dpCreatedDate.SelectedDate;
            customer.Contact = dialog.txtContact.Text;
            customer.Email = dialog.txtEmail.Text;
            customer.Address = dialog.txtAddress.Text;
            customer.Phone = dialog.txtPhone.Text;

            DB.UpdateCustomers(customer);
            return true;
        }

        private void dgReports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEditCostumer.IsEnabled = true;
        }

        private void btnNewCostumer_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EditCustomer
            {
                Title = " יצירת מוסד חדש"
            };

            Button customSaveButton = new Button() { Content = "שמירה", Margin = new Thickness(5) };
            customSaveButton.Click += (ss, ee) =>
            {
                dialog.txtCustomerId.Text = "0";
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
    }

    public class RequiredValidationRule : ValidationRule
    {
        public static string GetErrorMessage(string fieldName, object fieldValue, object nullValue = null)
        {
            string errorMessage = string.Empty;
            if (nullValue != null && nullValue.Equals(fieldValue))
                errorMessage = string.Format("{0} נדרש", fieldName);
            if (fieldValue == null || string.IsNullOrEmpty(fieldValue.ToString()))
                errorMessage = string.Format(" {0} נדרש", fieldName);
            return errorMessage;
        }
        public string FieldName { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string error = GetErrorMessage(FieldName, value);
            if (!string.IsNullOrEmpty(error))
                return new ValidationResult(false, error);
            return ValidationResult.ValidResult;
        }

    }
}