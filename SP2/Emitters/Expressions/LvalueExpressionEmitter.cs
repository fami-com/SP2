using System;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class LvalueExpressionEmitter  : Emitter
    {
        private LvalueExpression expression;
        public string Addr;
        
        public LvalueExpressionEmitter(LvalueExpression expr)
        {
            expression = expr;
        }
        
        public override void Emit()
        {
            switch (expression)
            {
                case VariableExpression ve:
                    var t = new VariableAddressEmitter(ve);
                    code = t.CodeI;
                    Addr = t.Addr;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(expression));
            }
            
#if DEBUG
            Console.WriteLine($"Lem: {Symbol}");
#endif
        }
    }
}