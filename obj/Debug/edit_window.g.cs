﻿#pragma checksum "..\..\edit_window.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F1747785FD6D5216DB03B1E1302B31B42BE1B907B64B67FA20149811DC9C006B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Library;
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


namespace Library {
    
    
    /// <summary>
    /// edit_window
    /// </summary>
    public partial class edit_window : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\edit_window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox new_author;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\edit_window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox new_title;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\edit_window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cancel_btn;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\edit_window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button submit_btn;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\edit_window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label id_label;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\edit_window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label new_holder;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\edit_window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label new_status;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\edit_window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox new_isbn;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\edit_window.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox new_grbc_code;
        
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
            System.Uri resourceLocater = new System.Uri("/Library;component/edit_window.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\edit_window.xaml"
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
            
            #line 8 "..\..\edit_window.xaml"
            ((Library.edit_window)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.new_author = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.new_title = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.cancel_btn = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\edit_window.xaml"
            this.cancel_btn.Click += new System.Windows.RoutedEventHandler(this.Cancel_btn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.submit_btn = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\edit_window.xaml"
            this.submit_btn.Click += new System.Windows.RoutedEventHandler(this.Submit_btn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.id_label = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.new_holder = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.new_status = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.new_isbn = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.new_grbc_code = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

