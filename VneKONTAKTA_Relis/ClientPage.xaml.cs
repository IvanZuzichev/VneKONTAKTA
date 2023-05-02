using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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
    public partial class ClientPage : Page
    {
        private Socket server;
        public ClientPage(string Txt, string name)
        {
            InitializeComponent();

            try
            {
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.ConnectAsync(Txt, 8888);
                SendMessage("/u " +name);
                RecievMessage();
            }
            catch
            {
                MessageBox.Show("Вы должны выйти из программы");
            }
        }
        private async Task RecievMessage()
        {
            while (true)
            {
                DateTime localDate = DateTime.Now;
                byte[] bytes = new byte[1024];
                await server.ReceiveAsync(bytes, SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);

                MessagesLbx.Items.Add(message);
            }
        }
        private async Task SendMessage(string message)
        {
            if (MessageTxt.Text != "/disconnect")
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                await server.SendAsync(bytes, SocketFlags.None);
            }
            else
            {
                MessageTxt.Text = "";
                (Application.Current.MainWindow as MainWindow).perexod.Content = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime localDate = DateTime.Now;
          
            string message = MessageTxt.Text;
            SendMessage(message);
            MessageTxt.Text = "";
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MainWindow).perexod.Content = null;
        }
    }
}
