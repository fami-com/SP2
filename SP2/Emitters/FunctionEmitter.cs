using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SP2.Definitions;
using SP2.Emitters.Statements;
using SP2.Tokens;
using Type = SP2.Tokens.Type;

namespace SP2.Emitters
{
    internal class FunctionEmitter : Emitter
    {
        private static readonly Regex _rx = new Regex(@"#@([_A-Za-z][_A-Za-z0-9]*)");
        
        private readonly Function function;

        public readonly List<string> Prototypes;
        public readonly Dictionary<string, (int offset, Type t)> SymbolTable;

        public FunctionEmitter(Function func)
        {
            function = func;
            Prototypes = new List<string> {$"{function.Definition.Name} PROTO"};
            code = new List<string> {$"{function.Definition.Name} PROC"};
            SymbolTable = new Dictionary<string, (int, Type)>();
        }

        public override void Emit()
        {
            var t = new BlockEmitter(function.Body);
            t.Emit();

            var offset = 0;
            foreach (var (sym,type) in t.Symbols)
            {
                offset += 4;
                SymbolTable.Add(sym,(-offset,type));
            }

            code.Add("push ebp");
            code.Add("mov ebp, esp");
            code.Add($"sub ebp, {offset}");

            foreach (var s in t.Code)
            {
                var m = _rx.Match(s).ToString();
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
                var y=_rx.Replace(s, $"{tp.Ptr} ptr[ebp{o}]");
                code.Add(y);
            }

            code.Add($"add ebp, {offset}");
            code.Add("mov esp, ebp");
            code.Add("pop ebp");
            code.Add($"ret");
            code.Add($"{function.Definition.Name} ENDP");
        }
    }
}