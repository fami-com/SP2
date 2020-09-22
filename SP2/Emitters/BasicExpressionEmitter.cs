using System;
using SP2.Blocks;

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
                char c => $"mov edx, {(int)c}",
                int n => $"mov edx, {n}",
                {} e => throw new Exception($"Unrecognized type: {e}")
            });
        }
    }
}