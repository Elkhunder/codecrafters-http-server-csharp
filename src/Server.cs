using System.Net;
using System.Net.Sockets;
using System.Text;

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");

//Uncomment this block to pass the first stage
TcpListener server = new TcpListener(IPAddress.Any, 4221);
//
server.Start();
Byte[] bytes = new Byte[256];
String data == null;

// Enter listening loop
while (true)
{
    Console.WriteLine("Waiting for connection...");
    using var socket = server.AcceptSocket();
    Console.WriteLine("Connected!");

    data = null;
    
    // Get stream object for reading and writing
    var stream = new NetworkStream(socket);

    int i;
    
    // loop to receive all data
    while ((i = stream.Read(bytes, 0, bytes.Length))!=0)
    {
        data = Encoding.ASCII.GetString(bytes, 0, i);
        Console.WriteLine("Received: {0}", data);

        data = data.ToUpper();

        byte[] msg = Encoding.ASCII.GetBytes(data);
    }
}
socket.Send(Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n\r\n"));
