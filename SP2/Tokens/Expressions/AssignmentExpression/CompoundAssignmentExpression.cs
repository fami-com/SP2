using SP2.Tokens.Operators;

namespace SP2.Tokens.Expressions.AssignmentExpression
{
    class CompoundAssignmentExpression : AssignmentExpression
    {
        public AssOperator Operator;

        public override string ToString() => $"{Lvalue} {Operator} {Rvalue}";
    }
}