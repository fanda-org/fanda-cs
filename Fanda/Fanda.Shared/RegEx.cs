﻿using System.Text.RegularExpressions;

namespace Fanda.Shared
{
    public static class RegEx
    {
        public static bool IsEmail(string emailString) => Regex.IsMatch(emailString,
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase);

        public static string TrimExtraSpaces(this string input)
        {
            if (input == null)
                return input;
            const string reduceMultiSpace = @"[ ]{2,}";
            return Regex.Replace(input.Replace('\t', ' '), reduceMultiSpace, " ").TrimEnd(' ', '\t');
        }
    }
}
