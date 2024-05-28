using System.Net;
using System.Net.Sockets;
using System.Text;


var ipEndPoint = new IPEndPoint(IPAddress.Loopback, 4221);

// You can use print statements as follows for debugging, they'll be visible when running tests.
Console.WriteLine("Logs from your program will appear here!");
TcpListener? server = null;
try
{
    server = new TcpListener(ipEndPoint);
    
    server.Start();
    
    Console.WriteLine("Waiting for connection...");

    server.BeginAcceptSocket(AcceptConnectionCallback, server);
}
catch (SocketException e)
{
    Console.WriteLine("SocketException: {0}", e);
    server?.Stop();
}

Console.WriteLine("\nHitEnter to continue");
Console.Read();
return;

void AcceptConnectionCallback(IAsyncResult asyncResult)
{
    if (asyncResult.AsyncState is null) return;
    server = (TcpListener)asyncResult.AsyncState;

    var socket = server.EndAcceptSocket(asyncResult);

    Task.Run(() => HandleClientConnection(socket));
    
    server.BeginAcceptSocket(AcceptConnectionCallback, server);
}

void HandleClientConnection(Socket socket)
{
    var routes = new Dictionary<string, string>()
    {
        {"index", "/"},
        {"echo", "/echo"},
        {"user-agent", "/user-agent"},
    };
    var bytes = new byte[256];
    {
        Console.WriteLine("Connected!");

        // Get stream object for reading and writing
        var stream = new NetworkStream(socket);

        int i;

        // loop to receive all data
        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
        {
            var httpRequest = Encoding.ASCII.GetString(bytes, 0, i);
            Console.WriteLine("Received: {0}", httpRequest);
            var request = HandleRequest(httpRequest);

            var hostEndpoint = new IPEndPoint(request.Headers.Address, request.Headers.Port);
            if (hostEndpoint.Equals(ipEndPoint) && routes.ContainsValue(request.Resource))
            {
                
                if (request.Resource == routes["index"])
                {
                    var response = new HttpResponse("HTTP/1.1 200 OK");
                    var returnBuffer = Encoding.ASCII.GetBytes(response.ToString());    
                    stream.Write(returnBuffer, 0, returnBuffer.Length);
                }

                if (request.Resource == routes["user-agent"])
                {
                    // string response = "HTTP/1.1 200 OK\r\n" + // Status line (includes protocol version and status code)
                    //                   "Content-Type: text/plain\r\n" + // Content-Type header
                    //                   $"Content-Length: {request.Headers.UserAgent.Length}\r\n" + // Content-Length header
                    //                   "\r\n" + // Empty line indicates the end of headers
                    //                   request.Headers.UserAgent; // Response body content
                    HttpResponse response = new HttpResponse("HTTP/1.1 200 OK", request.Headers.UserAgent);
                    var returnBuffer = Encoding.ASCII.GetBytes(response.ToString());
                    stream.Write(returnBuffer, 0, returnBuffer.Length);
                }

                if (request.Resource == routes["echo"])
                {
                    // string response = "HTTP/1.1 200 OK\r\n" + // Status line (includes protocol version and status code)
                    //                   "Content-Type: text/plain\r\n" + // Content-Type header
                    //                   $"Content-Length: {request.Body.Length}\r\n" + // Content-Length header
                    //                   "\r\n" + // Empty line indicates the end of headers
                    //                   request.Body; // Response body content
                    var response = new HttpResponse("HTTP/1.1 200 OK", request.Body);
                    
                    var returnBuffer = Encoding.ASCII.GetBytes(response.ToString());
                    stream.Write(returnBuffer, 0, returnBuffer.Length);
                }
            }
            else
            {
                var response = new HttpResponse("HTTP/1.1 404 Not Found");
                var returnBuffer = Encoding.ASCII.GetBytes(response.ToString());
                stream.Write(returnBuffer, 0, returnBuffer.Length);
            }
        }
    }

    Request HandleRequest(string httpRequest)
    {
        var request = new Request();
        var requestLines = httpRequest.Split(["\n", "\r\n"], StringSplitOptions.RemoveEmptyEntries);
        var requestHeader = request.Headers;
    
        foreach (var line in requestLines)
        {
            // Check if the line contains a header, indicated by a colon
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
                request.Method = requestParts[0];
                request.Resource = requestParts[1];
                request.ProtocolVersion = requestParts[2];

                if (request.Resource.StartsWith(routes["echo"]))
                {
                    var resourceSplit = request.Resource.Split(new[]
                        { routes["echo"]}, 2, StringSplitOptions.RemoveEmptyEntries);

                    if (resourceSplit.Length > 0)
                    {
                        request.Body = resourceSplit[0][1..];
                        request.Resource = routes["echo"];
                    }
                }
            
                Console.WriteLine($"Method: {request.Method}, Resources: {request.Resource}, Protocol Version: {request.ProtocolVersion}, Request Body: {request.Body}");
            }
        }
    
        return request;
    }
}

public record HttpResponse(string Status, string? ContentType = null, int ContentLength = 0, string? Body = null)
{
    public HttpResponse(string status, string body) 
        : this(status, "text/plain", body.Length, body)
    {
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{Status}\r");
        sb.AppendLine("\r");
        if (Body == null) return sb.ToString();
        sb.AppendLine($"Content-Type: {ContentType}\r");
        sb.AppendLine($"Content-Length: {ContentLength}\r");
        sb.AppendLine();
        sb.Append(Body);
        return $"{Status}\r\n" + // Status line (includes protocol version and status code)"
               $"Content-Type: {ContentType}\r\n" + // Content-Type header
               $"Content-Length: {ContentLength}\r\n" + // Content-Length header
               "\r\n" + // Empty line indicates the end of headers
               Body; // Response body content
    }
}
public record Request
{
    public string Method { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string ProtocolVersion { get; set; } = string.Empty;
    public RequestHeader Headers { get; set; } = new RequestHeader();
}

public record RequestHeader
{
    public string UserAgent { get; set; } = string.Empty;
    public string Accept { get; set; } = string.Empty;
    public int Port { get; set; } = 0;
    public IPAddress Address { get; set; } = IPAddress.Any;
}

