using System;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class UnaryExpressionEmitter : Emitter
    {
        private readonly UnaryExpression _expression;

        public UnaryExpressionEmitter(UnaryExpression expr)
        {
            _expression = expr;
        }
        
        public override void Emit()
        {
            code.AddRange(_expression switch
            {
                UnaryValExpression ve => new UnaryValExpressionEmitter(ve).CodeI,
                {} e => throw new Exception($"Unsupported expression: {e}")
            });
        }
    }
}