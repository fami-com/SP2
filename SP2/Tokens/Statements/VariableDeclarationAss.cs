using SP2.Tokens.Expressions;

namespace SP2.Tokens.Statements
{
    internal class VariableDeclarationAss : Statement
    {
        // Don't set directly please, use the properties
        public Type Type;
        public Identifier Identifier;
        public Expression Rvalue;

        public override string ToString()
        {
            return $"{Type} {Identifier} = {Rvalue};";
        }
    }
}