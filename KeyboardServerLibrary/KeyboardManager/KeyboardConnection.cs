using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace KeyboardReader.KeyboardManager
{
    [Deprecated("UDP communication isn't used anymore", DeprecationType.Remove, 1)]
    public class KeyboardConnection : IDisposable
    {
        #region Singleton instance

        private static KeyboardConnection _instance;

        /// <summary>
        /// Returns the current instance of the <see cref="KeyboardConnection"/>
        /// singleton.
        /// If the previous instance was disposed, it creates a new one.
        /// </summary>
        public static KeyboardConnection GetInstance
        { 
            get
            {
                if (_instance == null || _instance._disposed)
                    _instance = new KeyboardConnection();

                return _instance;
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// Socket used for the communication with the keyboard.
        /// </summary>
        private Socket _tcpSocket;

        /// <summary>
        /// Tells whether the app is listening for 
        /// incoming messages.
        /// </summary>
        private bool _isListening;

        /// <summary>
        /// Used to stop the listening tasks.
        /// </summary>
        private CancellationTokenSource _tokenSource;

        /// <summary>
        /// The local <see cref="IPEndPoint"/> used by 
        /// the <see cref="_tcpSocket"/>.
        /// </summary>
        private readonly IPEndPoint KEYBOARD_EP = new IPEndPoint(
            IPAddress.Loopback,
            48_080);

        private bool _disposed;

        /// <summary>
        /// Acknowledgement message in order to update the 
        /// state of the <see cref="_tcpSocket"/> (Connected or not).
        /// </summary>
        private readonly byte[] ACK = Encoding.UTF8.GetBytes("ack\n");

        #endregion

        #region Properties

        /// <summary>
        /// Tells whether the underlying <see cref="Socket"/> is 
        /// connected or not.
        /// </summary>
        public bool IsConnected { get => _tcpSocket.Connected; }

        #endregion

        #region Events

        /// <summary>
        /// Raised when the application connects with the keyboard.
        /// </summary>
        public event KeyboardConnectedHandler KeyboardConnected;
        public delegate void KeyboardConnectedHandler();

        /// <summary>
        /// Raised when the keyboard disconnects.
        /// </summary>
        public event KeyboardDisconnectedHandler KeyboardDisconnected;
        public delegate void KeyboardDisconnectedHandler();

        /// <summary>
        /// Raised when a key is pressed on the keyboard.
        /// </summary>
        public event KeyReceivedHandler KeyReceived;
        public delegate void KeyReceivedHandler(string key);

        #endregion

        #region Exceptions

        /// <summary>
        /// Raised when the keyboard isn't connected.
        /// </summary>
        [Serializable]
        public class KeyboardNotConnectedException : Exception
        {
            public KeyboardNotConnectedException() : base("You must call ConnectAsync() before listening.") { }
            public KeyboardNotConnectedException(string message) : base(message) { }
            public KeyboardNotConnectedException(string message, Exception inner) : base(message, inner) { }
            protected KeyboardNotConnectedException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

        #endregion


        #region Constructor
        private KeyboardConnection()
        {
            InstantiateSocket();
            _isListening = false;
        }
        #endregion

        #region Instantiate socket
        internal void InstantiateSocket()
        {
            if (_tcpSocket != null) return;

            // Instantiating and binding the socket
            _tcpSocket =
                new Socket(SocketType.Stream, ProtocolType.Tcp);
        }
        #endregion


        #region Connect async
        /// <summary>
        /// Connects to the remote keyboard using the
        /// <see cref="KEYBOARD_EP"/>.
        /// </summary>
        public async Task ConnectAsync()
        {
            InstantiateSocket();
            if (_tcpSocket.Connected) return;

            // Connecting
            await _tcpSocket.ConnectAsync(KEYBOARD_EP);
            KeyboardConnected?.Invoke();
        }
        #endregion

        #region Disconnect
        /// <summary>
        /// Stops listening and closes the socket.
        /// </summary>
        public void Disconnect()
        {
            StopListening();
            _tcpSocket?.Dispose();
        }
        #endregion


        #region Start listening
        /// <summary>
        /// Starts listening for incoming messages.
        /// Raises the <see cref="KeyReceived"/> when it 
        /// receives one.
        /// </summary>
        public void StartListening()
        {
            if (_isListening)          return;
            if (!_tcpSocket.Connected) throw new KeyboardNotConnectedException();

            _tokenSource = new CancellationTokenSource();

            // Starting the listening task and
            // binding it with the _token.
            Task.Factory.StartNew(async () =>
            {
                using (_tokenSource)
                using (var stream = new StreamReader(new NetworkStream(_tcpSocket)))
                {
                    _isListening = true;
                    try
                    {
                        while (_tcpSocket.Connected)
                        {
                            // Raw key message ("key = ...")
                            string key =
                                KeyParser.Parse(await stream.ReadLineAsync());

                            await _tcpSocket.SendAsync(ACK, SocketFlags.None);

                            KeyReceived?.Invoke(key);
                        }
                    }
                    catch { }
                    finally
                    {
                        KeyboardDisconnected?.Invoke();
                        _isListening = false;
                    }
                }
            },
            _tokenSource.Token,
            TaskCreationOptions.LongRunning,
            TaskScheduler.Default);
        }
        #endregion

        #region Stop listening
        public void StopListening()
        {
            if (!_isListening) return;

            _tokenSource.Dispose();
            _isListening = false;
        }
        #endregion


        #region Dispose
        /// <summary>
        /// Closes the socket and stops the 
        /// listening tasks.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _tokenSource?.Dispose();
                _tcpSocket?.Dispose();
                _disposed = true;
            }
        }
        #endregion

        #region Complete
        public static void Complete()
        {
            _instance?.Dispose();
        }
        #endregion
    }
}
