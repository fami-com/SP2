#pragma warning disable 8618
namespace SP2.Tokens.Expressions
{
    internal class ParExpression : RvalueExpression
    {
        public RvalueExpression Expression;

        public override string ToString() => $"({Expression})";
    }
}