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
                              $"#####{nameof(HttpResponse)}: {httpResponse}");
            Console.WriteLine();
            var bytesSent = await socket.SendAsync(buffer, SocketFlags.None);
            Console.WriteLine($"Bytes Sent: {bytesSent}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}