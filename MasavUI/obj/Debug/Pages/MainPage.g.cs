﻿#pragma checksum "..\..\..\Pages\MainPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "BC04EE7ED1D5C10759A88646FDB0CC1C98C4CDD4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Converters;
using FirstFloor.ModernUI.Windows.Navigation;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MasavUI.Pages {
    
    
    /// <summary>
    /// MainPage
    /// </summary>
    public partial class MainPage : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\Pages\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbCustomers;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Pages\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbActivity;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\Pages\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFilter;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\Pages\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnExportToExcel;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\Pages\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnEditPaying;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\Pages\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnNewPaying;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\Pages\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgReports;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MasavUI;component/pages/mainpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\MainPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.cmbCustomers = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 2:
            this.cmbActivity = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.btnFilter = ((System.Windows.Controls.Button)(target));
            
            #line 43 "..\..\..\Pages\MainPage.xaml"
            this.btnFilter.Click += new System.Windows.RoutedEventHandler(this.btnFilter_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnExportToExcel = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\..\Pages\MainPage.xaml"
            this.btnExportToExcel.Click += new System.Windows.RoutedEventHandler(this.ExportToExcelAndCsv_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnEditPaying = ((System.Windows.Controls.Button)(target));
            
            #line 48 "..\..\..\Pages\MainPage.xaml"
            this.btnEditPaying.Click += new System.Windows.RoutedEventHandler(this.btnEditPaying_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnNewPaying = ((System.Windows.Controls.Button)(target));
            
            #line 49 "..\..\..\Pages\MainPage.xaml"
            this.btnNewPaying.Click += new System.Windows.RoutedEventHandler(this.btnNewPaying_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.dgReports = ((System.Windows.Controls.DataGrid)(target));
            
            #line 64 "..\..\..\Pages\MainPage.xaml"
            this.dgReports.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.dgReports_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

