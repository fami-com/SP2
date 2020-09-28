using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class LvalueExpressionEmitter  : Emitter
    {
        private LvalueExpression expression;

        public LvalueExpressionEmitter(LvalueExpression expr)
        {
            expression = expr;
        }
        
        public override void Emit()
        {
            throw new System.NotImplementedException();
        }
    }
}