using System.Net.Sockets;
using System.Text;

namespace codecrafters_http_server.Helpers;

public abstract class HttpResponseHandler()
{
    public static void Respond(Socket socket, HttpResponse response)
    {
        var buffer = Encoding.ASCII.GetBytes(response.ToString());
        socket.Send(new ArraySegment<byte>(buffer, 0, buffer.Length), SocketFlags.None);
    }
}