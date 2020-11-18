namespace SP2.Tokens.Expressions.AssignmentExpression
{
    internal class SimpleAssignmentExpression : AssignmentExpression
    {
        
        public override string ToString()
        {
            return $"{Lvalue} = {Rvalue}";
        }
    }
}