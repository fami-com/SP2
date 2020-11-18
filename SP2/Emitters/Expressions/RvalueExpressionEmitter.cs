using System;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class RvalueExpressionEmitter : Emitter
    {
        private readonly RvalueExpression _expression;

        public RvalueExpressionEmitter(RvalueExpression expr)
        {
            _expression = expr;
        }

        public override void Emit()
        {
            code = _expression switch
            {
                BinaryExpression be => new BinaryExpressionEmitter(be).CodeI,
                UnaryExpression ue => new UnaryExpressionEmitter(ue).CodeI,
                ParExpression pe => new ParExpressionEmitter(pe).CodeI,
                ValueExpression ve => new ValueExpressionEmitter(ve).CodeI,
                FunctionCall ce => new FunctionCallEmitter(ce).CodeI,
                LvalueExpression le => new LvalueAsRvalueExpressionEmitter(le).CodeI,
                {} e => throw new Exception($"Unexpected rvalue expression: {e}"),
            };
        }
    }
}