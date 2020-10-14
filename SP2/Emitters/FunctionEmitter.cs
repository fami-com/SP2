using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SP2.Emitters.Statements;
using SP2.Tokens;
using Type = SP2.Tokens.Type;

namespace SP2.Emitters
{
    internal class FunctionEmitter : Emitter
    {
        private static readonly Regex Rx = new Regex(@"#@([_A-Za-z][_A-Za-z0-9]*)");
        
        private readonly Function function;

        public readonly List<string> Prototypes;
        public readonly Dictionary<string, (int offset, Type t)> SymbolTable;

        public FunctionEmitter(Function func)
        {
            function = func;
            Prototypes = new List<string> {$"{function.Definition.Name} PROTO"};
            code = new List<string> {$"{function.Definition.Name} PROC"};
            SymbolTable = function.SymbolTable;
        }

        public override void Emit()
        {
            var t = new BlockEmitter(function.Body);
            t.Emit();

            code.Add("push ebp");
            code.Add("mov ebp, esp");
            code.Add($"sub ebp, {-function.Offset}");

            Console.WriteLine(string.Join(", ", SymbolTable.Keys));
            Console.WriteLine(string.Join(", ", SymbolTable.Values));
            
            foreach (var s in t.Code)
            {
                var m = Rx.Match(s).ToString();
                if (!string.IsNullOrEmpty(m))
                {
                    m = m.Remove(0, 2);
                }

                if(!SymbolTable.ContainsKey(m))
                {
                    code.Add(s);
                    continue;
                }
                
                var (o, tp) = SymbolTable[m];
                var y= Rx.Replace(s, $"{tp.Ptr} ptr[ebp{o}]");

                if (y.Contains("mov eax, byte")) y = y.Replace(" eax, byte", "sx eax, byte");
                else if (y.Contains("mov eax, word")) y = y.Replace(" eax, word", "sx eax, word");
                else if (y.Contains("byte") && y.Contains("eax")) y=y.Replace("eax", "al");
                else if (new Regex(@"[^dqtozy]word").Match(y).Success && y.Contains("eax")) y = y.Replace("eax", "ax");

                code.Add(y);
            }

            code.Add($"add ebp, {-function.Offset}");
            code.Add("mov esp, ebp");
            code.Add("pop ebp");
            code.Add("ret");
            code.Add($"{function.Definition.Name} ENDP");
        }
    }
}