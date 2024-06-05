using System.Net;

namespace codecrafters_http_server.Helpers;

public record RequestHeader(string Name, string Value)
{
    private HttpRequestHeader? _httpRequestHeader;

    public HttpRequestHeader? HttpRequestHeader => _httpRequestHeader ??= SetHttpRequestHeader(Name);

    private static HttpRequestHeader? SetHttpRequestHeader(string headerName)
    {
        if (Enum.TryParse<HttpRequestHeader>(headerName, out var httpRequestHeader))
        {
            return httpRequestHeader;
        }

        return null;
    }
}