#pragma warning disable 8618
namespace SP2.Tokens.Expressions
{
    internal class VariableExpression : LvalueExpression
    {
        public Identifier Identifier;

        public override string ToString() => Identifier.Name;
    }
}