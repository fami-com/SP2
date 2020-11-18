using SP2.Tokens.Operators;
#pragma warning disable 8618

namespace SP2.Tokens.Expressions.AssignmentExpression
{
    internal class CompoundAssignmentExpression : AssignmentExpression
    {
        public AssOperator Operator;

        public override string ToString() => $"{Lvalue} {Operator} {Rvalue}";
    }
}