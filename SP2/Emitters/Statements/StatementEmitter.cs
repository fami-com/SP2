using System;
using System.Collections.Generic;
using SP2.Tokens.Statements;

namespace SP2.Emitters.Statements
{
    internal class StatementEmitter : Emitter
    {
        private readonly Statement statement;

        public List<(string s, Tokens.Type t)> Symbols;
        
        public StatementEmitter(Statement stat)
        {
            statement = stat;
            Symbols = new List<(string s, Tokens.Type t)>();
        }

        public override void Emit()
        {
            switch (statement)
            {
                case Block bs:
                    var tb = new BlockEmitter(bs);
                    code = tb.CodeI;
                    Symbols.AddRange(tb.Symbols);
                    break;
                case ReturnStatement rs:
                    code = new ReturnStatementEmitter(rs).CodeI;
                    break;
                case VariableDeclaration vds:
                    var tv = new VariableDeclarationEmitter(vds);
                    code = tv.CodeI;
                    Symbols.Add(tv.Symbol);
                    break;
                case VariableDeclarationAss vdas:
                    var tva = new VariableDeclarationAssEmitter(vdas);
                    code = tva.CodeI;
                    Symbols.Add(tva.Symbol);
                    break;
                case ExpressionStatement es:
                    code = new ExpressionStatementEmitter(es).CodeI;
                    break;
                case {} e:
                    throw new Exception($"Unxpected expression: {e}");
            }
        }
    }
}