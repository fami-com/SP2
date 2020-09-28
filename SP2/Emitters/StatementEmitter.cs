using System;
using SP2.Tokens;
using SP2.Tokens.Statements;

namespace SP2.Emitters
{
    internal class StatementEmitter : Emitter
    {
        private readonly Statement statement;

        public StatementEmitter(Statement stat)
        {
            statement = stat;
        }

        public override void Emit()
        {
            code = statement switch
            {
                ReturnStatement rs => new ReturnStatementEmitter(rs).CodeI,
                ExpressionStatement es => new ExpressionStatementEmitter(es).CodeI,
                {} e => throw new Exception($"Unxpected expression: {e}")
            };
        }
    }
}