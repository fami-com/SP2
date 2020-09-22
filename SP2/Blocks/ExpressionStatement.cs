namespace SP2.Blocks
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