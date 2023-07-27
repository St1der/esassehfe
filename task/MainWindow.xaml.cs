using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
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

namespace task
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var ipAddress = IPAddress.Parse("10.2.27.4");
            var port = 90;
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var ep = new IPEndPoint(ipAddress, port);
                socket.Connect(ep);
                socket.Listen(5);
                datagrid.Items.Add ($"Listen over {socket.LocalEndPoint}");
                while (true)
                {
                    var client = socket.Accept();
                    Task.Run(() =>
                    {
                        datagrid.Items.Add($"{client.RemoteEndPoint} connected");

                        var length = 0;
                        var bytes = new byte[1024];

                        do
                        {
                            length = client.Receive(bytes);
                            var msg = Encoding.UTF8.GetString(bytes, 0, length);
                            datagrid.Items.Add  ($"CLIENT : {client.RemoteEndPoint} : {msg}"); ;
                            if (msg == "exit")
                            {
                                client.Shutdown(SocketShutdown.Both);
                                client.Dispose();
                                break;
                            }
                        } while (true);




                    });
                }
            }
        }
    }
}
