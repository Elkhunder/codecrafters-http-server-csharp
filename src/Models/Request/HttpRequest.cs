using System.Net;

namespace codecrafters_http_server.Helpers;

public record HttpRequest(RequestLine RequestLine, List<RequestHeader> Headers, Route Route, string? Body, RequestFile? RequestFile )
{
    public RequestLine? RequestLine { get; set; } = RequestLine;
    public List<RequestHeader> Headers { get; set; } = Headers;
    public Route Route { get; set; } = Route;
    public string? Body { get; set; } = Body;
    public RequestFile? RequestFile { get; set; } = RequestFile;
    
}