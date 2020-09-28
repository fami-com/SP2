using System;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class UnaryExpressionEmitter : Emitter
    {
        private readonly UnaryExpression expression;

        public UnaryExpressionEmitter(UnaryExpression expr)
        {
            expression = expr;
        }
        
        public override void Emit()
        {
            code.AddRange(expression switch
            {
                UnaryValExpression ve => new UnaryValExpressionEmitter(ve).CodeI,
                {} e => throw new Exception($"Unsupported expression: {e}")
            });
        }
    }
}