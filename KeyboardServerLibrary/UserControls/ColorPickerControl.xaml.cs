using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;


namespace KeyboardReaderLibrary.UserControls
{
    public sealed partial class ColorPickerControl : UserControl
    {
        public ColorPickerControl()
        {
            this.InitializeComponent();
        }

        #region Events

        public delegate void ColorPickedHandler(Brush brush, ElementTheme theme);

        /// <summary>
        /// Raised when a color is picker.
        /// </summary>
        public event ColorPickedHandler ColorPicked;

        #endregion

        #region Pick color
        /// <summary>
        /// Picks a color and raises the 
        /// <see cref="ColorPicked"/> event.
        /// </summary>
        private void PickColor(object sender, PointerRoutedEventArgs e)
        {
            var rect = sender as Rectangle;

            ColorPicked?.Invoke(
                rect.Fill,
                rect.RequestedTheme);
        }
        #endregion
    }
}
