namespace SP2.Tokens
{
    class ExpressionStatement : Statement
    {
        public Expression Expression;

        public override string ToString()
        {
            return $"{Expression};";
        }
    }
}