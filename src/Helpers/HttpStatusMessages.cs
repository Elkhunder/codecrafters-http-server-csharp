using System.Net;

namespace codecrafters_http_server.Helpers;

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