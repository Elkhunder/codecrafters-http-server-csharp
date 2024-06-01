namespace codecrafters_http_server.Helpers;

public class FileHandler(string directory, HttpRequest httpRequest)
{
    private readonly string _directory = directory;
    private HttpRequest _httpRequest = httpRequest;
    
    public string? GetFileContents()
    {
        if (_directory is null || _httpRequest.Route is null) throw new NullReferenceException();
        var path = Path.Combine(_directory, _httpRequest.Route.Parameter);
        return !ValidateFile(path) ? null : File.ReadAllText(path);
    }

    public bool SaveFileContents(string fileContents, string directory, string fileName)
    {
        var path = Path.Combine(directory, fileName);
        try
        {
            File.WriteAllText(path, fileContents);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    private bool ValidateFile(string path)
    {
        return Path.Exists(path);
    }
}