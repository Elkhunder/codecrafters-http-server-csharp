using System.Net.Sockets;
using System.Text;

namespace codecrafters_http_server.Helpers;

public abstract class HttpResponseHandler()
{
    public static void Respond(Socket socket, HttpResponse httpResponse)
    {
        try
        {
            var buffer = Encoding.UTF8.GetBytes($"{httpResponse.ToString()}");
            Console.WriteLine($"###{nameof(HttpResponseHandler)}###\r\n" +
                              $"#####{nameof(HttpResponse)}#####" +
                              $"\r\n" +
                              $"{httpResponse}");
            Console.WriteLine();
            var bytesSent = socket.Send(buffer, SocketFlags.None);
            if (httpResponse.ResponseEntity.Body is not null)
            {
                socket.Send(httpResponse.ResponseEntity.Body, 0, httpResponse.ResponseEntity.Body.Length, SocketFlags.None);
            }
            Console.WriteLine($"Bytes Sent: {bytesSent}");

            if (!socket.Connected) return;
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}