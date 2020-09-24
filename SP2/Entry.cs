using System;
using System.IO;
using SP2.Emitters;
using Sprache;

namespace SP2
{
    static class Entry
    {
        static void Main(string[] args)
        {
            //if(args.Length < 1) throw new ArgumentException("Not enough arguments");
            //if (args.Length > 1) throw new ArgumentException("Too many arguments");

            //var path = args[0];
            const string path = "1-9-CSharp-IO-81-Ivanov.txt";
            const string path2 = "1-9-CSharp-IO-81-Ivanov.c";
            var pathSave = Path.ChangeExtension(path, "asm");

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