using SP2.Tokens;
using SP2.Tokens.Statements;

namespace SP2.Emitters.Statements
{
    class VariableDeclarationEmitter : Emitter
    {
        private readonly VariableDeclaration decl;
        public (string s, Type t) Symbol;

        public VariableDeclarationEmitter(VariableDeclaration d) => decl = d;
        
        public override void Emit()
        {
            Symbol = (decl.Identifier, decl.Type);
        }
    }
}