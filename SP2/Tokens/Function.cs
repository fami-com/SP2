using System.Collections.Generic;
using SP2.Tokens.Statements;
#pragma warning disable 8618

namespace SP2.Tokens
{
    internal class Function : TopLevel
    {
        public FunctionDefinition Definition;
        public Block Body;
        public List<string> Variables;

        public override string ToString()
        {
            return $"{Definition} {Body}";
        }
    }
}