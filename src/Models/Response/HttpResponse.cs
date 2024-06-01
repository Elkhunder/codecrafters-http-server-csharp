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
        
        var sb = new StringBuilder();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (ResponseEntity?.Body == null)
            {
                sb.AppendLine($"{Status}");
                sb.AppendLine("");
                return sb.ToString();
            }
            sb.AppendLine(StatusLine);
            sb.AppendLine(ResponseEntity.Headers.FirstOrDefault(header => header.Name == ContentType);
            sb.AppendLine($"Content-Length: {ContentLength}");
            sb.AppendLine("");
            sb.Append(Body);
            return sb.ToString();
        }
        else
        {
            if (Body == null)
            {
                sb.AppendLine($"{Status}\r");
                sb.AppendLine("\r");
                return sb.ToString();
            }
            sb.AppendLine($"{Status}\r");
            sb.AppendLine($"Content-Type: {ContentType}\r");
            sb.AppendLine($"Content-Length: {ContentLength}\r");
            sb.AppendLine("\r");
            sb.Append(Body);
            return sb.ToString();
        }
    }
};