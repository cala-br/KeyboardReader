using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Windows.Foundation.Metadata;

namespace KeyboardReader.KeyboardManager
{
    [Deprecated("UDP communication isn't used anymore", DeprecationType.Remove, 1)]
    public sealed class KeyboardWatcher
    {
        #region Instance

        private static KeyboardWatcher _instance;

        public static KeyboardWatcher GetInstance()
        {
            if (_instance == null)
                _instance = new KeyboardWatcher();

            return _instance;
        }

        #endregion


        #region Fields

        public const int REMOTE_PORT = 48_080;
        public static readonly byte[] DISCOVERY_MESSAGE =
            Encoding.UTF8.GetBytes("keyboard_discovery");

        private UdpClient _udpClient;

        #endregion


        #region Constructor
        private KeyboardWatcher()
        {

        }
        #endregion
    }
}
