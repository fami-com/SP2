using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class ParExpressionEmitter : Emitter
    {
        private readonly ParExpression _expression;

        public ParExpressionEmitter(ParExpression expr)
        {
            _expression = expr;
        }

        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(_expression.Expression).CodeI);
        }
    }
}