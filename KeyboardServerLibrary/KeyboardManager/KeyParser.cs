using System.Text.RegularExpressions;
using Windows.Foundation.Metadata;

namespace KeyboardReader.KeyboardManager
{
    [Deprecated("UDP communication isn't used anymore", DeprecationType.Remove, 1)]
    public static class KeyParser
    {
        public static string Parse(string rawKeyMessage)
        {
            // Extracting the key from the message
            var groups =
                Regex.Match(rawKeyMessage, "key *= *(.+)").Groups;

            return 
                groups.Count > 1 ? groups[1].Value : null;
        }
    }
}
