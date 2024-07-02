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
        return GeneratePasswordsRecursive("", length);
    }

    private IEnumerable<string> GeneratePasswordsRecursive(string prefix, int length)
    {
        if (length == 0)
        {
            yield return prefix;
        }
        else
        {
            foreach (var c in _charSet)
            {
                foreach (var password in GeneratePasswordsRecursive(prefix + c, length - 1))
                {
                    yield return password;
                }
            }
        }
    }
}