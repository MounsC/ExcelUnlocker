using System.Collections.Concurrent;

class Program
{
    private static readonly object fileLock = new object();
    private static BlockingCollection<string> testedPasswordsQueue = new BlockingCollection<string>();
    private static ConcurrentDictionary<string, bool> testedPasswords = new ConcurrentDictionary<string, bool>();

    static void Main(string[] args)
    {
        string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        var config = Configuration.LoadConfig(configFilePath);

        var excelHandler = new ExcelHandler();
        var excelFilePath = excelHandler.GetExcelFilePath();

        if (string.IsNullOrEmpty(excelFilePath))
        {
            Console.WriteLine("Aucun fichier Excel trouvé dans le dossier 'Excel'.");
            return;
        }

        var passwordGenerator = new PasswordGenerator(config);
        LoadTestedPasswords();
        int attemptCount = testedPasswords.Count;

        Console.WriteLine("Tentatives de mot de passe:");
        Console.WriteLine(attemptCount);

        var passwordFound = new ConcurrentBag<string>();

        Parallel.For(config.MinPasswordLength, config.MaxPasswordLength + 1, length =>
        {
            foreach (var password in passwordGenerator.GeneratePasswords(length))
            {
                if (testedPasswords.ContainsKey(password) || passwordFound.Count > 0)
                    continue;

                attemptCount++;
                Console.SetCursorPosition(0, 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, 1);
                Console.Write(attemptCount);

                if (excelHandler.TryOpenExcel(excelFilePath, password))
                {
                    Console.WriteLine($"\nMot de passe trouvé: {password}");
                    passwordFound.Add(password);
                    break;
                }

                testedPasswords.TryAdd(password, true);
                testedPasswordsQueue.Add(password);

                if (attemptCount % 1000 == 0)
                {
                    SaveTestedPasswords();
                }

                Thread.Sleep(10);
            }
        });

        if (passwordFound.Count == 0)
        {
            SaveTestedPasswords();
            Console.WriteLine($"\nMot de passe non trouvé après {attemptCount} tentatives.");
        }
        else
        {
            excelHandler.SavePassword(passwordFound.First());
        }
    }

    static void LoadTestedPasswords()
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tested_passwords.txt");

        if (!File.Exists(filePath))
        {
            return;
        }

        var passwords = File.ReadAllLines(filePath);
        foreach (var password in passwords)
        {
            testedPasswords.TryAdd(password, true);
        }
    }

    static void SaveTestedPasswords()
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tested_passwords.txt");

        lock (fileLock)
        {
            try
            {
                File.AppendAllLines(filePath, testedPasswordsQueue);
                testedPasswordsQueue = new BlockingCollection<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la sauvegarde des mots de passe testés: {ex.Message}");
            }
        }
    }
}
