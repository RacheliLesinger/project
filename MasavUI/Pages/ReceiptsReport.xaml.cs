using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
//using CommonFunctions;
using FirstFloor.ModernUI.Windows;
//using GenerateExcels;
using MasavBL.Models;
using MasavBL;

namespace MasavUI.Pages
{
    /// <summary>
    /// Interaction logic for ReceiptsReport.xaml
    /// </summary>
    public partial class ReceiptsReport : UserControl, IContent
    {
        public bool OverrideFile = false;
        public ReceiptsReport()
        {
            InitializeComponent();

            //initialize comboBox
            var customersList = new List<Customer>();
            //  customersList.Add(new Customer { Name = "כל הלקוחות", Id = 0 });
            customersList.AddRange(DB.GetCustomersList());
            cmbCustomers.DisplayMemberPath = "Name";
            cmbCustomers.SelectedValuePath = "Id";
            cmbCustomers.ItemsSource = customersList;
            cmbCustomers.SelectedIndex = 0;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var customerId = (int)cmbCustomers.SelectedValue;
            btnCreateReport_Click(sender, e);
        }

        private async void btnCreateReport_Click(object sender, RoutedEventArgs e)
        {
            if (CheckRequierd() == true)
            {
                tbError.Visibility = Visibility.Collapsed;
                tbWaiting.Visibility = Visibility.Visible;
                mpbWaiting.Visibility = Visibility.Visible;
                GenerateReportRes res = null;
                if (rbReceipts.IsChecked == true)
                {
                    //res = await PdfReport.GenerateReceiptsReport(dpStartDate.SelectedDate.Value, dpEndDate.SelectedDate.Value,
                    //                        (int)cmbCustomers.SelectedValue, OverrideFile);
                }
                else
                {
                    res = await PdfReport.GenerateBroadcastReport(dpStartDate.SelectedDate.Value.Day,
                                            (int)cmbCustomers.SelectedValue, OverrideFile);
                }
                if (res.Success && res.FilePath != string.Empty)
                {
                    tbProgressSuccess.Text = "הדוח נוצר בהצלחה";
                    tbProgressSuccess.Visibility = Visibility.Visible;
                    tbWaiting.Visibility = Visibility.Collapsed;
                    mpbWaiting.Visibility = Visibility.Collapsed;
                    System.Diagnostics.Process.Start(res.FilePath);
                }
                else
                {
                    if (res.ErrorMessage == "File already Exsist")
                    {
                        tbProblematic.Visibility = Visibility.Visible;
                        spProblematic.Visibility = Visibility.Visible;
                        tbWaiting.Visibility = Visibility.Collapsed;
                        mpbWaiting.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private bool CheckRequierd()
        {
            if (dpStartDate.SelectedDate == null || dpEndDate.SelectedDate == null)
            {
                tbError.Text = "חובה לבחור תאריך התחלה ותאריך סיום ";
                if (cmbCustomers.SelectedValue == null)
                    tbError.Text += Environment.NewLine + "חובה לבחור לקוח ";
                tbError.Visibility = Visibility.Visible;
                return false;
            }
            else if (cmbCustomers.SelectedValue == null)
            {
                tbError.Text = "חובה לבחור לקוח ";
                tbError.Visibility = Visibility.Visible;
                return false;
            }
            return true;
        }

        private void btnCancalCreateReport_Click(object sender, RoutedEventArgs e)
        {
            var t = Application.Current;
            var c = t.MainWindow;


            MainWindow m = (MainWindow)c;
            m.NavigateToFirstTab();
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
            tbProgressSuccess.Visibility = Visibility.Collapsed;
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
        }

        private void btnForceCreateReport_Click(object sender, RoutedEventArgs e)
        {
            OverrideFile = true;
            btnCreateReport_Click(sender, e);
        }

        private void Brodcasts_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
