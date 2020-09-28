using System;
using System.IO;
using SP2.Emitters;
using Sprache;

namespace SP2
{
    internal static class Entry
    {
        private static void Main(string[] args)
        {
            //if(args.Length < 1) throw new ArgumentException("Not enough arguments");
            //if (args.Length > 1) throw new ArgumentException("Too many arguments");

            //var path = args[0];
            const string path = "2-9-CSharp-IO-81-Ivanov.txt";
            const string path2 = "2-9-CSharp-IO-81-Ivanov.c";
            const string pathSave = "2-9-CSharp-IO-81-Ivanov.asm";

            string input = null;

            try
            {
                input = File.ReadAllText(path);
            }
            catch (FileNotFoundException)
            {
                try
                {
                    input = File.ReadAllText(path2);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Input file not found.");
                    Console.ReadKey();
                    Environment.Exit(2);
                }
            }
            
            try
            {
                var parsed = Grammar.Program.Parse(input);
                Console.WriteLine(parsed);
                var emitter = new ProgramEmitter(parsed);
                File.WriteAllText(pathSave, emitter.AssemblyI);
            }
            catch (ParseException e)
            {
                Console.Error.WriteLine(e.Message);
                Console.ReadKey();
                Environment.Exit(1);
            }
        }
    }
}