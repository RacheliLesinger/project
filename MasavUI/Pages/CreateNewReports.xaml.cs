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




    /// <summary>
    /// Interaction logic for CreateNewReports.xaml
    /// </summary>
    public partial class CreateNewReports : UserControl, IContent
    {
        public bool OverrideFile = false;
        public CreateNewReports()
        {
            InitializeComponent();

            //initialize comboBox:
            cmbYear.ItemsSource = GetYears();
            cmbYear.SelectedItem = DateTime.Now.Year;

            var monthList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            cmbMonthly.ItemsSource = monthList;
            cmbMonthly.SelectedItem = DateTime.Now.Month;

            var dayInMonthList = new List<int>();
            dayInMonthList.AddRange(Enumerable.Range(1, 31).ToArray());
            cmbDayInMonth.ItemsSource = dayInMonthList;
            cmbDayInMonth.SelectedIndex = 0;

            var classList = new List<int>();
            classList.AddRange(Enumerable.Range(0, 99).ToArray());
            cmbClass.ItemsSource = classList;
            cmbClass.SelectedIndex = 0;

            var customersList = new List<Customer>();
            customersList.AddRange(DB.GetCustomersList());
            cmbCustomers.DisplayMemberPath = "Name";
            cmbCustomers.SelectedValuePath = "Id";
            cmbCustomers.ItemsSource = customersList;
            cmbCustomers.SelectedIndex = 0;
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

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            var year = cmbYear.SelectedItem != null ? (int)(cmbYear.SelectedItem) : DateTime.Now.Year;
            var month = (int)cmbMonthly.SelectedValue;
            var customerId = (int)cmbCustomers.SelectedValue;
            btnCreateReport_Click(sender, e);

        }

        private async void btnCreateReport_Click(object sender, RoutedEventArgs e)
        { 
            tbProblematic.Visibility = Visibility.Collapsed;
            spProblematic.Visibility = Visibility.Collapsed;
            tbWaiting.Visibility = Visibility.Visible;
            mpbWaiting.Visibility = Visibility.Visible;
            if(tbDolarRate.Text != null && tbDolarRate.Text != string.Empty)
              await DB.UpdateCurrencyRateAsync(tbDolarRate.Text);
            var res = await GenarateReport.GenerateReport(cmbYear.SelectedValue.ToString(), cmbMonthly.SelectedValue.ToString()
                                                 ,(int)cmbDayInMonth.SelectedValue, (int)cmbCustomers.SelectedValue
                                                 ,(int)cmbClass.SelectedValue,OverrideFile, (bool)cbIsOverride.IsChecked);
            if (res.Success && res.FilePath != string.Empty)
            {
                tbProgressSuccess.Text = "התהליך הסתיים בהצלחה," + System.Environment.NewLine + "נוצר דוח חדש  " + 
                    System.Environment.NewLine + res.FilePath.Substring(res.FilePath.LastIndexOf("\\") +1);
                tbProgressSuccess.Visibility = Visibility.Visible;
                tbWaiting.Visibility = Visibility.Collapsed;
                mpbWaiting.Visibility = Visibility.Collapsed;
                var res1 = await PdfReport.GenerateBroadcastReport((int)cmbYear.SelectedValue,
                       (int)cmbMonthly.SelectedValue, (int)cmbDayInMonth.SelectedValue,
                                           (int)cmbCustomers.SelectedValue,(int)cmbClass.SelectedValue, OverrideFile);
            }
            else
            {
                if(res.ErrorMessage == "File already Exsist")
                {
                    tbProblematic.Visibility = Visibility.Visible;
                    spProblematic.Visibility = Visibility.Visible;
                    tbWaiting.Visibility = Visibility.Collapsed;
                    mpbWaiting.Visibility = Visibility.Collapsed;
                }
                if (res.ErrorMessage == "AmountSum is 0")
                {
                    tbProgressSuccess.Text = "לא נמצאו משלמים למוסד עבור התאריכים שנבחרו";
                    tbProgressSuccess.Visibility = Visibility.Visible;
                    tbWaiting.Visibility = Visibility.Collapsed;
                    mpbWaiting.Visibility = Visibility.Collapsed;
                }
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

        private void CmbCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbDayInMonth.SelectedItem = ((Customer)cmbCustomers.SelectedItem).PaymentDate1;
            var classList = new List<int>();
            var customerId = (int)cmbCustomers.SelectedValue;
            classList.AddRange(DB.GetClassesToCustomer(customerId));

            cmbClass.ItemsSource = classList;
            cmbClass.SelectedIndex = 0;
        }
    }
}
