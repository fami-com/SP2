namespace SP2.Tokens.Expressions
{
    internal class VariableExpression
    {
        public readonly string Identifier;
        public readonly Type Type;

        public override string ToString() => Identifier;
    }
}