using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace itransition_introductory_exam
{
    class Program
    {
        static String input = String.Empty;
        static String output = String.Empty;

        static Dictionary<String, Int32> wordsCounter = new Dictionary<string, int>();
        static List<List<KeyValuePair<String, Int32>>> groups = new List<List<KeyValuePair<String, Int32>>>();

        static void Main(string[] args)
        {
            Init(args);

            try
            {
                String[] words = LoadContent();

                Process(words);

                SaveContent();
            }
            catch (Exception ex)
            {
                PrintException(ex);
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        static void Init(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Startup arguments is wrong format or missing. Settings default parameters...");

                input = "input.txt";
                Console.WriteLine("Input file: " + input);

                output = "output.txt";
                Console.WriteLine("Output file: " + output);
            }
            else
            {
                input = args[0];
                Console.WriteLine("Input file: " + input);

                output = args[1];
                Console.WriteLine("Output file: " + output);
            }
        }

        static String[] LoadContent()
        {
            String content = File.ReadAllText(input, Encoding.UTF8);

            string pattern = @"[^a-zA-Z-]";
            content = new Regex(pattern).Replace(content, " ");

            return content.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        }

        static void Process(String[] words)
        {
            for (Int32 i = 0; i < words.Length; i++)
            {
                string _word = words[i].ToLowerInvariant();

                if (wordsCounter.ContainsKey(_word))
                {
                    wordsCounter[_word] += 1;
                }
                else
                {
                    wordsCounter.Add(_word, 1);
                }
            }

            var wordsOrderedCounter = wordsCounter.OrderBy(x => x.Key).ToList();

            char lastChar = wordsOrderedCounter.First().Key.First();
            List<KeyValuePair<String, Int32>> lastGroup = new List<KeyValuePair<String, Int32>>();
            for (Int32 i = 0; i < wordsOrderedCounter.Count(); i++)
            {
                char letter = wordsOrderedCounter[i].Key.First();

                if (letter == lastChar)
                {
                    lastGroup.Add(wordsOrderedCounter[i]);
                }
                else
                {    
                    groups.Add(new List<KeyValuePair<String, Int32>>(lastGroup.OrderByDescending(x => x.Value)));

                    lastGroup.Clear();
                    lastChar = letter;

                    lastGroup.Add(wordsOrderedCounter[i]);
                }
            }

            groups.Add(new List<KeyValuePair<String, Int32>>(lastGroup.OrderByDescending(x => x.Value)));
        }

        static void SaveContent()
        {
            String outputContent = String.Empty;

            foreach (List<KeyValuePair<String, Int32>> group in groups)
            {
                outputContent += group.First().Key.First() + Environment.NewLine;
                foreach (KeyValuePair<String, Int32> pair in group)
                {
                    outputContent += $"{pair.Key} {pair.Value}" + Environment.NewLine;
                }
            }

            File.WriteAllText(output, outputContent, Encoding.UTF8);
        }

        static void PrintException(Exception ex)
        {
            Console.WriteLine($"Exception [{ex.GetType().Name}] occurred: {ex.Message}");
            Console.WriteLine("Waiting for key to exit...");
            Console.ReadKey(true);

            Environment.Exit(-1);
        }
    }
}
