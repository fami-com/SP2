using System;
using System.IO;
using SP2.Definitions;
using SP2.Emitters;
using Sprache;

namespace SP2
{
    internal static class Entry
    {
        private static void Main()
        {
#if DEBUG
            const string path = @"D:\RiderProjects\SP2\SP2\SP2\bin\Release\netcoreapp3.1\5-9-CSharp-IO-81-Ivanov.txt";
            const string pathSave = @"D:\RiderProjects\SP2\SP2\SP2\bin\Release\netcoreapp3.1\5-9-CSharp-IO-81-Ivanov.asm";
#else
            const string path = "5-9-CSharp-IO-81-Ivanov.txt";
            const string pathSave = "5-9-CSharp-IO-81-Ivanov.asm";
#endif

            var input = "";

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

            var parsed = Grammar.Program.Parse(input);
            Console.WriteLine(parsed);
            var emitter = new ProgramEmitter(parsed);
            emitter.Emit();
            var asm = emitter.Assembly;
            Console.WriteLine(asm);
            File.WriteAllText(pathSave, asm);
        }
    }
}