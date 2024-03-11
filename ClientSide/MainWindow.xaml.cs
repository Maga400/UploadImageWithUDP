using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ClientSide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GetStart(object sender, RoutedEventArgs e)
        {
            await Task.Delay(1000);
            await LoadImage();

        }

        public async Task LoadImage()
        {
            Task.Delay(1000);
            var client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var IP = IPAddress.Parse("127.0.0.1");
            var port = 27001;
            EndPoint clientEP = new IPEndPoint(IP, port);
            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            var buffer = new byte[1024];
            var buffer2 = new byte[ushort.MaxValue - 29];

            while (true)
            {
                buffer = Encoding.UTF8.GetBytes("Request Servere atildi.Sekil cekilmeye baslayacaq.");
                await client.SendToAsync(new ArraySegment<byte>(buffer), clientEP);

                var d = Task.Factory.StartNew(() =>
                {

                    var imageData = client.ReceiveFrom(buffer2, SocketFlags.None, ref remoteEP);

                    using (MemoryStream stream = new MemoryStream(imageData))
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = stream;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();

                        MessageBox.Show(image.Width.ToString());

                        ScreenImage.Source = image;

                    }

                });
            }
        }
    }
}