using System;
using SP2.Tokens.Statements;
using Type = SP2.Tokens.Type;

namespace SP2.Emitters.Statements
{
    class VariableDeclarationEmitter : Emitter
    {
        private readonly VariableDeclaration decl;

        public VariableDeclarationEmitter(VariableDeclaration d) => decl = d;
        
        public override void Emit()
        {
            
        }
    }
}