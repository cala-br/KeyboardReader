using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

namespace KeyboardReaderLibrary.Settings
{
    public static class Settings
    {
        #region Fields

        private static object _loadedSettings;
        /// <summary>
        /// The currently loaded settings.
        /// </summary>
        public static object GetLoadedSettings => _loadedSettings;

        public static readonly Uri PATH = 
            new Uri("ms-appdata:///local/settings.json");

        #endregion

        #region Load async
        /// <summary>
        /// Loads the settings asynchronously.
        /// </summary>
        public static async Task<T> LoadAsync<T>() where T : class
        {
            // Opening the file
            StorageFile file;
            try
            {
                file =
                    await StorageFile.GetFileFromApplicationUriAsync(PATH);
            }
            catch { return null; }

            // Reading the file contents asynchronously
            string jsonString =
                await FileIO.ReadTextAsync(file);

            if (string.IsNullOrWhiteSpace(jsonString))
                return null;

            // Converting the json string into an
            // object and returning it
            _loadedSettings = 
                JsonConvert.DeserializeObject<T>(jsonString);

            return _loadedSettings as T;
        }
        #endregion

        #region Save async
        /// <summary>
        /// Saves the settings asynchronously.
        /// </summary>
        /// <param name="values">An object containing the values that need to be saved.</param>
        public static async Task SaveAsync(object settings)
        {
            _loadedSettings = settings;

            // Getting the serialized objects'
            // string
            string jsonString =
                JsonConvert.SerializeObject(settings);

            var file =
                await StorageFile.GetFileFromApplicationUriAsync(PATH);

            await FileIO.WriteTextAsync(file, jsonString);
        }
        #endregion
    }
}
