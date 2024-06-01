using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Text;

namespace codecrafters_http_server.Helpers;

public record HttpResponseOld(string? Status = null, ContentType? ContentType = null, int ContentLength = 0, string? Body = null)
{
    
    public HttpResponseOld(string status, ContentType contentType, string body) 
        : this(status, contentType, body.Length, body)
    {
    }

    public HttpResponseOld(string status)
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