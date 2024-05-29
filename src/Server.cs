using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

var port = 4221;
var ipEndPoint = new IPEndPoint(IPAddress.Loopback, port);

var routes = new Dictionary<string, Routes>()
{
    {"/", Routes.Default},
    {"/echo", Routes.Echo},
    {"/user-agent", Routes.UserAgent},
    {"/files", Routes.Files},
};

string[] arguments = Environment.GetCommandLineArgs();
string directoryPath = string.Empty;

if (arguments.Length > 0)
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
        _ = HandleClientAsync(clientSocket);
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
            await HttpResponseHandler.Respond(socket, response);
        }
    }
    catch (SocketException socketException)
    {
        Console.WriteLine($"Socket Exception: {socketException}");
        throw;
    }
}

public abstract class HttpRequestParser()
{
    public static HttpRequest Parse(string rawHttpRequest, Dictionary<string, Routes> routes)
    {
        var httpRequest = new HttpRequest();
        var requestLines = rawHttpRequest.Split(["\n", "\r\n"], StringSplitOptions.RemoveEmptyEntries);
        var requestHeader = httpRequest.Headers;
        
        foreach (var line in requestLines)
        {
            if (line.Contains(':'))
            {
                // Split the line into header name and value
                var headerParts = line.Split([":"], 2, StringSplitOptions.None);
                switch (headerParts[0])
                {
                    case "Host":
                        var hostParts = headerParts[1].Split(':');
                        var hostname = hostParts[0].Trim();

                        if (hostname.Equals("localhost", StringComparison.InvariantCultureIgnoreCase))
                        {
                            requestHeader.Address = IPAddress.Loopback;
                        }
                        else
                        {
                            if (IPAddress.TryParse(hostname, out var address))
                            {
                                requestHeader.Address = address;
                            }
                        }

                        Console.WriteLine($"Added IP Address to Request Header: {requestHeader.Address}");

                        if (int.TryParse(hostParts[1], out var port))
                        {
                            requestHeader.Port = port;
                            Console.WriteLine($"Added IP Port to Request Header: {requestHeader.Port}");
                        }

                        break;
                    case "User-Agent":
                        requestHeader.UserAgent = headerParts[1].Trim();
                        Console.WriteLine($"Added User Agent to Request Header: {requestHeader.UserAgent}");
                        break;
                    case "Accept":
                        requestHeader.Accept = headerParts[1];
                        Console.WriteLine($"Added Accept to Request Header: {requestHeader.Accept}");
                        break;
                }

            }
            else
            {
                // Assuming this is the request line, split it by spaces
                var requestParts = line.Split(' ');
                if (requestParts.Length < 3) continue;
                httpRequest.Method = requestParts[0];
                SplitPath(requestParts[1], out var httpResource, out var content);
                httpRequest.ProtocolVersion = requestParts[2];
                if (routes.TryGetValue(httpResource, out var route))
                {
                    Console.WriteLine($"Path '{httpResource}' maps to Routes.{route}");
                    httpRequest.Route = route;
                    httpRequest.Body = content;
                }
                else
                {
                    Console.WriteLine($"Path: '{httpResource}' does not match any available route");
                }

                Console.WriteLine(
                    $"Method: {httpRequest.Method}, Route: {httpRequest.Route}, Protocol Version: {httpRequest.ProtocolVersion}, Request Body: {httpRequest.Body}");
            }
        }
        return httpRequest;
    }

    private static void SplitPath(string path, out string resource, out string content)
    {
        resource = string.Empty;
        content = string.Empty;

        path = path.TrimStart('/');
        var segments = path.Split('/');
        resource = $"/{segments[0]}";
        content = segments[1];
    }
}

public abstract class HttpResponseHandler()
{
    public static async Task Respond(Socket socket, HttpResponse response)
    {
        var buffer = Encoding.ASCII.GetBytes(response.ToString());
        await socket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), SocketFlags.None);
    }
}

public abstract class HttpResponseBuilder()
{
    public static HttpResponse Build(HttpRequest httpRequest, string directoryPath)
    {
        if (!string.IsNullOrEmpty(directoryPath))
        {
            var filePath = Path.Combine(directoryPath, httpRequest.Body);
            if (File.Exists(filePath))
            {
                Console.WriteLine("File Exists");
                var fileContent = File.ReadAllText(filePath);
                Console.WriteLine($"File Contents: {fileContent}");
                httpRequest.StatusCode = (int)HttpStatusCode.OK;
                httpRequest.Body = fileContent;
                Console.WriteLine($"Http Body: {httpRequest.Body} - Should match file contents");
            }
            else
            {
                httpRequest.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
        return httpRequest.Route switch
        {
            Routes.Default => new HttpResponse($"{httpRequest.ProtocolVersion} {httpRequest.StatusCode = (int)HttpStatusCode.OK} {httpRequest.StatusMessage}"),
            Routes.Echo => new HttpResponse($"{httpRequest.ProtocolVersion} {httpRequest.StatusCode = (int)HttpStatusCode.OK} {httpRequest.StatusMessage}", httpRequest.Body),
            Routes.UserAgent => new HttpResponse($"{httpRequest.ProtocolVersion} {httpRequest.StatusCode = (int)HttpStatusCode.OK} {httpRequest.StatusMessage}", httpRequest.Headers.UserAgent),
            Routes.Files => new HttpResponse($"{httpRequest.ProtocolVersion} {httpRequest.StatusCode = (int)HttpStatusCode.OK} {httpRequest.StatusMessage}", ContentType: "application/octet-stream" , Body: httpRequest.Body),
            _ => new HttpResponse($"{httpRequest.ProtocolVersion} {httpRequest.StatusCode = (int)HttpStatusCode.NotFound} {httpRequest.StatusMessage}"),
        };
    }
}

public record HttpResponse(string? Status = null, string? ContentType = null, int ContentLength = 0, string? Body = null)
{
    public HttpResponse(string status, string body) 
        : this(status, "text/plain", body.Length, body)
    {
    }

    public HttpResponse(string status)
        : this(status, null, 0, null)
    {
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        if (Body == null)
        {
            sb.AppendLine($"{Status}\r");
            sb.AppendLine("\r");
            return sb.ToString();
        }
        sb.AppendLine($"{Status}\r");
        sb.AppendLine($"Content-Type: {ContentType}\r");
        sb.AppendLine($"Content-Length: {ContentLength}\r");
        sb.AppendLine("\r");
        sb.Append(Body);
        return sb.ToString();
    }
}

public abstract record HttpStatusMessages
{
    private static readonly Dictionary<HttpStatusCode, string> StatusMessages = new Dictionary<HttpStatusCode, string>
    {
        {HttpStatusCode.OK, "OK"},
        {HttpStatusCode.NotFound, "Not Found"},
    };

    public static string GetStatusMessage(HttpStatusCode statusCode)
    {
        return StatusMessages.FirstOrDefault(pair => pair.Key == statusCode).Value;
    }
}

public enum Routes {Default, Echo, UserAgent, Files}
public record HttpRequest()
{
    private int _statusCode;
    public string Method { get; set; } = string.Empty;
    public string StatusMessage { get; private set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string ProtocolVersion { get; set; } = string.Empty;
    public Routes Route { get; set; }
    public RequestHeader Headers { get; set; } = new();

    public int StatusCode
    {
        get => _statusCode;
        set
        {
            _statusCode = value;
            StatusMessage = HttpStatusMessages.GetStatusMessage((HttpStatusCode)_statusCode);
        }
    }
}

public record RequestHeader
{
    public string UserAgent { get; set; } = string.Empty;
    public string Accept { get; set; } = string.Empty;
    public int Port { get; set; } = 0;
    public IPAddress Address { get; set; } = IPAddress.Any;
}

