using System;
using System.Text.RegularExpressions;
using SP2.Emitters.Expressions;
using SP2.Tokens.Statements;

namespace SP2.Emitters.Statements
{
    class ForLoopEmitter : Emitter
    {
        private static int Id;
        private ForStatement _loop;
        
        private Regex _rx = new Regex(@"jmp FOR[EI]\{0\}");

        public ForLoopEmitter(ForStatement loop)
        {
            _loop = loop;
        }


        public override void Emit()
        {
            var n = Id;
            Id++;

            if (!(_loop.Init is null))
            {
                code.AddRange(new ForLoopInitEmitter(_loop.Init).CodeI);
            }
            
            code.Add($"FORC{n}:");
            if (!(_loop.Condition is null))
            {
                code.AddRange(new ExpressionEmitter(_loop.Condition).CodeI);
            }

            code.Add("test eax, eax");
            code.Add($"jz FORE{n}");
            foreach (var c in new StatementEmitter(_loop.Statement).CodeI)
            {
                var k = c;
                if (_rx.IsMatch(c))
                {
                    k = string.Format(c, n);
                }
                code.Add(k);
            }
            code.Add($"FORI{n}:");
            if (!(_loop.Iteration is null))
            {
                code.AddRange(new ExpressionEmitter(_loop.Iteration).CodeI);
            }

            code.Add($"jmp FORC{n}");
            code.Add($"FORE{n}:");
        }
    }
}