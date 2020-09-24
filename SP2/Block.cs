using System.Collections.Generic;
using System.Text;
using SP2.Tokens;

namespace SP2
{
    class Block : Statement
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