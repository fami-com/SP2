using SP2.Tokens.Expressions;
#pragma warning disable 8618

namespace SP2.Tokens.Statements
{
    internal class ConditionalStatement : Statement
    {
        public Expression Condition;
        public Statement IfTrue;
        public Statement IfFalse;

        public override string ToString()
        {
            var s = $"if ({Condition}) {IfTrue}";
            if (IfFalse is {})
            {
                s += $"\nelse {IfFalse}";
            }

            return s;
        }
    }
}