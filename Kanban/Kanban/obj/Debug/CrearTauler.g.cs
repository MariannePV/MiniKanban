﻿#pragma checksum "..\..\CrearTauler.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "380812DB2B9C73FDCC7A4B4A332610941C9DAE4246C89E02C0DD4DF186588860"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using Kanban;
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


namespace Kanban {
    
    
    /// <summary>
    /// CrearTauler
    /// </summary>
    public partial class CrearTauler : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 39 "..\..\CrearTauler.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock idTauler;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\CrearTauler.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtTitol;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\CrearTauler.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle ColorRectangle;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\CrearTauler.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnEliminar;
        
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
            System.Uri resourceLocater = new System.Uri("/Kanban;component/creartauler.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\CrearTauler.xaml"
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
            
            #line 9 "..\..\CrearTauler.xaml"
            ((Kanban.CrearTauler)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 36 "..\..\CrearTauler.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.CerrarVentana_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.idTauler = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.txtTitol = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.ColorRectangle = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 6:
            
            #line 53 "..\..\CrearTauler.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SeleccionarColor_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnEliminar = ((System.Windows.Controls.Button)(target));
            
            #line 62 "..\..\CrearTauler.xaml"
            this.btnEliminar.Click += new System.Windows.RoutedEventHandler(this.EliminarTauler);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 63 "..\..\CrearTauler.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.DesarTauler);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
