﻿#pragma checksum "C:\Condivisi\4H\NAO\KeyboardReader\KeyboardReader\KeyboardConnectionPage\KeyboardConnectionPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "98D07934B9B443BD6EB4EE7AAE2A53D9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KeyboardReader.Pages
{
    partial class KeyboardConnectionPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // KeyboardConnectionPage\KeyboardConnectionPage.xaml line 24
                {
                    this.clearTextButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.clearTextButton).Click += this.ClearText;
                }
                break;
            case 3: // KeyboardConnectionPage\KeyboardConnectionPage.xaml line 85
                {
                    this.connectionPanel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                }
                break;
            case 4: // KeyboardConnectionPage\KeyboardConnectionPage.xaml line 106
                {
                    this._mediaElement = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this._mediaElement).MediaEnded += this.EnableNextSpeech;
                }
                break;
            case 5: // KeyboardConnectionPage\KeyboardConnectionPage.xaml line 36
                {
                    this.buttonEnterAnimation = (global::Windows.UI.Xaml.Media.Animation.Storyboard)(target);
                }
                break;
            case 6: // KeyboardConnectionPage\KeyboardConnectionPage.xaml line 49
                {
                    this.buttonExitAnimation = (global::Windows.UI.Xaml.Media.Animation.Storyboard)(target);
                }
                break;
            case 7: // KeyboardConnectionPage\KeyboardConnectionPage.xaml line 14
                {
                    this.typedText = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.18362.1")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

