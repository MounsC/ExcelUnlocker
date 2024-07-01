using System.Text.Json;

public class Configuration
{
    public bool IncludeNumbers { get; set; }
    public bool IncludeLetters { get; set; }
    public bool IncludeUpperCase { get; set; }
    public bool IncludeLowerCase { get; set; }
    public bool IncludeSpecialChars { get; set; }
    public int MinPasswordLength { get; set; }
    public int MaxPasswordLength { get; set; }

    public static Configuration LoadConfig(string filePath)
    {
        if (!File.Exists(filePath))
        {
            CreateDefaultConfig(filePath);
        }

        var configJson = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Configuration>(configJson);
    }

    private static void CreateDefaultConfig(string filePath)
    {
        var defaultConfig = new Configuration
        {
            IncludeNumbers = true,
            IncludeLetters = true,
            IncludeUpperCase = true,
            IncludeLowerCase = true,
            IncludeSpecialChars = false,
            MinPasswordLength = 4,
            MaxPasswordLength = 6
        };

        var configJson = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, configJson);

        Console.WriteLine("Fichier de configuration par défaut créé.");
    }
}