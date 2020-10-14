using System;
using System.Collections.Generic;
using SP2.Emitters.Expressions;
using SP2.Tokens.Statements;

namespace SP2.Emitters.Statements
{
    internal class ExpressionStatementEmitter : Emitter
    {
        private readonly ExpressionStatement expressionStatement;

        public ExpressionStatementEmitter(ExpressionStatement es)
        {
            expressionStatement = es;
            code = new List<string>();
        }

        public override void Emit()
        {
            var t =new ExpressionEmitter(expressionStatement.Expression);
            code = t.CodeI;
        }
        
    }
}