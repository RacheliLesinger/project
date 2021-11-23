using FirstFloor.ModernUI.Windows.Controls;
using System;
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
using System.Collections;
using FirstFloor.ModernUI.Presentation;

namespace MasavUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
		public MainWindow()
		{
			try
			{
				InitializeComponent();
				ContentSource = MenuLinkGroups.First().Links.First().Source;
				//AppearanceManager.Current.AccentColor = Colors.CadetBlue;
			}
			catch (Exception ex)
			{
				MessageBox.Show(String.Format("MainWindow: {0} --- trace {1}  --- inner exception {2}", ex.Message, ex.StackTrace, ex.InnerException));
				Console.WriteLine(String.Format("MainWindow: {0} --- trace {1}", ex.Message, ex.StackTrace));
			}
		}
		public  void NavigateToFirstTab()
		{
			ContentSource = MenuLinkGroups.First().Links.First().Source;
		}

		public void OnNavigatingFrom(NavigatingCancelEventArgs e)
		{
			
		}
		public void OnFragmentNavigation(FragmentNavigationEventArgs e)
		{
		}
		public void OnNavigatedFrom(NavigationEventArgs e)
		{
		}
		public void OnNavigatedTo(NavigationEventArgs e)
		{
		}

		private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
		{
			this.Left = SystemParameters.WorkArea.Left;
			this.Top = SystemParameters.WorkArea.Top;
			this.Height = SystemParameters.WorkArea.Height;
			this.Width = SystemParameters.WorkArea.Width;
		}
	}
}
