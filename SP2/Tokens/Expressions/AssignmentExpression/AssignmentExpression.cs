using System;
using SP2.Tokens.Operators;

#pragma warning disable 8618

namespace SP2.Tokens.Expressions.AssignmentExpression
{
    internal class AssignmentExpression : BinaryExpression
    {
        public LvalueExpression Lvalue;
        public Expression Rvalue;
        
        public override string ToString()
        {
            return this switch
            {
                SimpleAssignmentExpression se => se.ToString(),
                CompoundAssignmentExpression ce => ce.ToString(),
                _ => throw new Exception()
            };
        }
    }
}