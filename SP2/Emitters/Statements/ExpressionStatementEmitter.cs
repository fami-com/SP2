using System.Collections.Generic;
using SP2.Emitters.Expressions;
using SP2.Tokens.Statements;

namespace SP2.Emitters.Statements
{
    internal class ExpressionStatementEmitter : Emitter
    {
        private readonly ExpressionStatement _expressionStatement;

        public ExpressionStatementEmitter(ExpressionStatement es)
        {
            _expressionStatement = es;
            code = new List<string>();
        }

        public override void Emit()
        {
            var t =new ExpressionEmitter(_expressionStatement.Expression);
            code = t.CodeI;
        }
        
    }
}