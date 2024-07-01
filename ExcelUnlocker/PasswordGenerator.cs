public class PasswordGenerator
{
    private readonly Configuration _config;
    private readonly List<string> _charSet;

    public PasswordGenerator(Configuration config)
    {
        _config = config;
        _charSet = BuildCharSet();
    }

    private List<string> BuildCharSet()
    {
        var charSet = new List<string>();

        if (_config.IncludeNumbers)
            charSet.AddRange("0123456789".ToCharArray().Select(c => c.ToString()));
        if (_config.IncludeLetters)
        {
            if (_config.IncludeUpperCase)
                charSet.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Select(c => c.ToString()));
            if (_config.IncludeLowerCase)
                charSet.AddRange("abcdefghijklmnopqrstuvwxyz".ToCharArray().Select(c => c.ToString()));
        }
        if (_config.IncludeSpecialChars)
            charSet.AddRange("!@#$%^&*()".ToCharArray().Select(c => c.ToString()));

        return charSet;
    }

    public IEnumerable<string> GeneratePasswords(int length)
    {
        var combinations = new List<string> { "" };
        for (int i = 0; i < length; i++)
        {
            var newCombinations = new List<string>();
            foreach (var combo in combinations)
            {
                foreach (var c in _charSet)
                {
                    newCombinations.Add(combo + c);
                }
            }
            combinations = newCombinations;
        }
        return combinations;
    }
}