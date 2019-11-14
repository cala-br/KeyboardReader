using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Finestra di dialogo contenuto è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace KeyboardReader.Pages
{
    public sealed partial class KeyboardDisconnectedDialog : ContentDialog
    {
        public KeyboardDisconnectedDialog()
        {
            this.InitializeComponent();
        }
    }
}
