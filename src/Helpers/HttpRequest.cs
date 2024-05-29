using System.Net;

namespace codecrafters_http_server.Helpers;

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