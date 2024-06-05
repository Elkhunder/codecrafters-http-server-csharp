using System.Net;
using codecrafters_http_server.Helpers;

namespace codecrafters_http_server.Dictionaries;

public static class HeaderExtensions
{
    private static readonly Dictionary<HttpRequestHeader, string> RequestHeaderHttpHeaderMap = 
        new()
        {
            { HttpRequestHeader.AcceptEncoding, "Accept-Encoding" },
            { HttpRequestHeader.UserAgent, "User-Agent"}
        };

    private static readonly Dictionary<string, HttpRequestHeader> HttpHeaderRequestHeaderMap =
        new()
        {
            {"Accept-Encoding", HttpRequestHeader.AcceptEncoding},
            {"User-Agent", HttpRequestHeader.UserAgent}
        };
    
    private static readonly Dictionary<HttpResponseHeader, string> ResponseHeaderHttpHeaderMap =
        new()
        {
            { HttpResponseHeader.ContentType, "Content-Type" },
            { HttpResponseHeader.ContentLength, "Content-Length" },
            { HttpResponseHeader.ContentEncoding, "Content-Encoding"}
        };

    private static readonly Dictionary<string, HttpResponseHeader> HttpHeaderResponseHeaderMap =
        new()
        {
            { "Content-Type", HttpResponseHeader.ContentType },
            { "Content-Length", HttpResponseHeader.ContentLength },
            { "Content-Encoding", HttpResponseHeader.ContentEncoding }
        };

    public static string ToHttpHeader(this HttpResponseHeader header)
    {
        return ResponseHeaderHttpHeaderMap.TryGetValue(header, out var httpHeader) 
            ? httpHeader 
            : header.ToString();
    }

    public static HttpResponseHeader ToResponseHeader(this string httpHeader)
    {
        return HttpHeaderResponseHeaderMap.TryGetValue(httpHeader, out var responseHeader)
            ? responseHeader
            : throw new ArgumentException($"The header string '{httpHeader}' is not a recognized header.");
    }

    public static string ToHttpHeader(this HttpRequestHeader header)
    {
        return RequestHeaderHttpHeaderMap.TryGetValue(header, out var httpHeader) 
            ? httpHeader 
            : header.ToString();
    }

    public static HttpRequestHeader ToRequestHeader(this string httpHeader)
    {
        return HttpHeaderRequestHeaderMap.TryGetValue(httpHeader, out var requestHeader) 
            ? requestHeader 
            : throw new ArgumentException($"The header string '{httpHeader}' is not a recognized header.");
    }
}