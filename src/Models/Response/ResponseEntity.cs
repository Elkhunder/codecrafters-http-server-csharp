namespace codecrafters_http_server.Helpers;

public record ResponseEntity(List<IResponseHeader> Headers, string? Body)
{
    public static ResponseEntity Empty => new ResponseEntity([], null);
};