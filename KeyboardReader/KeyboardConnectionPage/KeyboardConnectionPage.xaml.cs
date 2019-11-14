using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.Foundation.Metadata;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.Media.SpeechSynthesis;
using System.Threading;
using Windows.UI.Xaml.Media;
using Windows.Media.SpeechRecognition;

namespace KeyboardReader.Pages
{
    public sealed partial class KeyboardConnectionPage : Page
    {
        #region Fields

        /// <summary>
        /// The mqtt client singleton used by the app.
        /// </summary>
        private static MqttClient _mqttClient;

        /// <summary>
        /// The last typed text.
        /// </summary>
        private static string _lastText = string.Empty;

        /// <summary>
        /// The broker's hostname
        /// </summary>
        public static readonly string BROKER_HOSTNAME = "localhost";

        /// <summary>
        /// The topics that the client subscribes to.
        /// </summary>
        public static readonly string[] TOPICS = new string[]
        {
            "keyboard/key",
            "keyboard/state"
        };

        /// <summary>
        /// The quality of service for the topics.
        /// </summary>
        public static readonly byte[] QOS_LEVELS = new byte[]
        {
            MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,
            MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE
        };

        private static bool _connecting = false;

        /// <summary>
        /// The last message that has been received.
        /// Used by the <see cref="HandleSpeech(string)"/> method.
        /// </summary>
        private static string _lastMessage = string.Empty;


        /// <summary>
        /// Used to synthesize the written 
        /// words.
        /// </summary>
        private static SpeechSynthesizer _synthetizer;

        /// <summary>
        /// Controls the access to the 
        /// media element.
        /// </summary>
        private static EventWaitHandle _voiceHandle;

        #endregion

        #region Static methods

        /// <summary>
        /// Disconnects the mqtt client used to listen 
        /// for the keys.
        /// </summary>
        public static void DisconnectMqtt()
        {
            _mqttClient?.Disconnect();
        }

        #endregion


        #region Constructor
        public KeyboardConnectionPage()
        {
            InitializeComponent();

            typedText.Text = _lastText;
            InitMqtt();
        }
        #endregion

        #region Init MQTT
        private async void InitMqtt()
        {
            _mqttClient ??=
                new MqttClient(BROKER_HOSTNAME);

            _mqttClient.MqttMsgSubscribed      += MqttClientSubscribed;
            _mqttClient.MqttMsgPublishReceived += MqttPublishReceived;
            _mqttClient.ConnectionClosed       += MqttConnectionClosed;

            await ConnectMQTT();
        }
        #endregion

        #region Connect MQTT
        private async Task ConnectMQTT()
        {
            if (_mqttClient.IsConnected)
            {
                clearTextButton.Visibility = Visibility.Visible;
                buttonEnterAnimation.Begin();
                connectionPanel.Visibility = Visibility.Collapsed;
                return;
            }

            if (_connecting) return;

            _connecting = true;
            // Waiting for the client to connect
            while (!_mqttClient.IsConnected && _connecting)
            try
            {
                // Connecting
                await Task.Run(() =>
                    _mqttClient.Connect("KeyboardReader"));

                // Subscribing 
                _mqttClient.Subscribe(TOPICS, QOS_LEVELS);
            }
            catch { await Task.Delay(2000); }
        }
        #endregion


        #region Mqtt message received
        /// <summary>
        /// Displays the text or shows the 
        /// disconnection dialog.
        /// </summary>
        private async void MqttPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            await Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                switch (e.Topic)
                {
                    case "keyboard/key":
                        var msg =
                            Encoding.UTF8.GetString(e.Message);

                        typedText.Text += msg;
                        HandleSpeech(msg);
                        break;

                    case "keyboard/state":
                        await new KeyboardDisconnectedDialog().ShowAsync();
                        break;
                }
            });
        }
        #endregion

        #region Mqtt connection closed
        private async void MqttConnectionClosed(object sender, EventArgs e)
        {
            await Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, () =>
            {
                clearTextButton.Visibility = Visibility.Collapsed;
                buttonExitAnimation.Begin();
                connectionPanel.Visibility = Visibility.Visible;
                _connecting = false;

                // Reinstantiating the client
                _mqttClient = 
                    new MqttClient(BROKER_HOSTNAME);

                GC.Collect();
                InitMqtt();
            });
        }
        #endregion

        #region Mqtt client subscribed
        private async void MqttClientSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            await Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _connecting = false;
                connectionPanel.Visibility = Visibility.Collapsed;
                clearTextButton.Visibility = Visibility.Visible;
                buttonEnterAnimation.Begin();
            });
        }
        #endregion


        #region Initialize speech fields
        private void InitializeSpeechFields()
        {
            _voiceHandle ??=
                new EventWaitHandle(true, EventResetMode.AutoReset);

            _synthetizer  ??= new SpeechSynthesizer
            {
                Voice = SpeechSynthesizer
                .AllVoices
                .FirstOrDefault(vi =>
                    vi.Language == "it-IT" &&
                    vi.Gender == VoiceGender.Female)
                ??
                SpeechSynthesizer.DefaultVoice
            };
        }
        #endregion

        #region Enable next speech
        private void EnableNextSpeech(object sender, RoutedEventArgs e)
        {
            _voiceHandle.Set();
        }
        #endregion

        #region Handle speech
        /// <summary>
        /// Says the last word that has been received 
        /// as soon as a space is sent (or multiple spaces).
        /// </summary>
        /// <param name="message">
        /// The last message that has been received.
        /// </param>
        private async void HandleSpeech(string message)
        {
            _lastMessage += message;

            var words = 
                _lastMessage.Split(' ');

            if (words.Length <= 1) return;

            InitializeSpeechFields();

            foreach(var word in words)
            {
                if (string.IsNullOrWhiteSpace(word))
                    continue;

                var stream = 
                    await _synthetizer.SynthesizeTextToStreamAsync(word);

                // Waiting until we can speak again
                await Task.Run(_voiceHandle.WaitOne);

                _mediaElement.SetSource(stream, stream.ContentType);
                _mediaElement.Play();
            }

            _lastMessage = string.Empty;
        }
        #endregion


        #region Clear text
        /// <summary>
        /// Clears the text that has been typed.
        /// </summary>
        private void ClearText(object sender, RoutedEventArgs e)
        {
            typedText.Text = string.Empty;
        }
        #endregion


        #region On navigated from
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _mqttClient.MqttMsgSubscribed      -= MqttClientSubscribed;
            _mqttClient.MqttMsgPublishReceived -= MqttPublishReceived;
            _mqttClient.ConnectionClosed       -= MqttConnectionClosed;

            _lastText = typedText.Text;
            GC.Collect();
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Frees the unmanaged resources.
        /// </summary>
        public static void Dispose()
        {
            _synthetizer?.Dispose();
            _voiceHandle?.Dispose();
        }
        #endregion
    }
}
