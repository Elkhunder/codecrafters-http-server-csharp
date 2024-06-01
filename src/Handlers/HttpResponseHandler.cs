using System.Net.Sockets;
using System.Text;

namespace codecrafters_http_server.Helpers;

public abstract class HttpResponseHandler()
{
    public static void Respond(Socket socket, HttpResponse httpResponse)
    {
        var buffer = Encoding.ASCII.GetBytes(httpResponse.ToString());
        socket.Send(new ArraySegment<byte>(buffer, 0, buffer.Length), SocketFlags.None);
    }
}