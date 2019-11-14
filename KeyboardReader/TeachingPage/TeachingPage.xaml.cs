using KeyboardReader.Pages;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace KeyboardReader.Pages
{
    public sealed partial class TeachingPage : Page
    {
        public TeachingPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Navigates to the communication page.
        /// </summary>
        private void NavigateToCommunicationPage(object sender, RoutedEventArgs e)
        {
            MainPage.ContentFrame.Navigate(typeof(CommunicationMainPage));
        }

        /// <summary>
        /// Navigates to the lessons preparation page.
        /// </summary>
        private void NavigateToLessonsPreparationPage(object sender, RoutedEventArgs e)
        {
            MainPage.ContentFrame.Navigate(typeof(LessonsPage));
        }

        private void NavigateToTestsPage(object sender, RoutedEventArgs e)
        {
            MainPage.ContentFrame.Navigate(typeof(TestsPage));
        }
    }
}
