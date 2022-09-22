using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_1
{
    interface IStatsGenerator
    {
        int GetLineCount { get;}

        int GetCharCount { get; }

        List<char> GetEndLine { get; }

        List<string> GetFirstWord { get;  }

        int GetWordCount { get; }

        List<char> GetFirstLetter { get; }
    }
}
