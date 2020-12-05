using SP2.Tokens.Expressions;
using SP2.Tokens.Statements;

namespace SP2.Tokens.Partial
{
    class ForLoopInit : IToken
    {
        public VariableAss Init1;
        public Expression Init2;

        public override string ToString()
        {
            return Init2 is null ? $"{Init1}" : $"{Init2}";
        }
    }
}