using System.Net.Sockets;
using System.Text;

namespace codecrafters_http_server.Helpers;

public abstract class HttpResponseHandler()
{
    public static async Task Respond(Socket socket, HttpResponse httpResponse)
    {
        try
        {
            var buffer = Encoding.ASCII.GetBytes(httpResponse.ToString());
            Console.WriteLine($"###{nameof(HttpResponseHandler)}" +
                              $"#####\r\n" +
                              $"#####{nameof(HttpResponse)}: {httpResponse}" +
                              $"#####\r\n" +
                              $"#####{nameof(Encoding.UTF8)}-{nameof(Encoding.UTF8.GetBytes)}: {buffer}" +
                              $"#####\r\n" +
                              $"#####{nameof(ArraySegment<byte>)}: {new ArraySegment<byte>(buffer).ToString()}");
            var sb = new StringBuilder();
            foreach (var b in  buffer)
            {
                sb.Append($"{b}\r\n");
            }
            Console.WriteLine(sb);
            var bytesSent = await socket.SendAsync(buffer, SocketFlags.None);
            Console.WriteLine($"Bytes Sent: {bytesSent}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}