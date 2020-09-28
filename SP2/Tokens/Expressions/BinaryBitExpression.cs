using SP2.Tokens.Operators;

namespace SP2.Tokens.Expressions
{
    internal class BinaryBitExpression : BinaryExpression
    {       
        public Expression Lhs;
        public Expression Rhs;
        public BinaryBitOperator Operator;

        public override string ToString() => $"{Lhs} {Operator} {Rhs}";
    }
}