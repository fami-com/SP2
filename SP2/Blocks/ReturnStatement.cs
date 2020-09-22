namespace SP2.Blocks
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