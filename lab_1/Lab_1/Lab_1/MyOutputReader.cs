using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab_1
{
    class MyOutputReader : MyReader, IStatsGenerator
    {
        private string outputFilePath;
        private string statsFilePath; 
        private List<KeyValuePair<int, string>> outputLines = new List<KeyValuePair<int, string>>();


        public int GetLineCount { get; private set; }

        public int GetCharCount { get; private set; }

        public List<char> GetEndLine { get; private set; }

        public List<string> GetFirstWord { get; private set; }

        public int GetWordCount { get; private set; }

        public List<char> GetFirstLetter { get; private set; }

        public MyOutputReader(string filePath)
        {
            outputFilePath = Path.Combine(filePath, "outputFile.txt");
            statsFilePath = Path.Combine(filePath, "stats.txt");
            Reset();
        }


        protected override void ProcessLine(string line, int lineNum)
        {
            if (lineNum == 0) Reset(); // rest 
            string[] words = line.Split(" ", StringSplitOptions.RemoveEmptyEntries); // get words in line 
            if (words.Length == 0 || words[0][0] == '#') return;
            string firstWord = words[0];
            string lastWord = words[words.Length - 1];
            // check 
            ErrorCheck(lineNum, words);

            // stats
            GetLineCount++;
            GetFirstWord.Add(firstWord);
            GetFirstLetter.Add(firstWord.ToCharArray()[0]);
            GetEndLine.Add(lastWord.ToCharArray()[lastWord.Length - 1]);
            GetWordCount += words.Length; 
            foreach (string word in words) GetCharCount += word.Length + 1;
            outputLines.Add(new KeyValuePair<int, string>(lineNum, line)); 
        }

        public void GenerateOutputFile()
        {
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
            foreach (KeyValuePair<int, string> line in outputLines)
            {
                using (StreamWriter streamWriter = File.AppendText(outputFilePath))
                {
                    streamWriter.WriteLine($"{line.Key}:{line.Value}");
                }
            }
        }

        public void GenerateStatsFile()
        {
            if (File.Exists(statsFilePath))
            {
                File.Delete(statsFilePath);
            }
            using (StreamWriter streamWriter = File.AppendText(statsFilePath))
            {
                streamWriter.WriteLine($"~~~~ File summary ~~~~");
                streamWriter.WriteLine($"Total line count:{GetLineCount}");
                streamWriter.WriteLine($"Total word count:{GetWordCount}");
                streamWriter.WriteLine($"Total char count:{GetCharCount}");
                streamWriter.WriteLine($"Total char count:{GetFirstLetter.Count}");
                streamWriter.WriteLine($"~~~~ Line info ~~~~");
                for (int i = 0; i < GetLineCount; i++)
                {
                    streamWriter.WriteLine($"~~ Line:{i+1} ~~");
                    streamWriter.WriteLine($"First word:{GetFirstWord[i]}");
                    streamWriter.WriteLine($"First leter:{GetFirstLetter[i]}");
                    streamWriter.WriteLine($"Last leter:{GetEndLine[i]}");
                }
            }
                
        }

        private void ErrorCheck(int lineNum, string[] words)
        {
            string lastWord = words[words.Length - 1];
            Console.WriteLine(lastWord); 
            switch (lastWord)
            {
                case string s when !s.EndsWith(';') && !s.EndsWith('+'):
                    throw new Exception($"Error, line:{lineNum+1}, Syntax error: Line must end with a \"+\" or \";\"");
                default:
                    return; 
            }
            
        }

        private void Reset()
        {
            GetLineCount = 0;

            GetCharCount = 0;

            GetEndLine = new List<char>();

            GetFirstWord = new List<string>();

            GetWordCount = 0;

            GetFirstLetter = new List<char>();

            outputLines = new List<KeyValuePair<int, string>>();
        }
    }
}
