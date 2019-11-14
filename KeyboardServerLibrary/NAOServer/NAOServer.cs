using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardReaderLibrary.NAOServer
{
    public class NAOServer : IDisposable
    {
        #region Singleton instance

        private static NAOServer _instance;

        /// <summary>
        /// Returns the current instance of the <see cref="NAOServer"/>
        /// singleton.
        /// </summary>
        public static NAOServer GetInstance()
        {
            if (_instance == null)
                _instance = new NAOServer();

            return _instance;
        }

        #endregion

        #region Fields

        private Socket _client;

        /// <summary>
        /// The server's EndPoint.
        /// </summary>
        public static readonly IPEndPoint NAO_END_POINT = 
            new IPEndPoint(
                IPAddress.Parse("192.168.88.247"),
                40_000);

        #endregion


        #region Events

        public delegate void ConnectedHandler();
        public delegate void DisconnectedHandler();
        public delegate void MessageReceivedHandler(string message);

        /// <summary>
        /// Raised when the NAO connects to the server.
        /// </summary>
        public event ConnectedHandler Connected;

        /// <summary>
        /// Raised when the NAO disconnects from the server.
        /// </summary>
        public event DisconnectedHandler Disconnected;

        /// <summary>
        /// Raised when the NAO sends a new message.
        /// </summary>
        public event MessageReceivedHandler MessageReceived;

        #endregion


        #region Constructor
        /// <summary>
        /// Instantiates the client socket.
        /// </summary>
        private NAOServer()
        {
            _client =
                new Socket(SocketType.Stream, ProtocolType.Tcp);
        }
        #endregion


        #region Try connect async
        /// <summary>
        /// Tries to connect to the NAO asynchronously.
        /// </summary>
        public async Task<bool> TryConnectAsync()
        {
            try
            {
                await 
                    _client.ConnectAsync(NAO_END_POINT);

                return true;
            }
            catch { return false; }
        }
        #endregion

        #region Start receiving async
        /// <summary>
        /// Starts receiving.
        /// Raises the <see cref="MessageReceived"/> event
        /// when a new message is received.
        /// </summary>
        public async void StartReceivingAsync()
        {
            // Getting the stream
            var stream =
                new StreamReader(new NetworkStream(_client));

            using (stream)
            while (true)
            {
                MessageReceived?.Invoke( 
                    await stream.ReadLineAsync());
            }
        }
        #endregion

        #region Send async
        /// <summary>
        /// Sends a message to the NAO asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task TrySendAsync(string message)
        {
            await _client.SendAsync(
                Encoding.UTF8.GetBytes(message),
                SocketFlags.None);
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            _client?.Dispose();
        }
        #endregion
    }
}
