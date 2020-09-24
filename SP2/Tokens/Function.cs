namespace SP2.Tokens
{
    class Function : TopLevel
    {
        public FunctionDefinition Definition;
        public Block Body;

        public override string ToString()
        {
            return $"{Definition} {Body}";
        }
    }
}