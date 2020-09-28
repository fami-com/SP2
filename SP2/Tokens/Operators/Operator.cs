namespace SP2.Tokens.Operators
{
    internal abstract class Operator
    {
    }

    internal abstract class UnaryOperator
    {
    }

    internal abstract class PrefixOperator : UnaryOperator
    {
    }

    internal abstract class PostfixOperator : UnaryOperator
    {
    }

    internal abstract class BinaryOperator : Operator
    {
    }
}