namespace SP2.Tokens
{
    class ReturnStatement : Statement
    {
        public Expression Expression;

        public override string ToString()
        {
            return $"return {Expression};";
        }
    }
}