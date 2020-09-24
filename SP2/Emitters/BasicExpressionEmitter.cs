using System;
using SP2.Tokens;

namespace SP2.Emitters
{
    class BasicExpressionEmitter : Emitter
    {
        private readonly BasicExpression expression;

        public BasicExpressionEmitter(BasicExpression expr)
        {
            expression = expr;
        }

        public override void Emit()
        {
            code.Add(expression.ToReturn switch
            {
                char c => $"mov eax, {(int)c}",
                int n => $"mov eax, {n}",
                {} e => throw new Exception($"Unrecognized type: {e}")
            });
        }
    }
}