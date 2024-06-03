namespace codecrafters_http_server.Handlers;

public class FileHandler(string directory, string fileName)
{
    private readonly string _path = Path.Combine(directory, fileName);
    public bool FileValidated = false;
    
    public string GetFileContents()
    {
        FileValidated = ValidateFile();
        if (!FileValidated) return string.Empty;
        var file = File.ReadAllText(_path);
        Console.WriteLine($"{nameof(FileHandler)}\r\n Path: {_path}, {nameof(ValidateFile)}: {ValidateFile()}, {nameof(file)}: {file}, {nameof(file.Length)}: {file.Length}");
        return file;
    }

    public bool SaveFileContents(string fileContents)
    {
        try
        {
            File.WriteAllText(_path, fileContents);
            FileValidated = ValidateFile();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return ValidateFile();
    }
    public bool ValidateFile()
    {
        return Path.Exists(_path);
    }
}