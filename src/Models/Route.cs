namespace codecrafters_http_server.Helpers;

public record Route(Routes Path, string Parameter)
{
    public Routes Path { get; init; } = Path;
    public string Parameter { get; init; } = Parameter;
};