using System.Collections.Generic;
using SP2.Tokens.Partial;

#pragma warning disable 8618
namespace SP2.Tokens
{
    internal class FunctionDefinition
    {
        public Type Type;
        public string Name;
        public List<Variable> Variables;

        public override string ToString()
        {
            return $"{Type} {Name}({string.Join(", ", Variables)})";
        }
    }
}