using System;

namespace Lab_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(args[1]);
            MyOutputReader myOutput = new MyOutputReader(args[1]);
            myOutput.ReadFile(args[0]);
            myOutput.ReadFile(args[0]);
            myOutput.GenerateStatsFile();
            myOutput.GenerateOutputFile(); 
        }
    }
}
