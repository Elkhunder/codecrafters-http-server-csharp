using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace codecrafters_http_server.Helpers;

public record BaseHeader
{
    protected static readonly Dictionary<int, string> ResponseHeaderDictionary = new Dictionary<int, string>()
    {
        { 20, "Accept-Ranges" },
        { 21, "Age" },
        { 10, "Allow" },
        { 0, "Cache-Control" },
        { 1, "Connection" },
        { 13, "Content-Encoding" },
        { 14, "Content-Language" },
        { 11, "Content-Length" },
        { 15, "Content-Location" },
        { 16, "Content-MD5" },
        { 17, "Content-Range" },
        { 12, "Content-Type" },
        { 2, "Date" },
        { 22, "ETag" },
        { 18, "Expires" },
        { 3, "Keep-Alive" },
        { 19, "Last-Modified" },
        { 23, "Location" },
        { 4, "Pragma" },
        { 24, "Proxy-Authenticate" },
        { 25, "Retry-After" },
        { 26, "Server" },
        { 27, "Set-Cookie" },
        { 5, "Trailer" },
        { 6, "Transfer-Encoding" },
        { 7, "Upgrade" },
        { 28, "Vary" },
        { 8, "Via" },
        { 9, "Warning" },
        { 29, "WWW-Authenticate" }
    };
    public string? Name { get; set; }
    public  Value { get; set; }
    
};