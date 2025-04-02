public interface ISaveLoadService<T>
{
    void Save(T data, string identifier);
    T Load(string identifier);
}

public class FileSystemSaveLoadService : ISaveLoadService<string>
{
    private readonly string _basePath;

    public FileSystemSaveLoadService(string basePath)
    {
        _basePath = basePath;

        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }

    public void Save(string data, string identifier)
    {
        string filePath = Path.Combine(_basePath, $"{identifier}.txt");
        File.WriteAllText(filePath, data);
    }

    public string Load(string identifier)
    {
        string filePath = Path.Combine(_basePath, $"{identifier}.txt");

        if (!File.Exists(filePath))
        {
            return null;
        }

        return File.ReadAllText(filePath);
    }
}
