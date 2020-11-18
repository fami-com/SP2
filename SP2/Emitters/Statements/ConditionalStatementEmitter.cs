using SP2.Emitters.Expressions;
using SP2.Tokens.Statements;

namespace SP2.Emitters.Statements
{
    internal class ConditionalStatementEmitter : Emitter
    {
        private static int _counter;
        private static int Counter => _counter++;
        
        private readonly ConditionalStatement _cond;

        public ConditionalStatementEmitter(ConditionalStatement cond)
        {
            _cond = cond;
        }

        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(_cond.Condition).CodeI);
            code.Add("test eax, eax");
            var t1 = Counter;

            code.Add(_cond.IfFalse is {} ? $"jz CE{t1}" : $"jz CF{t1}");
            
            code.AddRange(new StatementEmitter(_cond.IfTrue).CodeI);
            
            if (_cond.IfFalse is {})
            {
                code.Add($"jmp CF{t1}");
                code.Add($"CE{t1}:");
                code.AddRange(new StatementEmitter(_cond.IfFalse).CodeI);
            }
            code.Add($"CF{t1}:");
        }
    }
}