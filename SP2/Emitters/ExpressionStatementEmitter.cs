using System.Collections.Generic;
using SP2.Tokens;

namespace SP2.Emitters
{
    class ExpressionStatementEmitter : Emitter
    {
        private readonly ExpressionStatement expressionStatement;

        public ExpressionStatementEmitter(ExpressionStatement es)
        {
            expressionStatement = es;
            code = new List<string>();
        }

        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(expressionStatement.Expression).CodeI);
        }
    }
}