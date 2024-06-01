using System.Net;
using System.Net.Http.Headers;

namespace codecrafters_http_server.Helpers;

public record ResponseHeader<T> : IResponseHeader
{
    public HttpResponseHeader Name { get; init; }
    public T Value { get; init; } = default!;
    
    private ResponseHeader(){}

    public ResponseHeader(HttpResponseHeader name, T value)
    {
        Name = name;
        Value = value;
    }

    public static ResponseHeader<T> Empty => new ResponseHeader<T>();
}