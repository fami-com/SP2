using System;
using SP2.Emitters.Expressions;
using SP2.Tokens.Statements;
using Type = SP2.Tokens.Type;

namespace SP2.Emitters.Statements
{
    class VariableDeclarationAssEmitter : Emitter
    {
        private readonly VariableDeclarationAss decl;

        public VariableDeclarationAssEmitter(VariableDeclarationAss d) => decl = d;
        
        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(decl.Rvalue).CodeI);
            code.Add($"mov #@{decl.Identifier}, eax");
        }
    }
}