using System.Collections.Generic;
using System.Text;
#pragma warning disable 8618

namespace SP2.Tokens.Statements
{
    internal class Block : Statement
    {
        public List<Statement> Statements;

        public override string ToString()
        {
            var sb = new StringBuilder("{\n");

            foreach (var s in Statements)
            {
                sb.Append($"{s}\n");
            }

            sb.Append("}");
            return sb.ToString();
        }
    }
}