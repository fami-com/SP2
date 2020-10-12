namespace SP2.Tokens.Expressions.AssignmentExpression
{
    class SimpleAssignmentExpression : AssignmentExpression
    {
        
        public override string ToString()
        {
            return $"{Lvalue} = {Rvalue}";
        }
    }
}