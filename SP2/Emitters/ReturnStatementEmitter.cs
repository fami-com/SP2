using System.Collections.Generic;
using SP2.Emitters.Expressions;
using SP2.Tokens;
using SP2.Tokens.Statements;

namespace SP2.Emitters
{
    internal class ReturnStatementEmitter : Emitter
    {
        private readonly ReturnStatement returnStatement;

        public ReturnStatementEmitter(ReturnStatement statement)
        {
            returnStatement = statement;
            code = new List<string>();
        }

        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(returnStatement.Expression).CodeI);
        }
    }
}