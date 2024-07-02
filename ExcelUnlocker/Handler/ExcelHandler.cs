using OfficeOpenXml;

public class ExcelHandler
{
    public string GetExcelFilePath()
    {
        var excelDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Excel");

        if (!Directory.Exists(excelDirectory))
        {
            Directory.CreateDirectory(excelDirectory);
            return null;
        }

        var excelFiles = Directory.GetFiles(excelDirectory, "*.xlsx");

        if (excelFiles.Length == 0)
        {
            return null;
        }

        Console.WriteLine("Sélectionnez le fichier Excel à utiliser:");
        for (int i = 0; i < excelFiles.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {Path.GetFileName(excelFiles[i])}");
        }

        int selectedIndex = -1;
        while (selectedIndex < 1 || selectedIndex > excelFiles.Length)
        {
            Console.Write("Entrez le numéro du fichier: ");
            int.TryParse(Console.ReadLine(), out selectedIndex);
        }

        return excelFiles[selectedIndex - 1];
    }

    public bool TryOpenExcel(string filePath, string password)
    {
        try
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(new FileInfo(filePath), password))
            {
                SavePassword(password);
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    public void SavePassword(string password)
    {
        string passwordFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "found_passwords.txt");
        File.AppendAllText(passwordFilePath, password + Environment.NewLine);
    }
}