using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UXK.KurupapuruDialog
{
    public static class KDFIleReader
    {
        public enum LineType
        {
            Normal,
            NodeStart,
            OptionStart,
            Method
        }
        
        public struct Line
        {
            public readonly uint Tabulation;
            public readonly string LineString;

            public Line(string lineString, uint tabulation = 0)
            {
                this.LineString = lineString;
                this.Tabulation = tabulation;
            }
        }


        #region REGEX CONSTANTS
        // Group 1: Actor Ids
        // Group 2: Actor Name
        private const string PATTERN_ACTOR_DEFINE = @"## *Actor: *(.*) *= *(.*)";
        
        // Group 1: Node Id
        private const string PATTERN_NODE_START = @"# *(([a-z]|[A-Z]|[0-9]|\.)+)";

        // Group 1: Actor Id
        // Group 2: Phrase
        private const string PATTERN_ACTOR_PHRASE = @"([^\n ]+) *: *(.+)";
        
        // Group 1: Actor Id
        private const string PATTERN_ACTOR_SET = @"([^\n ]+) *:";

        private const string PATTERN_DECISION_OPTION = @"-*> *(.+)";
        
        private const string PATTERN_IF_START     = @"if *\((.*)\)";
        private const string PATTERN_OPTION_START = @"# *.*";
        #endregion
        
        private static List<Line> _lines = new List<Line>();
        private static KDAsset result;
        
        
        public static KDAsset Read(StreamReader r)
        {
            result = ScriptableObject.CreateInstance<KDAsset>();

            while (!r.EndOfStream)
                _lines.Add(new Line(r.ReadLine()));

            ReadLines(_lines);

            return result;
        }

        private static void ReadLines(List<Line> lines)
        {
            DialogNode currentNode = null;
            string lastActorId = "";
            int neededTabulation = 0;
            
            for (int i = 0; i < lines.Count; i++)
            {
                var lineStr = lines[i].LineString;
                Match match = null;
                
                if (String.IsNullOrEmpty(lineStr))
                    continue;
                
                
                if (TryGetMatch(lineStr, PATTERN_ACTOR_DEFINE, out match))
                {
                    result.Actors.Add(new Actor(match.Groups[1].Value.Split(',' , ' '), match.Groups[2].Value));
                    continue;
                }
                
                if (TryGetMatch(lineStr, PATTERN_NODE_START, out match))
                {
                    currentNode = new DialogNode(match.Groups[1].Value);
                    result.Nodes.Add(currentNode);
                    continue;
                }

                
                lineStr = RemoveUnnecessary(lineStr, GetTabulation(lineStr));
                

                if (String.IsNullOrEmpty(lineStr))
                    continue;

                if (TryGetMatch(lineStr, PATTERN_DECISION_OPTION, out match))
                {
                    var decision = ReadDecision(currentNode.NodeName, match.Groups[1].Value, lines, i, out i);
                    currentNode.Content.Add(decision);
                    continue;
                }
                
                if (TryGetMatch(lineStr, PATTERN_ACTOR_PHRASE, out match))
                {
                    lastActorId = match.Groups[1].Value;
                    var phrase = new DialogLine(lastActorId, match.Groups[2].Value);
                    currentNode.Content.Add(phrase);
                    continue;
                }

                if (TryGetMatch(lineStr, PATTERN_ACTOR_SET, out match))
                {
                    lastActorId = match.Groups[1].Value;
                    continue;
                }
                else
                {
                    // Add phrase
                    var phrase = new DialogLine(lastActorId, lineStr);
                    currentNode.Content.Add(phrase);
                }
            }
        }

        private static DialogDecision ReadDecision(string currentNodeName, string firstOptionName, IReadOnlyList<Line> lines, int currentLineIndex, out int lastUsedLine)
        {
            var decision = new DialogDecision();


            var writeToOption = new DialogDecisionOption(firstOptionName, currentNodeName);
            decision.Options.Add(writeToOption);
            while (writeToOption != null)
            {
                ReadDecisionOption();
                if (currentLineIndex < lines.Count-1 && TryGetMatch(lines[currentLineIndex + 1].LineString, PATTERN_DECISION_OPTION, out var match))
                {
                    currentLineIndex++;
                    writeToOption = new DialogDecisionOption(match.Groups[1].Value, currentNodeName);
                    decision.Options.Add(writeToOption);
                }
                else
                {
                    writeToOption = null;
                }
            }
            
            // returns: true = read another option, false = return 
            void ReadDecisionOption()
            {
                int targetTabulation = GetTabulation(lines[currentLineIndex + 1].LineString);
                string               lastActorId = String.Empty;
                
                for (currentLineIndex = currentLineIndex + 1; currentLineIndex < lines.Count; currentLineIndex++)
                {
                    var lineStr = lines[currentLineIndex].LineString;
                
                    var tabulation = GetTabulation(lineStr);
                
                    if (tabulation < targetTabulation)
                    {
                        currentLineIndex--;
                        return;
                    }

                    lineStr = RemoveUnnecessary(lineStr, tabulation);
                
                    Match match;
                
                    if (String.IsNullOrEmpty(lineStr))
                        continue;

                    if (TryGetMatch(lineStr, PATTERN_DECISION_OPTION, out match))
                    {
                        ReadDecision(writeToOption.NodeName, match.Groups[1].Value, lines, currentLineIndex, out currentLineIndex);
                        continue;
                    }
                
                    if (TryGetMatch(lineStr, PATTERN_ACTOR_PHRASE, out match))
                    {
                        lastActorId = match.Groups[1].Value;
                        var phrase = new DialogLine(lastActorId, match.Groups[2].Value);
                        writeToOption.Content.Add(phrase);
                        continue;
                    }

                    if (TryGetMatch(lineStr, PATTERN_ACTOR_SET, out match))
                    {
                        lastActorId = match.Groups[1].Value;
                        continue;
                    }
                    else
                    {
                        // Add phrase
                        var phrase = new DialogLine(lastActorId, lineStr);
                        writeToOption.Content.Add(phrase);
                    }
                }

                return;
            }

            lastUsedLine = currentLineIndex;
            return decision;

            // void ReadDecisionContent(DialogDecision addTo)
            // {
            //     i++;
            //     while (i < lines.Count)
            //     {
            //         var line = lines[i].LineString;
            //         if ()
            //     }
            // }
        }

        private static string RemoveUnnecessary(string line, int tabulation)
        {
            var commentStartIndex = line.IndexOf(@"//");

            var lineStart = tabulation;
            var lineEnd = commentStartIndex == -1
                ? line.Length
                : commentStartIndex == 0
                    ? 0
                    : commentStartIndex - 1;

            return line.Substring(lineStart, lineEnd - lineStart);
        }
        
        private static int GetTabulation(string line)
        {
            int lineTabulation = 0;
            while (lineTabulation < line.Length)
            {
                if (line[lineTabulation] == ' ')
                    lineTabulation++;
                else
                    break;
            }

            return lineTabulation;
        }

        private static bool TryGetMatch(string input, string pattern, out Match result)
        {
            result = Regex.Match(input, pattern);
            return result.Success;
        }
    }
}