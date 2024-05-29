using System.Runtime.InteropServices;
using System.Text;

namespace codecrafters_http_server.Helpers;

public record HttpResponse(string? Status = null, string? ContentType = null, int ContentLength = 0, string? Body = null)
{
    
    public HttpResponse(string status, string contentType, string body) 
        : this(status, contentType, body.Length, body)
    {
    }

    public HttpResponse(string status)
        : this(status, null, 0, null)
    {
    }

    public override string ToString()
    {
        
        var sb = new StringBuilder();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (Body == null)
            {
                sb.AppendLine($"{Status}");
                sb.AppendLine("");
                return sb.ToString();
            }
            sb.AppendLine($"{Status}");
            sb.AppendLine($"Content-Type: {ContentType}");
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
}