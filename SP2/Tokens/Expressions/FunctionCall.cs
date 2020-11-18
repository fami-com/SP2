using System.Collections.Generic;
using SP2.Emitters.Expressions;

namespace SP2.Tokens.Expressions
{
    class FunctionCall : RvalueExpression
    {
        public string Identifier;
        public List<Expression> Arguments;

        public override string ToString()
        {
            return $"{Identifier}({string.Join(", ", Arguments)})";
        }
    }
}