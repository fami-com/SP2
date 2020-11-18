using System;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class LvalueExpressionEmitter  : Emitter
    {
        private LvalueExpression _expression;
        public string Addr;
        
        public LvalueExpressionEmitter(LvalueExpression expr)
        {
            _expression = expr;
        }
        
        public override void Emit()
        {
            switch (_expression)
            {
                case VariableExpression ve:
                    var t = new VariableAddressEmitter(ve);
                    code = t.CodeI;
                    Addr = t.Addr;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_expression));
            }
        }
    }
}