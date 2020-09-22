namespace SP2.Blocks
{
    class FunctionDefinition
    {
        public string Type;
        public string Name;

        public override string ToString()
        {
            return $"{Type} {Name}()";
        }
    }
}