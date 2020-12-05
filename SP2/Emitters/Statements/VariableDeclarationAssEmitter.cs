using System;
using SP2.Emitters.Expressions;
using SP2.Tokens.Statements;

namespace SP2.Emitters.Statements
{
    internal class VariableDeclarationAssEmitter : Emitter
    {
        private readonly VariableDeclarationAss _decl;

        public VariableDeclarationAssEmitter(VariableDeclarationAss d) => _decl = d;
        
        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(_decl.Rvalue).CodeI);
            code.Add($"mov #@{_decl.Identifier}, eax");
        }
    }
}