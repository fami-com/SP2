using System;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class ExpressionEmitter : Emitter
    {
        private readonly Expression _expression;

        public ExpressionEmitter(Expression expr)
        {
            _expression = expr;
        }

        public override void Emit()
        {
            code = _expression switch
            {
                RvalueExpression re => new RvalueExpressionEmitter(re).CodeI,
                {} e => throw new Exception($"Unexpected expression: {e}"),
                _ => code
            };
        }
    }
}