#pragma warning disable 8618
namespace SP2.Tokens.Expressions
{
    internal class VariableExpression : LvalueExpression
    {
        public string Identifier;

        public override string ToString() => Identifier;
    }
}