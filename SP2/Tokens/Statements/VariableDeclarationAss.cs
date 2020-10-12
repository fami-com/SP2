using SP2.Tokens.Expressions;

namespace SP2.Tokens.Statements
{
    class VariableDeclarationAss : Statement
    {
        // Don't set directly please, use the properties
        public VariableDeclaration Variable;
        public Expression Rvalue;

        public string Identifier
        {
            get => Variable.Identifier;
            set => Variable.Identifier = value;
        }
        public Type Type
        {
            get => Variable.Type;
            set => Variable.Type = value;
        }

        public VariableDeclarationAss()
        {
            Variable = new VariableDeclaration();
        }

        public override string ToString()
        {
            return $"{Type} {Identifier} = {Rvalue};";
        }
    }
}