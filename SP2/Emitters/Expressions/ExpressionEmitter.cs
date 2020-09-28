using System;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class ExpressionEmitter : Emitter
    {
        private readonly Tokens.Expressions.Expression expression;

        public ExpressionEmitter(Tokens.Expressions.Expression expr)
        {
            expression = expr;
        }

        public override void Emit()
        {
            code = expression switch
            {
                ValueExpression be => new ValueExpressionEmitter(be).CodeI,
                LvalueExpression le => new LvalueExpressionEmitter(le).CodeI,
                RvalueExpression re => new RvalueExpressionEmitter(re).CodeI,
                {} e => throw new Exception($"Unexpected expression: {e}")
            };
        }
    }
}