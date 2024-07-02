using System.Collections.Concurrent;

class Program
{
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
        var testedPasswords = LoadTestedPasswords();
        int attemptCount = testedPasswords.Count;

        Console.WriteLine("Tentatives de mot de passe:");
        Console.WriteLine(attemptCount);

        var passwordFound = new ConcurrentBag<string>();

        Parallel.For(config.MinPasswordLength, config.MaxPasswordLength + 1, length =>
        {
            var passwords = passwordGenerator.GeneratePasswords(length);

            Parallel.ForEach(passwords, password =>
            {
                if (testedPasswords.Contains(password) || passwordFound.Count > 0)
                    return;

                attemptCount++;
                Console.SetCursorPosition(0, 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, 1);
                Console.Write(attemptCount);

                if (excelHandler.TryOpenExcel(excelFilePath, password))
                {
                    Console.WriteLine($"\nMot de passe trouvé: {password}");
                    passwordFound.Add(password);
                    return;
                }

                testedPasswords.Add(password);
                if (attemptCount % 1000 == 0)
                {
                    SaveTestedPasswords(testedPasswords);
                }
            });
        });

        if (passwordFound.Count == 0)
        {
            SaveTestedPasswords(testedPasswords);
            Console.WriteLine($"\nMot de passe non trouvé après {attemptCount} tentatives.");
        }
        else
        {
            excelHandler.SavePassword(passwordFound.First());
        }
    }

    static HashSet<string> LoadTestedPasswords()
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tested_passwords.txt");

        if (!File.Exists(filePath))
        {
            return new HashSet<string>();
        }

        var passwords = File.ReadAllLines(filePath);
        return new HashSet<string>(passwords);
    }

    static void SaveTestedPasswords(HashSet<string> testedPasswords)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tested_passwords.txt");
        File.WriteAllLines(filePath, testedPasswords);
    }
}