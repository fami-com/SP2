using System;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class ValueExpressionEmitter : Emitter
    {
        private readonly ValueExpression expression;

        public ValueExpressionEmitter(ValueExpression expr)
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