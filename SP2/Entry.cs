using System;
using System.IO;
using SP2.Definitions;
using SP2.Emitters;
using Sprache;

namespace SP2
{
    static class Entry
    {
        static void Main(string[] args)
        {
            if(args.Length < 2) throw new ArgumentException("Not enough arguments");
            if (args.Length > 2) throw new ArgumentException("Too many arguments");

            var parsed = Grammar.Program.Parse(File.ReadAllText(args[1]));
            var emitter = new ProgramEmitter(parsed);
            Console.WriteLine(emitter.AssemblyI);
            
        }
    }
}