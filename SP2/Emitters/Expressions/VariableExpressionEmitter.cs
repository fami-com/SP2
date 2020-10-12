using System;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    class VariableExpressionEmitter : Emitter
    {
        private readonly VariableExpression expr;

        public VariableExpressionEmitter(VariableExpression e) => expr = e;
        
        public override void Emit()
        {
            code.Add($"mov eax, #@{expr.Identifier}");
        }
    }
}