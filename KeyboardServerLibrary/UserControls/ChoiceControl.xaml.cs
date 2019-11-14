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

// Il modello di elemento Controllo utente è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234236

namespace KeyboardReaderLibrary.UserControls
{
    public sealed partial class ChoiceControl : UserControl
    {
        public ChoiceControl()
        {
            this.InitializeComponent();
        }

        #region Properties

        /// <summary>
        /// Placeholder text for this choice.
        /// </summary>
        public string PlaceholderText { get; set; }

        /// <summary>
        /// The group of radios that this control is part of.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// The choice's text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Tells whether this choice is checked or not.
        /// </summary>
        public bool IsChecked { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Raised when the delete button is pressed.
        /// </summary>
        public event Action<ChoiceControl> DeleteRequested;

        #endregion


        private void Delete(object sender, RoutedEventArgs e) => DeleteRequested?.Invoke(this);
    }
}
