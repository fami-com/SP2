using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class BinaryLogExpressionEmitter: Emitter
    {
        private readonly BinaryLogExpression expression;

        public BinaryLogExpressionEmitter(BinaryLogExpression expr)
        {
            expression = expr;
        }
        
        public override void Emit()
        {
            throw new System.NotImplementedException();
        }
    }
}