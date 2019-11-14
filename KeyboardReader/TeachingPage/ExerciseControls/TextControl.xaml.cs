using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace KeyboardReader.Pages.ExerciseControls
{
    public sealed partial class TextControl : UserControl, IExerciseControlContent
    {
        #region Properties

        /// <summary>
        /// The text of this control.
        /// </summary>
        public string Text
        {
            get
            {
                editBox.TextDocument.GetText(
                    TextGetOptions.None, 
                    out string text);

                return text;
            }
            set => editBox.TextDocument.SetText(TextSetOptions.None, value);
        }

        #endregion

        public TextControl()
        {
            this.InitializeComponent();
        }

        public string Glyph => "\uE8D2";

        #region Dump to json
        /// <summary>
        /// Dumps this control to a json string.
        /// </summary>
        public string DumpToJson()
        {
            return
                "{" +
                    $"type : \"text\"," +
                    $"text : {Text}"    +
                "}";
        }
        #endregion
    }
}
