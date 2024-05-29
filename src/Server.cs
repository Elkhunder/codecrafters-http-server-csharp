using System.Net;
using System.Net.Sockets;
using System.Text;
using codecrafters_http_server.Helpers;

var port = 4221;
var ipEndPoint = new IPEndPoint(IPAddress.Loopback, port);

var routes = new Dictionary<string, Routes>()
{
    {"/", Routes.Default},
    {"/echo", Routes.Echo},
    {"/user-agent", Routes.UserAgent},
    {"/files", Routes.Files},
};

var arguments = Environment.GetCommandLineArgs();
var directoryPath = string.Empty;
if (arguments.Length == 3)
{
    directoryPath = arguments[2];
}
// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");
TcpListener? server = null;
try
{
    server = new TcpListener(ipEndPoint);

    server.Start();
    Console.WriteLine($"Server is listening on port: {port}");

    while (true)
    {
        Console.WriteLine("Waiting for connection...");

        var clientSocket = await server.AcceptSocketAsync();
        _ = Task.Run(async () => await HandleClientAsync(clientSocket));
    }
}
catch (Exception e)
{
    Console.WriteLine($"Exception: {e.Message}");
}
finally
{
    server?.Stop();
}

return;


async Task HandleClientAsync(Socket socket)
{
    try
    {
        var buffer = new byte[1_024];

        while (true)
        {
            var bytesRead = await socket.ReceiveAsync(buffer, SocketFlags.None);

            if (bytesRead is 0)
            {
                break;
            }

            var rawHttpRequest = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received: {rawHttpRequest}");

            var httpRequest = HttpRequestParser.Parse(rawHttpRequest, routes);
            var response = HttpResponseBuilder.Build(httpRequest, directoryPath);
            HttpResponseHandler.Respond(socket, response);
        }
    }
    catch (SocketException socketException)
    {
        Console.WriteLine($"Socket Exception: {socketException}");
        throw;
    }
}