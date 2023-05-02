using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VneKONTAKTA_Relis
{
    public partial class ServerPage : Page
    {
        private CancellationTokenSource ServerToken;
        private Socket socket;
        private List<Socket> clients = new List<Socket>();
        // сервер
        private Socket server;
        public ServerPage()
        {
            InitializeComponent();

            try
                {
                PeopleList.Items.Add("Админ");
                ServerToken = new CancellationTokenSource();
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(ipPoint);
                socket.Listen(1000);

                ListenToClients(ServerToken.Token);
            }
            catch
            {
                MessageBox.Show("Вы должны выйти из программы");
            }
        }
        private async Task ListenToClients(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var client = await socket.AcceptAsync();
                
                clients.Add(client);
                RecievMessage(client, token);

            }
        }
        private async Task RecievMessage(Socket client, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                 DateTime localDate = DateTime.Now;
                byte[] bytes = new byte[1024];
                await client.ReceiveAsync(bytes, SocketFlags.None);
                
                string message = Encoding.UTF8.GetString(bytes);
                if (message.StartsWith("/u "))
                {
                    string namex = message.Substring(2);
                    PeopleList.Items.Add(namex);
                }
                string messagefull = $"[Сообщение от {client.RemoteEndPoint}] {localDate}: {message}";
                MessagesLbx.Items.Add(messagefull);

        
                    SendMessage( messagefull);
                
            }
        }
        public async Task SendMessage( string messagefull)
        {
            foreach (var item in clients)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(messagefull);
                await item.SendAsync(bytes, SocketFlags.None);
            }
        }
        private void Send_Click(object sender, RoutedEventArgs e )
        {
            DateTime localDate = DateTime.Now;
            string message = $"[Сообщение от Админ ] {localDate.ToString()}: {MessageTxt.Text}";
            Send(message);
        }
        private void Send(string message)
        {
            MessagesLbx.Items.Add(message);
            SendMessage(message);
            MessageTxt.Text = "";
        }
        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            ServerToken.Cancel();
            (Application.Current.MainWindow as MainWindow).perexod.Content = null;
        }
    }
}
