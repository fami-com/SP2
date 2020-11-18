using SP2.Tokens.Partial;

namespace SP2.Emitters.Statements
{
    class PartialVariableEmitter : Emitter
    {
        private readonly Variable _decl;

        public PartialVariableEmitter(Variable d) => _decl = d;

        public override void Emit()
        {

        }
    }
}