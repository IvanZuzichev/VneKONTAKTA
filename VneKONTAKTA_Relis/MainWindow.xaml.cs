using System;
using System.Collections.Generic;
using System.Linq;
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

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ToServer_Click(object sender, RoutedEventArgs e)
        {
            if (serverTxt.Text != "")
            {
                perexod.Content = new ServerPage();
                serverTxt.Text = "";
            }
            else
            {
                MessageBox.Show("Вы не ввели имя пользователя");            
            }
        }
        private void ToClient_Click(object sender, RoutedEventArgs e)
        {
            if (clientTxt.Text != "")
            {
                perexod.Content = new ClientPage(serverTxt.Text, clientTxt.Text);
                clientTxt.Text = "";
            }
            else
            {
                MessageBox.Show("Вы не ввели IP адрес чата");
            }
        }

    }
}
