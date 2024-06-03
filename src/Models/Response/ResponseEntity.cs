using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace codecrafters_http_server.Helpers;

public record ResponseEntity(List<IResponseHeader> Headers, string? Body)
{
    public static ResponseEntity Empty => new ResponseEntity([], null);

    public override string ToString()
    {
        var sb = new StringBuilder();

        foreach (var header in Headers)
        {
            switch (header)
            {
                case ResponseHeader<int> intResponseHeader when HeaderDictionary.TryGetValue(header.Name, out var intValue):
                    sb.Append($"{intValue}: {intResponseHeader.Value}\r\n");
                    break;
                case ResponseHeader<string> stringResponseHeader when
                    HeaderDictionary.TryGetValue(header.Name, out var stringValue):
                    sb.Append($"{stringValue}: {stringResponseHeader.Value}\r\n");
                    break;
            }
        }

        if (string.IsNullOrEmpty(Body)) return sb.ToString();
        Console.WriteLine("######Body has content appending new line after headers and adding body");
        sb.Append("\r\n");
        sb.Append(Body);
        return sb.ToString();
    }

    private static readonly Dictionary<HttpResponseHeader, string> HeaderDictionary =
        new()
        {
            { HttpResponseHeader.ContentType, "Content-Type" },
            { HttpResponseHeader.ContentLength, "Content-Length" }
        };
}