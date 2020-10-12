namespace SP2.Tokens.Expressions
{
    internal class ValueExpression : RvalueExpression
    {
        public object ToReturn;
 
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