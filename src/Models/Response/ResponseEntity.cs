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
                    sb.AppendLine($"{intValue}: {intResponseHeader.Value}");
                    break;
                case ResponseHeader<string> stringResponseHeader when
                    HeaderDictionary.TryGetValue(header.Name, out var stringValue):
                    sb.AppendLine($"{stringValue}: {stringResponseHeader.Value}");
                    break;
            }
        }

        if (Body is null) return sb.ToString();
        sb.AppendLine("");
        sb.AppendLine(Body);
        return sb.ToString();
    }

    private record HeaderType
    {
        public int? IntValue { get; init; }
        public string? StringValue { get; init; }
    }

    private static readonly Dictionary<HttpResponseHeader, string> HeaderDictionary =
        new Dictionary<HttpResponseHeader, string>()
        {
            { HttpResponseHeader.ContentType, "Content-Type" },
            { HttpResponseHeader.ContentLength, "Content-Length" }
        };
}