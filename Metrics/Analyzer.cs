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
        private int codeLineCount = 0;
        private int emptyLineCount = 0;
        private int commentedLineCount = 0;
        private int logicLineCount = 0;
        private int phyzicalLineCount = 0;
        
        public Analyzer(string[] filesArray)
        {
            filesPaths = filesArray;
            CountLines();
        }

        private void CountLines()
        {
            foreach(var path in filesPaths)
            {
                codeLineCount += File.ReadLines(path).Count();
                emptyLineCount += GetEmptyLinesCount(path);
                commentedLineCount += GetCommentedLinesCount(path);
                logicLineCount += GetLogicLinesCount(path);
                phyzicalLineCount = codeLineCount - emptyLineCount - commentedLineCount;
                //var wordsArr = GetWords(text);
            }
            Console.WriteLine("Количество файлов - " + filesPaths.Length);
            Console.WriteLine("==========================================");
            Console.WriteLine("кода строки - " + codeLineCount);
            Console.WriteLine("физические строки - " + phyzicalLineCount);
            Console.WriteLine("логические строки - " + logicLineCount);
            Console.WriteLine("пустые строки - " + emptyLineCount);
            Console.WriteLine("с коментариями строки - " + commentedLineCount);
            Console.WriteLine("уровень коментированости - " + ((double)commentedLineCount / codeLineCount).ToString("0.00"));
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
            return File.ReadLines(path).Count(line => line.Trim().Length == 0 || line.Length ==0);
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
            var count = lines.Count(x => Resources.logicWords.Any(x.Contains));
            var countMinus = lines.Count(x => x.Contains("};"));
            var countGoto = lines.Count(x => x.Contains("goto"));
            var countClass = lines.Count(x => x.Contains("class"));
            var countStruct = lines.Count(x => x.Contains("struct"));
            var countFor = lines.Count(x => x.Contains("for"));
            return count-countMinus- countGoto- countClass- countStruct- countFor; 
        }

    }
}
