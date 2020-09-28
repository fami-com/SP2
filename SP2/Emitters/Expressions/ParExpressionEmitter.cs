using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class ParExpressionEmitter : Emitter
    {
        private readonly ParExpression expression;

        public ParExpressionEmitter(ParExpression expr)
        {
            expression = expr;
        }

        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(expression.Expression).CodeI);
        }
    }
}