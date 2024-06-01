using System.Net.Sockets;
using System.Text;

namespace codecrafters_http_server.Helpers;

public abstract class HttpResponseHandler()
{
    public static async Task Respond(Socket socket, HttpResponse httpResponse)
    {
        try
        {
            var buffer = Encoding.UTF8.GetBytes(httpResponse.ToString());
            var bytesSent = await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), SocketFlags.None);
            Console.WriteLine($"Bytes Sent: {bytesSent}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}