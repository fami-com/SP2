namespace SP2.Blocks
{
    class BasicExpression : Expression
    {
        public readonly object ToReturn;

        public BasicExpression(object toReturn)
        {
            ToReturn = toReturn;
        }

        public override string ToString()
        {
            return ToReturn switch
            {
                char c => $"'{c}'",
                {} x => $"{x}"
            };
        }
    }
}