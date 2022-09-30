using System;
using System.IO;

namespace lab_2
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(args[0], "*.txt");
            Console.WriteLine("-----Select file by typing the number accoeated with it-------");
            int i = 1;
            foreach (var file in files)
            {
                Console.WriteLine($"{i}:{file}");
                i += 1; 
            }
            bool validKeyPreess = false;
            string selectedFile = files[0];
            do
            {
                try
                {
                    int value = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine(value);
                    if (value < files.Length + 1 && value > 0)
                    {
                        selectedFile = files[value - 1];
                        validKeyPreess = true;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid number {value}");
                    }

                }catch(Exception e)
                {
                    Console.WriteLine("invalid input");
                }
                
            } while (!validKeyPreess);
            Teleprompter.RunTeleprompterAsync(selectedFile).Wait();
        }

    }
}
