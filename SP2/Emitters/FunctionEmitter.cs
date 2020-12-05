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
        private static readonly Regex _rx = new Regex(@"#@([0-9]+)([_A-Za-z][_A-Za-z0-9]*)");
        
        private readonly Function _function;

        public readonly List<string> Prototypes;
        private readonly Dictionary<string, int> _symbolTable;
        private int _ret;

        public FunctionEmitter(Function func)
        {
            _function = func;
            Prototypes = new List<string> {$"{_function.Definition.Name} PROTO"};
            code = new List<string> {$"{_function.Definition.Name} PROC"};
            var argumentTable = new Dictionary<string, int>();
            _ret = 0;
            foreach (var t in func.Definition.Variables.AsEnumerable().Reverse())
            {
                argumentTable.Add(t.Identifier.Name, _ret+8);
                _ret += t.Type.Size;
            }
            
            _symbolTable = _function.Variables.Select((Value, Index) => new {Value, Index = (Index + 1) * -4})
                .ToDictionary(x => x.Value, x => x.Index);

            _symbolTable = _symbolTable.Concat(argumentTable)
                .ToDictionary(s => s.Key, s => s.Value);
            
            Console.WriteLine(_function.Definition.Name);
            Console.WriteLine(string.Join(", ", _symbolTable.Select(x => $"{x.Key} => {x.Value}")));
        }

        public override void Emit()
        {
            var t = new BlockEmitter(_function.Body);
            t.Emit();

            code.Add("push ebp");
            code.Add("mov ebp, esp");
            var idx = code.Count;
            code.Add("");
            var off = 0;
            foreach (var s in t.Code)
            {
                var m = _rx.Match(s);
                if (!m.Success)
                {
                    code.Add(s);
                    continue;
                }

                var key2 = m.Groups[2].ToString();

                var o = _symbolTable[key2];

                if (o < off) off = o;
                var y= _rx.Replace(s, $"dword ptr[ebp{o:+###;-###;+0}]");

                if (y.Contains("mov eax, byte")) y = y.Replace(" eax, byte", "sx eax, byte");
                else if (y.Contains("mov eax, word")) y = y.Replace(" eax, word", "sx eax, word");
                else if (y.Contains("byte") && y.Contains("eax")) y=y.Replace("eax", "al");
                else if (new Regex(@"[^dqtozy]word").Match(y).Success && y.Contains("eax")) y = y.Replace("eax", "ax");

                code.Add(y);
            }

            code[idx] = $"sub esp, {-off}";

            code.Add($"add esp, {-off}");
            code.Add("mov esp, ebp");
            code.Add("pop ebp");
            code.Add($"ret {_ret}");
            code.Add($"{_function.Definition.Name} ENDP");
        }
    }
}