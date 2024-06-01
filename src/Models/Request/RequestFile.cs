namespace codecrafters_http_server.Helpers;

public record RequestFile(string Directory, string FileName, string Contents)
{
    public string Directory { get; set; } = Directory;
    public string FileName { get; set; } = FileName;
    public string Contents { get; set; } = Contents;

    public static RequestFile Empty => new RequestFile(string.Empty, string.Empty, string.Empty);
};