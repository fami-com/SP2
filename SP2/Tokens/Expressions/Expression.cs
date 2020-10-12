namespace SP2.Tokens.Expressions
{
    internal abstract class Expression
    {

    }

    internal abstract class BinaryExpression : RvalueExpression
    {
    }

    internal abstract class UnaryExpression : RvalueExpression
    {
        
    }

    internal abstract class LvalueExpression : RvalueExpression
    {
        
    }

    internal abstract class RvalueExpression : Expression
    {
        
    }
}