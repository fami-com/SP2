using SP2.Tokens.Operators;

namespace SP2.Tokens.Expressions
{
    internal class UnaryValExpression : UnaryExpression
    {
        public Expression Expression;
        public UnaryValOperator Operator;

        public override string ToString() => $"{Operator}{Expression}";
    }
}