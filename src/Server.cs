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
            var bytesRead = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);

            if (bytesRead is 0)
            {
                break;
            }

            var rawHttpRequest = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received: {rawHttpRequest}");

            HttpRequestParser.Parse(rawHttpRequest, out var requestLine, out var headers, out var requestBody);
            var httpRequestLine = HttpRequestParser.ParseRequestLine(requestLine);
            var httpRequestHeaders = HttpRequestParser.ParseRequestHeaders(headers);
            var httpRequestRoute = HttpRequestParser.ParseRoute(httpRequestLine.RequestUri, routes);
            var httpRequestFile = HttpRequestParser.ParseRequestFile(directoryPath, httpRequestRoute, httpRequestLine.GetHttpMethod(), requestBody);
            var httpRequestBody = HttpRequestParser.ParseRequestBody(requestBody);

            var httpRequest = new HttpRequest(httpRequestLine, httpRequestHeaders, httpRequestRoute, httpRequestBody, httpRequestFile);
            var responseStatusLine = HttpResponseBuilder.BuildStatusLine(httpRequestRoute, httpRequestLine.GetHttpMethod(), httpRequest);
            var responseEntity =
                HttpResponseBuilder.BuildResponseEntity(httpRequestRoute, httpRequest, httpRequestLine.GetHttpMethod());
            var httpResponse = HttpResponseBuilder.Build(responseStatusLine, responseEntity);
            await HttpResponseHandler.Respond(socket, httpResponse);
        }
    }
    catch (SocketException socketException)
    {
        Console.WriteLine($"Socket Exception: {socketException}");
        throw;
    }
}