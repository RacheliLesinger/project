using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using CommonFunctions;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
//using GenerateExcels;
using System.Threading;
using System.Net;
using System.IO;
using Microsoft.Win32;
using MasavBL.Models;
using MasavBL;

namespace MasavUI.Pages
{
    public partial class ImportDateFromExcel : UserControl, IContent
    {
        public bool OverrideFile = false;
        public ImportDateFromExcel()
        {
            InitializeComponent();

            //initialize comboBox:          
            var customersList = new List<Customer>();
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
            tbProblematic.Visibility = Visibility.Collapsed;
            spProblematic.Visibility = Visibility.Collapsed;
            tbWaiting.Visibility = Visibility.Visible;
            mpbWaiting.Visibility = Visibility.Visible;

            // Create OpenFileDialog 
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".xlsx";
            dlg.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                var f = dlg.OpenFile();
            }
            //var res = await GenarateReport.GenerateReport(cmbYear.SelectedValue.ToString(), cmbMonthly.SelectedValue.ToString()
            //                                     ,(int)cmbDayInMonth.SelectedValue, (int)cmbCustomers.SelectedValue,OverrideFile);
            //if (res.Success && res.FilePath != string.Empty)
            {
                tbProgressSuccess.Text = "התהליך הסתיים בהצלחה" + System.Environment.NewLine;
                tbProgressSuccess.Visibility = Visibility.Visible;
                tbWaiting.Visibility = Visibility.Collapsed;
                mpbWaiting.Visibility = Visibility.Collapsed;
            }
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
    }
}
