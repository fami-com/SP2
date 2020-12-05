using SP2.Tokens.Expressions;
using SP2.Tokens.Partial;

namespace SP2.Tokens.Statements
{
    class ForStatement : Statement
    {
        public ForLoopInit Init;
        public Expression Condition;
        public Expression Iteration;
        public Statement Statement;

        public override string ToString()
        {
            return $"for ({Init}; {Condition}; {Iteration}) {Statement}";
        }
    }
}