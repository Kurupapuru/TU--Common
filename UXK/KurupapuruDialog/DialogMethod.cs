using System;
using System.Text.RegularExpressions;

namespace UXK.KurupapuruDialog
{
    [Serializable]
    public class DialogMethod
    {
        public string MethodName;
        public object[] Parameters;

        public static DialogMethod GetFromString(string str)
        {
            DialogMethod result = new DialogMethod();

            str = str.RemoveWhitespace();
            
            var parametersStartIndex = str.IndexOf('(') + 1;
            var parametersEndIndex = str.LastIndexOf(')') - 1;
            result.MethodName = str.Substring(0, parametersStartIndex - 2);

            var parametersStrs = str.Substring(parametersStartIndex, parametersEndIndex - parametersStartIndex).Split(',');
            result.Parameters = new object[parametersStrs.Length];
            for (int i = 0; i < parametersStrs.Length; i++)
            {
                var prm = parametersStrs[i];
                if (prm.StartsWith("\"") && prm.EndsWith("\""))
                    result.Parameters[i] = prm.Substring(1, prm.Length - 2);
                else if (prm.EndsWith("f") && float.TryParse(prm.Substring(0, prm.Length - 1), out var @float))
                    result.Parameters[i] = @float;
                else if (int.TryParse(prm, out var @int))
                    result.Parameters[i] = @int;
                else if (IsDialogMethod(prm))
                    result.Parameters[i] = GetFromString(prm);
                else
                    throw new ArgumentOutOfRangeException($"Cant understand parameter \"{prm}\" in method {str}");
            }

            return result;
        }

        public static bool IsDialogMethod(string str)
        {
            return Regex.IsMatch(str, @".*\(.*\)");
        }
    }
}