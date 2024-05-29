using System.Net;

namespace codecrafters_http_server.Helpers;

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
                Console.WriteLine($"Http Body Length: {httpRequest.Body.Length}");
            }
            else
            {
                Console.WriteLine("File Does Not Exist");
                httpRequest.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }

        if (httpRequest.Route is Routes.Default)
        {
            var response =
                new HttpResponse(
                    $"{httpRequest.ProtocolVersion} {httpRequest.StatusCode = (int)HttpStatusCode.OK} {httpRequest.StatusMessage}");
            Console.WriteLine($"Response: {response}");
        }
        return httpRequest.Route switch
        {
            Routes.Default => new HttpResponse($"{httpRequest.ProtocolVersion} {httpRequest.StatusCode = (int)HttpStatusCode.OK} {httpRequest.StatusMessage}"),
            Routes.Echo => new HttpResponse($"{httpRequest.ProtocolVersion} {httpRequest.StatusCode = (int)HttpStatusCode.OK} {httpRequest.StatusMessage}","text/plain" ,httpRequest.Body),
            Routes.UserAgent => new HttpResponse($"{httpRequest.ProtocolVersion} {httpRequest.StatusCode = (int)HttpStatusCode.OK} {httpRequest.StatusMessage}", "text/plain" ,httpRequest.Headers.UserAgent),
            Routes.Files => new HttpResponse($"{httpRequest.ProtocolVersion} {httpRequest.StatusCode} {httpRequest.StatusMessage}", "application/octet-stream" , httpRequest.Body),
            Routes.NotFound => new HttpResponse($"{httpRequest.ProtocolVersion} {httpRequest.StatusCode = (int)HttpStatusCode.NotFound} {httpRequest.StatusMessage}"),
        };
    }
}