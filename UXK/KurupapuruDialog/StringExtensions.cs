using System;
using System.Linq;

namespace UXK.KurupapuruDialog
{
    public static class StringExtensions
    {
        public static string RemoveWhitespace(this string input)
        {
            return new string(input.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }
    }
}