using KeyboardReader.Pages.ExerciseControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace KeyboardReader.Pages
{
    public sealed partial class LessonsPage : Page
    {
        #region Fields

        private static List<LessonControl> _lessons;

        /// <summary>
        /// Path where the lessons are being saved.
        /// </summary>
        public static readonly Uri PATH = new Uri("ms-appx:///local/lessons.json");

        #endregion

        #region Properties

        /// <summary>
        /// The list of currently loaded lessons.
        /// </summary>
        public static List<LessonControl> Lessons => _lessons;

        #endregion


        #region Constructor
        public LessonsPage()
        {
            this.InitializeComponent();

            lessonsGrid.Loaded += InitLessonsAsync;
        }
        #endregion


        #region Load lessons
        private async void InitLessonsAsync(object sender, RoutedEventArgs e)
        {
            // Loading the exercises
            // singleton
            if (_lessons == null)
            {
                _lessons =
                    await TryLoadLessonsAsync();
            }

            // Adding the loaded lessons to the grid
            foreach (var lesson in _lessons)
            {
                lessonsGrid.Items.Add(lesson);
                lesson.DeleteRequested += DeleteLesson;
            }
        }
        #endregion

        #region Delete lesson
        /// <summary>
        /// Deletes a lesson from the existing ones.
        /// </summary>
        private void DeleteLesson(LessonControl sender)
        {
            _lessons.Remove(sender);
            lessonsGrid.Items.Remove(sender);
        }
        #endregion

        #region Create lesson
        /// <summary>
        /// Navigates to the <see cref="LessonCreationPage"/>.
        /// </summary>
        private void CreateLesson(object sender, RoutedEventArgs e)
        {
            MainPage.ContentFrame.Navigate(typeof(LessonCreationPage));
        }
        #endregion

        #region Save lessons async
        /// <summary>
        /// Saves the lessons into a json file.
        /// </summary>
        public static async Task SaveLessonsAsync()
        {
            // Getting the serialized objects'
            // string
            string jsonString =
                JsonConvert.SerializeObject(new
                {
                    lessons =
                        from lesson in _lessons
                        select lesson.DumpToJson()
                });

            // If the file doesn't exist
            if (!File.Exists(PATH.OriginalString))
                File.Create(PATH.OriginalString);

            var file =
                await StorageFile.GetFileFromApplicationUriAsync(PATH);

            await FileIO.WriteTextAsync(file, jsonString);
        }
        #endregion

        #region Try load lessons async
        /// <summary>
        /// Tries to load the lessons. 
        /// Returns a A <see cref="List{T}"/> of <see cref="LessonControl"/> 
        /// that contains the loaded lessons.
        /// </summary>
        private async Task<List<LessonControl>> TryLoadLessonsAsync()
        {
            // If the file doesn't exist
            if (!File.Exists(PATH.OriginalString))
                return new List<LessonControl>();

            // Trying to get the file
            var file =
                await StorageFile.GetFileFromApplicationUriAsync(PATH);

            var jsonString =
                await FileIO.ReadTextAsync(file);

            // Returning an empty list if the file is empty
            if (string.IsNullOrWhiteSpace(jsonString))
                return new List<LessonControl>();

            // Converting and returning the loaded lessons
            return
                JsonConvert.DeserializeObject<List<LessonControl>>(jsonString);
        }
        #endregion

        #region On navigated from
        /// <summary>
        /// Cleanup on navigation.
        /// </summary>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            lessonsGrid.Items.Clear();
            GC.Collect();
        }
        #endregion
    }
}
