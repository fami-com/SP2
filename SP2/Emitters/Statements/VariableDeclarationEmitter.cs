using SP2.Tokens.Statements;

namespace SP2.Emitters.Statements
{
    internal class VariableDeclarationEmitter : Emitter
    {
        private readonly VariableDeclaration _decl;

        public VariableDeclarationEmitter(VariableDeclaration d) => _decl = d;
        
        public override void Emit()
        {
            
        }
    }
}