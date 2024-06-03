using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;

namespace codecrafters_http_server.Helpers;

public record HttpResponse(ResponseStatusLine StatusLine, ResponseEntity ResponseEntity)
{
    public ResponseStatusLine StatusLine { get; set; } = StatusLine;
    public ResponseEntity ResponseEntity { get; set; } = ResponseEntity;
    
    public override string ToString()
    {
        return $"{StatusLine}" +
               $"\r\n" +
               $"{ResponseEntity}";
    }
};