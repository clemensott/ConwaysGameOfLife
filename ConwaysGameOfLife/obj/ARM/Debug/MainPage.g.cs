﻿#pragma checksum "D:\Projects\Eigentlich Fertig\ConwaysGameOfLife\ConwaysGameOfLife\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7C5CEB383671B554572EF2A80F455320"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConwaysGameOfLife
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.ccDraw = (global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl)(target);
                    #line 63 "..\..\..\MainPage.xaml"
                    ((global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl)this.ccDraw).Draw += this.ccDraw_Draw;
                    #line default
                }
                break;
            case 2:
                {
                    this.slvGrid = (global::Windows.UI.Xaml.Controls.ScrollViewer)(target);
                    #line 67 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ScrollViewer)this.slvGrid).SizeChanged += this.slvGrid_SizeChanged;
                    #line 67 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ScrollViewer)this.slvGrid).ViewChanging += this.slvGrid_ViewChanging;
                    #line 68 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ScrollViewer)this.slvGrid).Tapped += this.ccEmpty_Tapped;
                    #line 68 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ScrollViewer)this.slvGrid).DirectManipulationStarted += this.slvGrid_DirectManipulationStarted;
                    #line 69 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ScrollViewer)this.slvGrid).DirectManipulationCompleted += this.slvGrid_DirectManipulationCompleted;
                    #line default
                }
                break;
            case 3:
                {
                    this.ccEmpty = (global::Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl)(target);
                }
                break;
            case 4:
                {
                    this.sldTimer = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    #line 43 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Slider)this.sldTimer).ValueChanged += this.SldTimer_ValueChanged;
                    #line default
                }
                break;
            case 5:
                {
                    this.sldColumns = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    #line 45 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Slider)this.sldColumns).ValueChanged += this.SldColumns_ValueChanged;
                    #line default
                }
                break;
            case 6:
                {
                    this.sldRows = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    #line 47 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Slider)this.sldRows).ValueChanged += this.SldRows_ValueChanged;
                    #line default
                }
                break;
            case 7:
                {
                    global::Windows.UI.Xaml.Controls.CheckBox element7 = (global::Windows.UI.Xaml.Controls.CheckBox)(target);
                    #line 54 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.CheckBox)element7).Checked += this.UpdateView;
                    #line 54 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.CheckBox)element7).Unchecked += this.UpdateView;
                    #line default
                }
                break;
            case 8:
                {
                    global::Windows.UI.Xaml.Controls.CheckBox element8 = (global::Windows.UI.Xaml.Controls.CheckBox)(target);
                    #line 56 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.CheckBox)element8).Checked += this.UpdateView;
                    #line 56 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.CheckBox)element8).Unchecked += this.UpdateView;
                    #line default
                }
                break;
            case 9:
                {
                    this.btnStartStop = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 21 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnStartStop).Click += this.BtnStartStop_Click;
                    #line default
                }
                break;
            case 10:
                {
                    global::Windows.UI.Xaml.Controls.Button element10 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 22 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element10).Click += this.BtnStep_Click;
                    #line default
                }
                break;
            case 11:
                {
                    global::Windows.UI.Xaml.Controls.Button element11 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 23 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element11).Click += this.BtnClear_Click;
                    #line default
                }
                break;
            case 12:
                {
                    global::Windows.UI.Xaml.Controls.Button element12 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 24 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element12).Click += this.BtnLoad_Click;
                    #line default
                }
                break;
            case 13:
                {
                    global::Windows.UI.Xaml.Controls.Button element13 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 25 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element13).Click += this.BtnSave_Click;
                    #line default
                }
                break;
            case 14:
                {
                    global::Windows.UI.Xaml.Controls.Button element14 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 26 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element14).Click += this.BtnRandom_Click;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

