using System;
using System.IO;
using SP2.Emitters;
using Sprache;

namespace SP2
{
    internal static class Entry
    {
        private static void Main()
        {
            const string path = "3-9-CSharp-IO-81-Ivanov.txt";
            const string pathSave = "3-9-CSharp-IO-81-Ivanov.asm";

            string input = "";

            try
            {
                input = File.ReadAllText(path);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Input file not found.");
                Console.ReadKey();
                Environment.Exit(2);
            }

            try
            {
                var parsed = Grammar.Program.Parse(input);
                Console.WriteLine(parsed);
                var emitter = new ProgramEmitter(parsed);
                emitter.Emit();
                Console.WriteLine(emitter.Assembly);
                File.WriteAllText(pathSave, emitter.Assembly);
            }
            catch (ParseException e)
            {
                Console.Error.WriteLine($"Error: {e.Message} at {e.Position}");
                Console.ReadKey();
                Environment.Exit(1);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error: {e.Message}");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }
    }
}