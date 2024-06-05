using System.Net;
using System.Net.Http.Headers;
using codecrafters_http_server.Dictionaries;

namespace codecrafters_http_server.Helpers;

public record ResponseHeader<T>(HttpResponseHeader HttpResponseHeader = default!, T Value = default!) : IResponseHeader
{
    private string? _headerName;

    public string? HeaderName => _headerName = HttpResponseHeader.ToHttpHeader(); 

    public static ResponseHeader<T> Empty => new ResponseHeader<T>();
    
    // private static HttpResponseHeader? SetHttpResponseHeader(string headerName)
    // {
    //     if (Enum.TryParse<HttpResponseHeader>(headerName, out var httpResponseHeader))
    //     {
    //         return httpResponseHeader;
    //     }
    //
    //     return null;
    // }
}