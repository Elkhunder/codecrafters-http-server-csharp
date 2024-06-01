namespace codecrafters_http_server.Helpers;

public record ContentLengthHeader() : BaseHeader
{
    public int Value;
    public ContentLengthHeader(int headerId, int value) : this()
    {
        Name = ResponseHeaderDictionary.GetValueOrDefault(headerId);
        Value = value;
    }
};