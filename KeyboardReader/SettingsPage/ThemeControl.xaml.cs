using KeyboardReader.Pages;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace KeyboardReader.Pages
{
    public sealed partial class ThemeControl : UserControl
    {
        #region Fields

        /// <summary>
        /// Binding between the radio buttons and 
        /// the themes.
        /// </summary>
        private Dictionary<object, ElementTheme> _themeRadiosDict;

        #endregion

        public ThemeControl()
        {
            this.InitializeComponent();

            // On themes panel loaded
            InitThemeRadios();
        }

        #region Init theme radios 
        private void InitThemeRadios()
        {
            // Initializing the radios dict
            _themeRadiosDict = new Dictionary<object, ElementTheme>
            {
                { lightThemeRadio,   ElementTheme.Light   },
                { darkThemeRadio,    ElementTheme.Dark    },
                { defaultThemeRadio, ElementTheme.Default }
            };

            // Selecting the current theme
            foreach (var child in themesPanel.Children)
            {
                if (child is RadioButton radioButton)
                {
                    if (_themeRadiosDict[radioButton] == MainPage.ApplicationTheme)
                    {
                        radioButton.IsChecked = true;
                        break;
                    }
                }
            }
        }
        #endregion

        #region Theme selected
        /// <summary>
        /// Changes the theme accordingly to the <see cref="RadioButton"/>
        /// that has been pressed.
        /// </summary>
        private void ThemeComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            MainPage.ApplicationTheme = _themeRadiosDict[sender];
        }
        #endregion
    }
}
