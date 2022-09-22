using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab_1
{
    class MyReader
    {
        public void ReadFile(string inputFilePath)
        {
            IEnumerable<string> lines = File.ReadLines(inputFilePath);
            int lineNum = 0;
            foreach (var line in lines)
            {
                ProcessLine(line, lineNum);
                lineNum++;
            }
        }

        protected virtual void ProcessLine(string line, int lineNum)
        {
            line = $"{lineNum}:{line}";
            Console.WriteLine(line);
        }
    }
}
