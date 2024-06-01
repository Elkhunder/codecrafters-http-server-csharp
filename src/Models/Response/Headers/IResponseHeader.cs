using System.Net;

namespace codecrafters_http_server.Helpers;

public interface IResponseHeader
{
    public HttpResponseHeader Name { get; }
}