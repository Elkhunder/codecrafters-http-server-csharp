namespace codecrafters_http_server.Helpers;

public record ResponseHeader() : BaseHeader
{
    public string Value { get; set; } = string.Empty;
    public ResponseHeader(int headerId, string value) : this()
    {
        Name = ResponseHeaderDictionary.GetValueOrDefault(headerId);
        Value = value;
    }
};