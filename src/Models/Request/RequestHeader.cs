using System.Net;

namespace codecrafters_http_server.Helpers;

public record RequestHeader(string Name, string Value)
{
    public string Name { get; private set; } = Name;
    public string Value { get; private set; } = Value;
}