using System;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class LvalueAsRvalueExpressionEmitter : Emitter
    {
        private LvalueExpression _lvalue;

        public LvalueAsRvalueExpressionEmitter(LvalueExpression lv) => _lvalue = lv;

        public override void Emit()
        {
            code = _lvalue switch
            {
                VariableExpression ve => new VariableExpressionEmitter(ve).CodeI,
                _ => throw new ArgumentOutOfRangeException(nameof(_lvalue))
            };
        }
    }
}