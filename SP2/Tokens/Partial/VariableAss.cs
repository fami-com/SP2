using SP2.Tokens.Expressions;

namespace SP2.Tokens.Partial
{
    class VariableAss : IToken
    {
        public string Identifier;
        public Type Type;
        public Expression Value;

        public override string ToString()
        {
            return $"{Type} {Identifier} = {Value}";
        }
    }
}