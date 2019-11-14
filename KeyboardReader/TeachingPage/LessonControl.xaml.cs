using KeyboardReader.Pages.ExerciseControls;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace KeyboardReader.Pages
{
    public sealed partial class LessonControl : UserControl
    {
        #region Properties

        /// <summary>
        /// The lesson's header.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// The list of <see cref="SmallExerciseControl"/> contained
        /// in this lesson control.
        /// </summary>
        public UIElementCollection Exercises => exercisesPanel.Children;

        #endregion

        #region Events

        public delegate void DeleteRequestedHandler(LessonControl sender);

        /// <summary>
        /// Invoked when the user requests the deletion of
        /// this ExerciseControl.
        /// </summary>
        public event DeleteRequestedHandler DeleteRequested;

        #endregion


        #region Constructor
        /// <summary>
        /// Creates an empty lesson
        /// </summary>
        public LessonControl()
        {
            this.InitializeComponent();
        }
        #endregion


        #region Add exercises
        /// <summary>
        /// Adds the specified <see cref="SmallExerciseControl"/>s to the control.
        /// </summary>
        public void AddExercises(IEnumerable<ExerciseControl> exercises)
        {
            foreach (var exercise in exercises)
                exercisesPanel.Children.Add((SmallExerciseControl)exercise);
        }
        #endregion

        #region Open in new window
        /// <summary>
        /// Opens this control in a new window, where it 
        /// can be customized.
        /// </summary>
        private void OpenInNewWindow(object sender, RoutedEventArgs e)
        {
            MainPage.ContentFrame.Navigate(typeof(LessonCreationPage), this);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Raises the <see cref="DeleteRequested"/> event.
        /// </summary>
        private void Delete(object sender, RoutedEventArgs e)
        {
            DeleteRequested?.Invoke(this
);
        }
        #endregion

        /// <summary>
        /// Hides the delete flyout.
        /// </summary>
        private void HideDeleteFlyout(object s, RoutedEventArgs e) => deleteButtonFlyout.Hide();

        #region Dump to json
        /// <summary>
        /// Dumps this control's exercises into
        /// a json string.
        /// </summary>
        public string DumpToJson()
        {
            string jsonString = 
                "{" +
                    $"header : \"{Header}\"" +
                    $"exercises : [";

            foreach (SmallExerciseControl control in Exercises)
            {
                jsonString += 
                    (control.InnerExercise as IExerciseControlContent).DumpToJson() + ",";
            }
            jsonString.Remove(jsonString.Length - 1);
            jsonString += "]}";

            return jsonString;
        }
        #endregion
    }
}
