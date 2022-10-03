using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;

namespace lab_2
{
    class Teleprompter
    {

        public static async Task RunTeleprompterAsync(string file)
        {
            TelePrompterConfig config = new TelePrompterConfig(file);
            Task displayTask = ShowTeleprompter(config);
            Task speedTask = GetInput(config);
            await Task.WhenAny(displayTask, speedTask); 
            
        }
        private static IEnumerable ReadFrom(TelePrompterConfig config)
        {
            using (StreamReader reader = File.OpenText(config.FilePath))
            {
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    string[] words = line.Split(" ");
                    int lineLength = 0; 
                    foreach (var word in words)
                    {
                        yield return word;
                        lineLength += word.Length + 1; 
                        if(lineLength > 70)
                        {
                            yield return Environment.NewLine;
                            lineLength = 0;
                        }
                    }
                    yield return Environment.NewLine;
                }
            }
        }
        private static async Task ShowTeleprompter(TelePrompterConfig config)
        {
            IEnumerable line = ReadFrom(config);
            foreach (string word in line)
            {
                Console.Write(word + " ");
                if (!string.IsNullOrWhiteSpace(word))
                {
                    await Task.Delay(config.DelayInMilliseconds);
                    if (word.EndsWith(".") && !word.Any(char.IsUpper)) // could be improved?!?
                    {
                        await Task.Delay(config.DelayInMilliseconds * 3);
                    }
                }
               
            }
            

        }

        public static async Task GetInput(TelePrompterConfig config)
        {
            void CheckKeyPress()
            {
                while (!config.Done)
                {
                    char key = Console.ReadKey(true).KeyChar;
                    switch (key)
                    {
                        case '>': config.UpdateDelay(-10); break;
                        case '<': config.UpdateDelay(10); break;
                        case '1': config.SetDelay(100); break;
                        case '2': config.SetDelay(200); break;
                        case '3': config.SetDelay(300); break;
                        case '4': config.SetDelay(400); break;
                        case '5': config.SetDelay(500); break;
                    }
                }
            }
            await Task.Run(CheckKeyPress); 
        }
    }   
}
