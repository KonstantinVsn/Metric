using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Metrics
{
    public class Analyzer
    {
        private string[] filesPaths;
        private int physicLineCount = 0;
        private int emptyLineCount = 0;
        private int commentedLineCount = 0;
        private int logicLineCount = 0;
        public Analyzer(string[] filesArray)
        {
            filesPaths = filesArray;
            CountLines();
        }

        private int CountLines()
        {
            foreach(var path in filesPaths)
            {
                physicLineCount += File.ReadLines(path).Count();
                emptyLineCount += GetEmptyLinesCount(path);
                commentedLineCount += GetCommentedLinesCount(path);
                //var wordsArr = GetWords(text);
            }
            Console.ReadKey();
            return 0;
        }

        static string[] GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\n");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value);

            var w = words.ToArray();
            return words.ToArray();
        }

        static string TrimSuffix(string word)
        {
            int apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return word;
        }

        private int GetEmptyLinesCount(string path)
        {
            return File.ReadLines(path).Count(line => line.Trim().Length == 0);
        }

        private int GetCommentedLinesCount(string path)
        {
            var lines = File.ReadLines(path).ToList();
            var commentedCount = 0;
            for (var i = 0; i < lines.Count(); i++)
            {
                var line = lines[i];
                if (lines[i].Contains("//"))
                {
                    commentedCount++;
                }
                if (lines[i].Contains("/*") && !lines[i].Contains("*/") && lines[i].Contains("//"))
                {
                    i++;
                    commentedCount++;
                    while (lines[i].Contains("*/"))
                    {
                        i++;
                        commentedCount++;
                    }
                }
                else if (lines[i].Contains("/*") && lines[i].Contains("*/") && lines[i].Contains("//"))
                    commentedCount++;

            }
            return commentedCount;
        }

        private int GetLogicLinesCount(string path)
        {
            var lines = File.ReadLines(path).ToList();
            

            return 0;
        }

    }
}
