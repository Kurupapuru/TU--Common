using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UXK.KurupapuruDialog
{
    public static class KDFIleReader
    {
        public static KDAsset Read(StreamReader r)
        {
            KDAsset result = ScriptableObject.CreateInstance<KDAsset>();
            
            while (!r.EndOfStream)
            {
                ReadMain(r);
            }

            return result;
        }

        private static void ReadMain(StreamReader reader)
        {
            var line = reader.ReadLine();
            
            RemoveCommentFromLine(ref line);
                
            int lineTabulation = 0;
            while (lineTabulation < line.Length)
            {
                if (line[lineTabulation] == ' ')
                    lineTabulation++;
                else
                    break;
            }
                
            string lineClear = lineTabulation == 0 ? line : line.Substring(lineTabulation, line.Length - lineTabulation);

            if (lineClear == String.Empty)
                return;

            if (IsActorLine(lineClear))
            {
                ReadActor(lineClear);
                return;
            }

            if (IsNodeStartLine(lineClear))
            {
                ReadNode(reader);
                return;
            }
        }
        
        private static void RemoveCommentFromLine(ref string line)
        {
            var commentStartIndex = line.IndexOf(@"//");
            if (commentStartIndex != -1)
                line = line.Substring(commentStartIndex, line.Length - commentStartIndex);
        }

        private static bool IsNodeStartLine(string line)
        {
            return Regex.IsMatch(line, "# *.*");
        }

        private static bool IsActorLine(string line)
        {
            return Regex.IsMatch(line, "## *Actor: *.+=.+");
        }
        
        private static Actor ReadActor(string lineClear)
        {
            var namesStartIndex = lineClear.IndexOf(':') + 1;
            var namesEndIndex = lineClear.LastIndexOf('=') - 1;
            var names = lineClear.Substring(namesStartIndex, namesEndIndex).Split(' ', ',');

            var translatedNameStartIndex = namesEndIndex + 2;
            var translatedName = lineClear
                .Substring(translatedNameStartIndex, lineClear.Length - translatedNameStartIndex)
                .Trim(' ');
            
            return new Actor(names, translatedName);
        }

        private static DialogNode ReadNode(StreamReader r)
        {
            var result = new DialogNode();
            
            while (true)
            {
                if (r.EndOfStream)
                    break;
                
                var line = r.ReadLine();

                if (IsIfLine(line))
                    ReadIfLine(line, r);

            }

            return result;
        }

        private static bool IsIfLine(string line)
        {
            return Regex.IsMatch(line, "if *(.*)");
        }

        private static DialogCondition ReadIfLine(string line, StreamReader r)
        {
            var conditionMethodStr = line.Substring(
                line.IndexOf('('),
                line.LastIndexOf(')'));
            
        }
    }
}