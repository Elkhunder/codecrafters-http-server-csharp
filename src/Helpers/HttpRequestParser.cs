using System.Net;

namespace codecrafters_http_server.Helpers
{
    public static class HttpRequestParser
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
                    httpRequest.Route = Routes.NotFound;
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
        resource = segments.Length > 0 ? $"/{segments[0]}" : string.Empty;
        content = segments.Length > 1 ? segments[1] : string.Empty;
    }
}
}
    

