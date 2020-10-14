using System;
using System.Collections.Generic;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    class LvalueAsRvalueExpressionEmitter : Emitter
    {
        private LvalueExpression lvalue;

        public LvalueAsRvalueExpressionEmitter(LvalueExpression lv) => lvalue = lv;

        public override void Emit()
        {
            code = lvalue switch
            {
                VariableExpression ve => new VariableExpressionEmitter(ve).CodeI,
                _ => throw new ArgumentOutOfRangeException(nameof(lvalue))
            };
        }
    }
}