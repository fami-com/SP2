using SP2.Tokens.Operators;
#pragma warning disable 8618

namespace SP2.Tokens.Expressions
{
    internal class UnaryValExpression : UnaryExpression
    {
        public RvalueExpression Expression;
        public UnaryValOperator Operator;

        public override string ToString() => $"{Operator}{Expression}";
    }
}