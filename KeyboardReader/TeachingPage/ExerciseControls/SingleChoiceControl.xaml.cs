using KeyboardReaderLibrary.UserControls;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace KeyboardReader.Pages.ExerciseControls
{
    public sealed partial class SingleChoiceControl : UserControl, IExerciseControlContent
    {
        #region Fields

        private static int _currentGroup;
        private string _group;

        #endregion

        #region Properties

        public string Glyph => "\uE8BC";

        #endregion


        public SingleChoiceControl()
        {
            this.InitializeComponent();

            InitChoicesPanel();
        }

        #region Init choices panel
        /// <summary>
        /// Initializes the choices panel.
        /// </summary>
        private void InitChoicesPanel()
        {
            choicesPanel.SizeChanged += (s, e) =>
            {
                choicesScrollViewer.ChangeView(null, int.MaxValue, null);
            };

            _group = $"{_currentGroup++}";

            AddChoice(null, null);
            AddChoice(null, null);
        }
        #endregion

        #region Add choice
        /// <summary>
        /// Adds a choice to the choices panel.
        /// </summary>
        private void AddChoice(object sender, RoutedEventArgs e)
        {
            var choiceControl = new ChoiceControl
            {
                PlaceholderText = "Risposta...",
                Group = _group
            };
            choiceControl.DeleteRequested += DeleteChoice;

            choicesPanel.Children.Add(choiceControl);
        }
        #endregion

        #region Delete choice
        /// <summary>
        /// Deletes a choice from the choices panel.
        /// </summary>
        private void DeleteChoice(ChoiceControl choiceControl)
        {
            choicesPanel.Children.Remove(choiceControl);
        }
        #endregion

        #region Dump to Json
        /// <summary>
        /// Dumps this <see cref="SingleChoiceControl"/> into a 
        /// Json string.
        /// </summary>
        public string DumpToJson()
        {
            // Putting all the choices in a string
            string choicesList = string.Empty;
            foreach (ChoiceControl choice in choicesPanel.Children)
                choicesList += $"\"{choice.Text}\",";
            choicesList.Remove(choicesList.Length - 1);

            // Getting the index of the selected element
            int index = -1;
            for (int i = 0; i < choicesPanel.Children.Count; i++)
            {
                if ((choicesPanel.Children[0] as ChoiceControl).IsChecked)
                {
                    index = i;
                    break;
                }
            }

            return
                "{" +
                    $"type : \"single#choice\","              +
                    $"question : \"{questionTextBox.Text}\"," +
                    $"choices : [{choicesList}],"             +
                    $"correctChoiceIndex : {index}"           +
                "}";
        }
        #endregion
    }
}
