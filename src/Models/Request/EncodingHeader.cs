using System.Net;

namespace codecrafters_http_server.Helpers;

public record EncodingHeader(HttpRequestHeader HeaderName, EncodingProvider EncodingProvider) : IRequestHeader
{
}