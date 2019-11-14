using KeyboardReader.Pages.ExerciseControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace KeyboardReader.Pages
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class LessonCreationPage : Page
    {
        #region Fields

        private static List<LessonControl> _lessons;

        /// <summary>
        /// The sender control that wants to be modified, 
        /// if any.
        /// </summary>
        private LessonControl _lessonControl;

        #endregion

        #region Properties

        /// <summary>
        /// The lesson's title.
        /// </summary>
        public string Header { get; set; }

        #endregion

        #region Constructor
        public LessonCreationPage()
        {
            this.InitializeComponent();

            _lessons = LessonsPage.Lessons;
        }
        #endregion

        private void AddTextControl(object sender, RoutedEventArgs e)           => CreateExercise(new TextControl());
        private void AddSingleChoiceControl(object sender, RoutedEventArgs e)   => CreateExercise(new SingleChoiceControl());
        private void AddMultipleChoiceControl(object sender, RoutedEventArgs e) => CreateExercise(null);

        #region Create exercise
        /// <summary>
        /// Creates a new <see cref="ExerciseControl"/>
        /// and adds it to the list.
        /// </summary>
        private void CreateExercise(UIElement innerControl)
        {
            var control = new ExerciseControl
            {
                InnerControl = innerControl
            };
            control.DeleteRequested += (c) => exercisesPanel.Children.Remove(c);
            exercisesPanel.Children.Add(control);
        }
        #endregion

        #region Save lesson
        /// <summary>
        /// Saves a lesson, adding it to the 
        /// <see cref="LessonsPage.Lessons"/> list.
        /// </summary>
        private void Save(object sender, RoutedEventArgs e)
        {
            // Getting the lesson control
            LessonControl control = new LessonControl
            {
                Header =
                    this.Header != string.Empty ? this.Header : "Senza titolo"
            };

            // Adding the various exercises to it
            control.AddExercises(
                exercisesPanel.Children.Cast<ExerciseControl>());

            // Modifying the existing one
            if (_lessonControl != null)
                _lessonControl = control;

            // Adding it to the lessons page
            else
                _lessons.Add(control);

            MainPage.ContentFrame.GoBack();
        }
        #endregion

        #region Close
        private void Close(object sender, RoutedEventArgs e)
        {
            MainPage.ContentFrame.GoBack();
        }
        #endregion

        #region On navigated to
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null) return;

            _lessonControl = 
                e.Parameter as LessonControl;

            // Setting the header
            Header = _lessonControl.Header;

            // Adding the exercises
            foreach (SmallExerciseControl smallExercise in _lessonControl.Exercises)
            {
                ExerciseControl exercise = smallExercise;

                exercise.DeleteRequested += (c) => exercisesPanel.Children.Remove(c);
                exercisesPanel.Children.Add(exercise);
            }
        }
        #endregion
    }
}
