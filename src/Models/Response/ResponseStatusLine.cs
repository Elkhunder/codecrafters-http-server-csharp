using System.Net;
using System.Text;

namespace codecrafters_http_server.Helpers;

public record ResponseStatusLine(HttpStatusCode StatusCode, string ProtocolVersion = "HTTP/1.1")
{
    public string ProtocolVersion { get; set; } = ProtocolVersion;
    public HttpStatusCode StatusCode { get; set; } = StatusCode;
    public string ReasonPhrase { get; set; } = StatusCode.ToString();
    
    public void Deconstruct(
        out string protocolVersion,
        out int statusCode,
        out string reasonPhrase)
    {
        protocolVersion = ProtocolVersion;
        statusCode = (int)StatusCode;
        reasonPhrase = ReasonPhrase;
    }
    
    public void Deconstruct(
        out string protocolVersion,
        out HttpStatusCode statusCode,
        out string reasonPhrase)
    {
        protocolVersion = ProtocolVersion;
        statusCode = StatusCode;
        reasonPhrase = ReasonPhrase;
    }

    public override string ToString()
    {
        if (StatusCode is HttpStatusCode.NotFound)
        {
            return new StringBuilder()
                .Append($"{ProtocolVersion} {(int)StatusCode} {ReasonPhrase.Insert(3, " ")}\r\n")
                .ToString();
        }
        return new StringBuilder()
            .Append($"{ProtocolVersion} {(int)StatusCode} {ReasonPhrase}\r\n")
            .ToString();
    }
};