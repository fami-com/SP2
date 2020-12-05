using SP2.Tokens.Operators;

#pragma warning disable 8618

namespace SP2.Tokens.Expressions
{
    internal class AssignmentExpression : BinaryExpression
    {
        public LvalueExpression Lvalue;
        public Expression Rvalue;
        public AssOperator Operator;

        public override string ToString() => $"{Lvalue} {Operator} {Rvalue}";
    }
}