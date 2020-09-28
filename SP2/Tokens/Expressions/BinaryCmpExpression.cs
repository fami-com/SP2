using SP2.Tokens.Operators;

namespace SP2.Tokens.Expressions
{
    internal class BinaryCmpExpression : BinaryExpression
    {
        public Expression Lhs;
        public Expression Rhs;
        public BinaryCmpOperator Operator;

        public override string ToString() => $"{Lhs} {Operator} {Rhs}";
    }
}