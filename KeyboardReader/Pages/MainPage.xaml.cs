using KeyboardReaderLibrary.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using KeyboardReaderLibrary.NAOServer;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace KeyboardReader.Pages
{
    public sealed partial class MainPage : Page
    {
        #region Settings
        /// <summary>
        /// Used to store the settings.
        /// </summary>
        internal class Setting
        {
            public int theme;
        }
        #endregion


        #region Fields

        /// <summary>
        /// Static reference to the main page.
        /// </summary>
        private static MainPage _mainPage;

        /// <summary>
        /// Dictionary used to quickly switch pages.
        /// </summary>
        private List<ValueTuple<object, Type>> _pagesList;

        /// <summary>
        /// The app's title bar.
        /// </summary>
        private static ApplicationViewTitleBar _titleBar = ApplicationView.GetForCurrentView().TitleBar;

        #endregion

        #region Properties

        /// <summary>
        /// The application's requested theme.
        /// </summary>
        public static ElementTheme ApplicationTheme
        {
            get => _mainPage.RequestedTheme;
            set
            {
                _mainPage.RequestedTheme = value;
                InitAppBar(true);
            }
        }

        /// <summary>
        /// The main page's content frame.
        /// </summary>
        public static Frame ContentFrame
        {
            get => _mainPage.contentFrame;
            set => _mainPage.contentFrame = value;
        }

        #endregion


        #region Constructor
        public MainPage()
        {
            InitializeComponent();

            _mainPage = this;

            InitAppBar();

            // Back button
            contentFrame.Navigated += ContentFrameNavigated;
            navView.BackRequested  += (s, e) => contentFrame.GoBack();

            // On navigation view loaded
            navView.Loaded += (s, e) =>
            {
                LoadSettings();
                InitMenuItems();
            };
        }
        #endregion


        #region Init app bar
        private static void InitAppBar(bool isThemeBeingChanged = false)
        {
            // Changing the foreground color
            if (_mainPage.ActualTheme == ElementTheme.Dark)
                _titleBar.ButtonForegroundColor = Windows.UI.Colors.White;
            else
                _titleBar.ButtonForegroundColor = Windows.UI.Colors.Black;

            if (isThemeBeingChanged) return;

            // Set active window colors
            _titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
            _titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.DarkGray;
            _titleBar.ButtonHoverBackgroundColor = Windows.UI.Colors.Gray;
            _titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.Gray;
            _titleBar.ButtonPressedBackgroundColor = Windows.UI.Colors.DarkGray;

            // Set inactive window colors
            _titleBar.InactiveForegroundColor = Windows.UI.Colors.Gray;
            _titleBar.InactiveBackgroundColor = Windows.UI.Colors.Transparent;
            _titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.Gray;
            _titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;

            // Title bar override
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
        }
        #endregion

        #region Load settings
        /// <summary>
        /// Loads the saved settings.
        /// </summary>
        private async void LoadSettings()
        {
            Setting settings =
                await Settings.LoadAsync<Setting>();

            // If there are any
            if (settings != null)
            {
                RequestedTheme = (ElementTheme)settings.theme;
            }
        }
        #endregion

        #region Init menu items
        /// <summary>
        /// Initializes the menu items and
        /// navigates to the first page.
        /// </summary>
        private void InitMenuItems()
        {
            // Initializing the pages dict
            _pagesList = new List<(object, Type)>
            {
                ( navView.MenuItems[0], typeof(KeyboardConnectionPage)),
                ( navView.MenuItems[1], typeof(TeachingPage)          ),
                ( navView.SettingsItem, typeof(SettingsPage)          ),

                // Sub pages of ExercisesPage
                ( navView.MenuItems[1], typeof(CommunicationMainPage)),
                ( navView.MenuItems[1], typeof(LessonsPage)          ),
                ( navView.MenuItems[1], typeof(LessonCreationPage)   )
            };

            // Handler for navigating to the selected page
            navView.SelectionChanged += (s, e) =>
            {
                var requestedPageType =
                    _pagesList.First(pair => pair.Item1 == e.SelectedItem).Item2;

                contentFrame.Navigate(requestedPageType);
            };

            // Navigating to connection page
            contentFrame.Navigate(typeof(KeyboardConnectionPage));
        }
        #endregion

        #region Content frame - navigated
        /// <summary>
        /// Enables or disables the back button, and takes care
        /// of selecting the right menu item when navigating back.
        /// </summary>
        private void ContentFrameNavigated(object sender, NavigationEventArgs e)
        {
            navView.IsBackEnabled = contentFrame.CanGoBack;

            // If the current displayed page is in the 
            // pages list, then selecting its relative
            // menu item on the navigation view           
            var menuItem =
                _pagesList.FirstOrDefault(pair => pair.Item2 == e.SourcePageType).Item1;

            if (menuItem == default) return;

            navView.SelectedItem = menuItem;
        }
        #endregion
    }
}
