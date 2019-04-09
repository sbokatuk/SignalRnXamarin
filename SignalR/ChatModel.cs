using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SignalR
{
    public class ChatModel : INotifyPropertyChanged
    {
        public const string DefaultApi = "http://10.10.20.21:3000";

        HubConnection hubConnection;

        public string UserName { get; set; }
        public string Message { get; set; }
        // список всех полученных сообщений
        public ObservableCollection<MessageData> Messages { get; }

        // идет ли отправка сообщений
        bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }
        // осуществлено ли подключение
        bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                if (isConnected != value)
                {
                    isConnected = value;
                    OnPropertyChanged("IsConnected");
                }
            }
        }

        public ChatModel()
        {
            // создание подключения
            hubConnection = new HubConnectionBuilder().ConfigureLogging(
                (Microsoft.Extensions.Logging.ILoggingBuilder logging) =>
                {
                    logging.AddProvider(new CustomLoggerProvider(new PingMessageTracker(1900, HandleUnPingAction)));
                    logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Trace);
                    logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Trace);
                })
                .WithUrl($"{DefaultApi}/chatHub")
                .Build();
            hubConnection.KeepAliveInterval = TimeSpan.FromMilliseconds(600);
            hubConnection.HandshakeTimeout = TimeSpan.FromMilliseconds(600);
            hubConnection.ServerTimeout = TimeSpan.FromMilliseconds(2500);
            Messages = new ObservableCollection<MessageData>();

            IsConnected = false;    // по умолчанию не подключены
            IsBusy = false;         // отправка сообщения не идет

            //SendMessageCommand = new Command(async () => await SendMessage(), () => IsConnected);

            hubConnection.Closed += HubConnection_Closed;

            hubConnection.On("ConnectionSlow", ConnectionSlow);
            hubConnection.On("Reconnecting", Reconnecting);
            hubConnection.On("Reconnected", Reconnected);
            hubConnection.On("StateChanged", StateChanged);
            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                SendLocalMessage(user, message);
            });
        }

        void HandleUnPingAction()
        {
            SendLocalMessage(String.Empty, "Delay Detected...");
        }


        private void Reconnecting()
        {
            SendLocalMessage(String.Empty, "Reconnecting...");
        }

        private void Reconnected()
        {
            SendLocalMessage(String.Empty, "Reconnected...");
        }

        private void StateChanged()
        {
            SendLocalMessage(String.Empty, "State Changed...");
        }

        private void ConnectionSlow()
        {
            SendLocalMessage(String.Empty, "Connection Slow...");
        }

        async Task HubConnection_Closed(Exception arg)
        {
            SendLocalMessage(String.Empty, "Hub Connection Closed...");
            IsConnected = false;
            await Connect();
        }

        // подключение к чату
        public async Task Connect()
        {
            if (IsConnected)
                return;
            try
            {
                await hubConnection.StartAsync();
                SendLocalMessage(String.Empty, "Connection started...");

                IsConnected = true;
            }
            catch (Exception ex)
            {
                SendLocalMessage(String.Empty, $"Connection started fail: {ex.Message}");
            }
        }

        // Отключение от чата
        public async Task Disconnect()
        {
            if (!IsConnected)
                return;
            hubConnection.StopAsync();
            IsConnected = false;
            SendLocalMessage(String.Empty, "Disconnected...");
        }

        // Отправка сообщения
        public async Task SendMessage()
        {
            try
            {
                SendLocalMessage(String.Empty, $"Sending Message");
                IsBusy = true;
                await hubConnection.SendAsync("SendMessage", "Mobile", Message);
            }
            catch (Exception ex)
            {
                SendLocalMessage(String.Empty, $"Send Message fail: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
        // Добавление сообщения
        private void SendLocalMessage(string user, string message)
        {
            Messages.Insert(0, new MessageData
            {
                Message = message,
                User = user.AddTimeStamp()
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
