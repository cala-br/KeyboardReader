using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Newtonsoft.Json;

namespace KeyboardReader.Pages.ExerciseControls
{
    public sealed partial class ExerciseControl : UserControl
    {
        #region Properties

        /// <summary>
        /// The inner control that's showed within this
        /// <see cref="ExerciseControl"/>.
        /// </summary>
        public UIElement InnerControl { get; set; }

        /// <summary>
        /// The control's header.
        /// </summary>
        public string Header { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Invoked when the user requests the deletion
        /// of this exercise.
        /// </summary>
        public event Action<ExerciseControl> DeleteRequested;

        #endregion


        #region Constructor
        public ExerciseControl()
        {
            this.InitializeComponent();
        }
        #endregion


        #region Request delete
        /// <summary>
        /// Requests the deletion of this exercise.
        /// </summary>
        private void RequestDelete(object sender, RoutedEventArgs e)
        {
            DeleteRequested?.Invoke(this);
            deleteButtonFlyout.Hide();
        }
        #endregion

        /// <summary>
        /// Hides the delete flyout.
        /// </summary>
        private void HideDeleteFlyout(object sender, RoutedEventArgs e) => deleteButtonFlyout.Hide();

        #region Color picked
        /// <summary>
        /// Changes the color of this control.
        /// </summary>
        /// <param name="brush">The chosen brush.</param>
        /// <param name="theme">The chosen theme.</param>
        private void ColorPicked(Brush brush, ElementTheme theme)
        {
            mainGrid.BorderBrush      = brush;
            headerGrid.Background     = brush;
            headerGrid.RequestedTheme = theme;
        }
        #endregion


        #region Conversion to SmallExerciseControl
        /// <summary>
        /// Converts an <see cref="ExerciseControl"/> to a 
        /// new <see cref="SmallExerciseControl"/>.
        /// </summary>
        public static implicit operator SmallExerciseControl(ExerciseControl exerciseControl)
        {
            return new SmallExerciseControl
            {
                Header = 
                    exerciseControl.header.Text,

                Glyph = 
                    ((IExerciseControlContent)exerciseControl.InnerControl).Glyph,

                InnerExercise = 
                    exerciseControl.InnerControl
            };
        }
        #endregion

        #region Conversion from SmallExerciseControl
        /// <summary>
        /// Converts an <see cref="ExerciseControl"/> to a 
        /// new <see cref="SmallExerciseControl"/>.
        /// </summary>
        public static implicit operator ExerciseControl(SmallExerciseControl smallExerciseControl)
        {
            return new ExerciseControl
            {
                Header =
                    smallExerciseControl.Header,

                InnerControl =
                    smallExerciseControl.InnerExercise
            };
        }
        #endregion
    }
}
