using System;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class RvalueExpressionEmitter : Emitter
    {
        private readonly RvalueExpression expression;

        public RvalueExpressionEmitter(RvalueExpression expr)
        {
            expression = expr;
        }

        public override void Emit()
        {
            code.AddRange(expression switch
            {
                BinaryExpression be => new BinaryExpressionEmitter(be).CodeI,
                UnaryExpression ue => new UnaryExpressionEmitter(ue).CodeI,
                ParExpression pe => new ParExpressionEmitter(pe).CodeI,
                ValueExpression ve => new ValueExpressionEmitter(ve).CodeI,
                LvalueExpression le => new LvalueAsRvalueExpressionEmitter(le).CodeI,
                {} e => throw new Exception($"Unexpected rvalue expression: {e}")
            });
        }
    }
}