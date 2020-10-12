using System;
using System.Reflection.Metadata;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    class VariableAddressEmitter:Emitter
    {
        private readonly VariableExpression expr;
        public string Addr;
        public VariableAddressEmitter(VariableExpression e) => expr = e;
        
        public override void Emit()
        {
            Addr = $"#@{expr.Identifier}";
        }
    }
}