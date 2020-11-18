namespace SP2.Tokens
{
    class FunctionDeclaration : TopLevel
    {
        public FunctionDefinition Definition;

        public override string ToString() => $"{Definition};";
    }
}