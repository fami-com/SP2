#pragma warning disable 8618
namespace SP2.Tokens
{
    internal class FunctionDefinition
    {
        public Type Type;
        public string Name;

        public override string ToString()
        {
            return $"{Type} {Name}()";
        }
    }
}