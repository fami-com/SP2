using System;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class VariableAddressEmitter:Emitter
    {
        private readonly VariableExpression _expr;
        public string Addr;
        
        public VariableAddressEmitter(VariableExpression e) => _expr = e;
        
        public override void Emit()
        {
            Addr = $"#@{_expr.Identifier}";
        }
    }
}