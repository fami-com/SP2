using SP2.Tokens.Operators;
#pragma warning disable 8618

namespace SP2.Tokens.Expressions
{
    internal class BinaryValExpression : BinaryExpression
    {
        public RvalueExpression Lhs;
        public RvalueExpression Rhs;
        public BinaryValOperator Operator;

        public override string ToString() => $"{Lhs} {Operator} {Rhs}";
    }
}