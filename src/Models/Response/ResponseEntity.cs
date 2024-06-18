using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using static codecrafters_http_server.Dictionaries.HeaderExtensions;

namespace codecrafters_http_server.Helpers;

public record ResponseEntity(List<IResponseHeader> Headers, byte[]? Body)
{
    public static ResponseEntity Empty => new ResponseEntity([], null);

    public override string ToString()
    {
        var sb = new StringBuilder();

        foreach (var header in Headers)
        {
            switch (header)
            {
                case ResponseHeader<int> intResponseHeader:
                    sb.Append($"{intResponseHeader.HeaderName}: {intResponseHeader.Value}\r\n");
                    break;
                case ResponseHeader<string> stringResponseHeader:
                    sb.Append($"{stringResponseHeader.HeaderName}: {stringResponseHeader.Value}\r\n");
                    break;
            }
        }

        // if (string.IsNullOrEmpty(Body)) return sb.ToString();
        Console.WriteLine("######Body has content appending new line after headers and adding body");
        sb.Append("\r\n");
        return sb.ToString();
    }

    
}