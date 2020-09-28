namespace SP2.Tokens
{
    internal class FunctionDefinition
    {
        public string Type;
        public string Name;

        public override string ToString()
        {
            return $"{Type} {Name}()";
        }
    }
}