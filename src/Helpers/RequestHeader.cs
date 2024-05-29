using System.Net;

namespace codecrafters_http_server.Helpers;

public record RequestHeader
{
    public string UserAgent { get; set; } = string.Empty;
    public string Accept { get; set; } = string.Empty;
    public int Port { get; set; } = 0;
    public IPAddress Address { get; set; } = IPAddress.Any;
}