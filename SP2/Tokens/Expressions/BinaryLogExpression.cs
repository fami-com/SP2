using SP2.Tokens.Operators;

namespace SP2.Tokens.Expressions
{
    internal class BinaryLogExpression : BinaryExpression
    {
        public Expression Lhs;
        public Expression Rhs;
        public BinaryLogOperator Operator;

        public override string ToString() => $"{Lhs} {Operator} {Rhs}";
    }
}