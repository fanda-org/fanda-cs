using System.Text.RegularExpressions;

namespace Fanda.Shared
{
    public static class RegEx
    {
        public static bool IsEmail(string emailString) => Regex.IsMatch(emailString,
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase);

    }
}
