using System;
using SP2.Blocks;

namespace SP2.Emitters
{
    class ExpressionEmitter : Emitter
    {
        private Expression expression;

        public ExpressionEmitter(Expression expr)
        {
            expression = expr;
        }

        public override void Emit()
        {
            code = expression switch
            {
                BasicExpression be => new BasicExpressionEmitter(be).CodeI,
                {} e => throw new Exception($"Unexpected expression: {e}")
            };
        }
    }
}