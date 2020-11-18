using System.Linq;
using SP2.Definitions;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    class FunctionCallEmitter : Emitter
    {
        private FunctionCall _func;

        public FunctionCallEmitter(FunctionCall func) => _func = func;

        public override void Emit()
        {
            var h = _func.Arguments.Copy().ToList();
            h.Reverse();
            foreach (var e in h.Select(expr => new ExpressionEmitter(expr)))
            {
                code.AddRange(e.CodeI);
                code.Add("push eax");
            }
            code.Add($"call {_func.Identifier}");
        }
    }
}