namespace codecrafters_http_server.Helpers;

public record RequestLine(string Method, string RequestUri, string ProtocolVersion)
{
    private readonly HttpMethod _method = new HttpMethod(Method);
    public string Method { get; set; } = Method;

    public string RequestUri { get; init; } = RequestUri;
    public string ProtocolVersion { get; init; } = ProtocolVersion;

    public static RequestLine Empty()
    {
        return new RequestLine(string.Empty, string.Empty, string.Empty);
    }

    public HttpMethod GetHttpMethod()
    {
        return _method;
    }
};