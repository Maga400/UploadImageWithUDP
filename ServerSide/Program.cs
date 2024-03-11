using System.Drawing.Imaging;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;

var listener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

var IP = IPAddress.Parse("127.0.0.1");
var port = 27001;

var listenerEP = new IPEndPoint(IP, port);
EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

listener.Bind(listenerEP);

string? message = null;
var buffer = new byte[ushort.MaxValue - 29];
int len = 0;

Console.WriteLine($@"Local EP => {listener.LocalEndPoint} ----- Message => Serverimiz ise basladi");
while (true)
{
    var result = await listener.ReceiveFromAsync(new ArraySegment<byte>(buffer), SocketFlags.None, remoteEP);

    len = result.ReceivedBytes;
    message = Encoding.UTF8.GetString(buffer, 0, len);
    Console.WriteLine($@"Remote EP => {result.RemoteEndPoint} ----- Message => {message}");

    Task a = Task.Factory.StartNew(() =>
    {


        int screenWidth = 50;
        int screenHeight = 50;

        using (Bitmap screenshot = new Bitmap(screenWidth, screenHeight))
        {

            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.FillRectangle(Brushes.White, 0, 0, screenWidth, screenHeight);
            }


            byte[] imageData;
            using (MemoryStream stream = new MemoryStream())
            {
                screenshot.Save(stream, ImageFormat.Png);
                imageData = stream.ToArray();
            }

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27001);
            listener.SendToAsync(new ArraySegment<byte>(imageData), SocketFlags.None, result.RemoteEndPoint);
            //client.Send(imageData, imageData.Length, endPoint);

        }

    });
    await a;
}










