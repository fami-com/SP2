using System;
using System.Collections.Generic;
using SP2.Tokens.Partial;
using SP2.Tokens.Statements;

namespace SP2.Emitters.Statements
{
    internal class StatementEmitter : Emitter
    {
        private readonly Statement _statement;

        public StatementEmitter(Statement stat)
        {
            _statement = stat;
        }

        public override void Emit()
        {
            switch (_statement)
            {
                case null:
                    code = new List<string>();
                    break;
                case Block bs:
                    var tb = new BlockEmitter(bs);
                    code = tb.CodeI;
                    break;
                case ReturnStatement rs:
                    code = new ReturnStatementEmitter(rs).CodeI;
                    break;
                case VariableDeclaration vds:
                    var tv = new VariableDeclarationEmitter(vds);
                    code = tv.CodeI;
                    break;
                case VariableDeclarationAss vdas:
                    var tva = new VariableDeclarationAssEmitter(vdas);
                    code = tva.CodeI;
                    break;
                case ExpressionStatement es:
                    var ese = new ExpressionStatementEmitter(es);
                    code = ese.CodeI;
                    break;
                case ConditionalStatement cd:
                    var cde = new ConditionalStatementEmitter(cd);
                    code = cde.CodeI;
                    break;
                case Variable vs:
                    var vse = new PartialVariableEmitter(vs);
                    code = vse.CodeI;
                    break;
                case {} e:
                    throw new Exception($"Unxpected expression: {e}");
            }
        }
    }
}