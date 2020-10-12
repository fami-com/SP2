using SP2.Emitters.Expressions;
using SP2.Tokens;
using SP2.Tokens.Statements;

namespace SP2.Emitters.Statements
{
    class VariableDeclarationAssEmitter : Emitter
    {
        private readonly VariableDeclarationAss decl;
        public (string s, Type t) Symbol;

        public VariableDeclarationAssEmitter(VariableDeclarationAss d) => decl = d;
        
        public override void Emit()
        {
            Symbol = (decl.Identifier, decl.Type);
            code.AddRange(new ExpressionEmitter(decl.Rvalue).CodeI);
            code.Add($"mov #@{decl.Identifier}, eax");
        }
    }
}