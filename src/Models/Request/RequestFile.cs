namespace codecrafters_http_server.Helpers;

public record RequestFile(string Directory, string FileName, string Contents, bool FileValidated)
{
    public string Directory { get; set; } = Directory;
    public string FileName { get; set; } = FileName;
    public bool FileValidated { get; set; } = FileValidated;
    public string Contents { get; init; } = Contents;

    public static RequestFile Empty => new RequestFile(string.Empty, string.Empty, string.Empty, false);
};