using System.Net;

namespace codecrafters_http_server.Helpers;

public record RequestHeader(string Name, List<string> Values) : IRequestHeader
{
    public RequestHeader(string Name, string value) : this(Name, value.Split().ToList())
    {
        
    }
    private HttpRequestHeader? _httpRequestHeader;

    public HttpRequestHeader? HttpRequestHeader => _httpRequestHeader ??= SetHttpRequestHeader(Name);

    private static HttpRequestHeader? SetHttpRequestHeader(string headerName)
    {
        if (Enum.TryParse<HttpRequestHeader>(headerName.Replace(",", ""), out var httpRequestHeader))
        {
            return httpRequestHeader;
        }

        return null;
    }
}