using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SP2.Definitions;
using SP2.Emitters;
using Sprache;
using Type = SP2.Tokens.Type;

namespace SP2
{
    internal static class Entry
    {
        private static void Main()
        {
            const string name = "РГР-9-CSharp-IO-81-Ivanov";
            var path = $"{name}.txt";
            var pathSave = $"{name}.asm";
#if DEBUG
            path = $@"D:\RiderProjects\SP2\SP2\SP2\bin\Release\netcoreapp3.1\{path}";
            pathSave = $@"D:\RiderProjects\SP2\SP2\SP2\bin\Release\netcoreapp3.1\{pathSave}";
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
                
            try
            {
                var lexer = new Lexer(input);
                lexer.Tokenize();
                foreach (var t in lexer.Tokens)
                {
                    Console.WriteLine(t);
                }
                
                var grammar = new Grammar();
                var parsed = grammar.Program.Parse(input);
                Console.WriteLine(parsed);

                var astw = new AstWriter(parsed);
                astw.WriteAst();
                var emitter = new ProgramEmitter(parsed);
                emitter.Emit();
                var asm = emitter.Assembly;
                Console.WriteLine(asm);
                File.WriteAllText(pathSave, asm);
            }
            catch (ParseException e)
            {
                Console.WriteLine($"{e.Message}, at row {e.Position.Line}, column {e.Position.Column}");
                throw;
            }
        }
    }
}