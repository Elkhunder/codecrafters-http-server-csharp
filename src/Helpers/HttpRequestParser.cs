using codecrafters_http_server.Handlers;

namespace codecrafters_http_server.Helpers
{
    public static class HttpRequestParser
    {
    public static void Parse(string rawHttpRequest, out string requestLine, out string[] headers, out string requestBody)
    {
        var sections = rawHttpRequest.Split(["\r\n\r\n"], StringSplitOptions.RemoveEmptyEntries);
        var requestLineAndHeaders = sections[0];
        requestBody = sections.Length > 1 ? sections[1] : string.Empty;
        var lines = requestLineAndHeaders.Split(["\r\n"], StringSplitOptions.RemoveEmptyEntries);
        requestLine = lines[0];
        headers = lines.Skip(1).ToArray();
        

        // ParseRequestLine(lines[0], httpRequest, routes);
        // ParseRoute(httpRequest.RequestLine.RequestUri, routes);
        // ParseRequestHeaders(headers);
        // ParseRequestFile();
        // ParseRequestBody();
    }

    private static void SplitUrl(string url, out string path, out string pathParameter)
    {
        path = string.Empty;
        pathParameter = string.Empty;

        url = url.TrimStart('/');
        var segments = url.Split('/');
        path = segments.Length > 0 ? $"/{segments[0]}" : string.Empty;
        pathParameter = segments.Length > 1 ? segments[1] : string.Empty;
    }

    public static Route ParseRoute(string requestUri, Dictionary<string, Routes> routes)
    {
        SplitUrl(requestUri, out var path, out var parameter);
        if (routes.TryGetValue(path, out var route))
        {
            return new Route(route, parameter);
        }

        return new Route(Routes.NotFound, parameter);
    }
    public static RequestLine ParseRequestLine(string requestLine)
    {
        var lineSegments = requestLine.Split(' ');
        if (lineSegments.Length >= 3)
        {
            return new RequestLine
            (
                lineSegments[0],
                lineSegments[1],
                lineSegments[2]
            );
        }
        
        return RequestLine.Empty();
    }
    public static List<RequestHeader> ParseRequestHeaders(string[] headers)
    {
        return headers.Select
        (
            header => 
                header.Split([":"], 2, StringSplitOptions.None)).Select(headerParts => 
            new RequestHeader(headerParts[0].Trim(), headerParts[1].Trim())
        ).ToList();
    }

    public static RequestFile ParseRequestFile(string directory, Route route, HttpMethod method, string contents)
    {
        Console.WriteLine($"Route: {route.Path}, Method: {method}, Directory: {directory}, File: {route.Parameter}");
        if (route.Path != Routes.Files) return RequestFile.Empty;
        var fileName = route.Parameter;
        var fileHandler = new FileHandler(directory, fileName);            
        if (method == HttpMethod.Get)
        {
            contents = fileHandler.GetFileContents();
            Console.WriteLine($"{nameof(ParseRequestFile)}-{nameof(HttpMethod.Get)} {nameof(FileHandler.ValidateFile)}: {fileHandler.FileValidated} {nameof(contents)}: {contents}, {nameof(contents.Length)}: {contents.Length}");
            return new RequestFile(directory, fileName, contents, fileHandler.FileValidated);
        }
        else if (method == HttpMethod.Post)
        {
            fileHandler.SaveFileContents(contents);
            Console.WriteLine($"File Saved!  File-Contents: {contents}");
            return new RequestFile(directory, fileName, string.Empty, fileHandler.FileValidated);
        }

        return RequestFile.Empty;
    }
    public static string ParseRequestBody(string requestBody)
    {
        return requestBody.Length switch
        {
            0 => string.Empty,
            _ => requestBody
        };
    }
}
}
    

