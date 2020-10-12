using SP2.Tokens.Expressions;
#pragma warning disable 8618

namespace SP2.Tokens.Statements
{
    internal class ReturnStatement : Statement
    {
        public Expression Expression;

        public override string ToString()
        {
            return $"return {Expression};";
        }
    }
}