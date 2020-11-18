using SP2.Tokens.Expressions;
using SP2.Tokens.Statements;

namespace SP2.Tokens.Partial
{
    class Variable : Statement
    {
        public string Identifier;
        public Type Type;

        public override string ToString()
        {
            return $"{Type} {Identifier}";
        }
    }
}