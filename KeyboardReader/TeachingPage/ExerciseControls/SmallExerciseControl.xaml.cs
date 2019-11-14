using KeyboardReader.Pages.ExerciseControls;
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

namespace KeyboardReader.Pages.ExerciseControls
{
    public sealed partial class SmallExerciseControl : UserControl
    {
        #region Fields

        private UIElement _fullExercise;

        #endregion

        #region Properties

        /// <summary>
        /// The glyph of this control.
        /// </summary>
        public string Glyph { get; set; }

        /// <summary>
        /// The header of the exercise.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// The exercise.
        /// </summary>
        public UIElement InnerExercise
        {
            get => _fullExercise;
            set => _fullExercise = value;
        }

        #endregion


        public SmallExerciseControl()
        {
            this.InitializeComponent();
        }
    }
}
