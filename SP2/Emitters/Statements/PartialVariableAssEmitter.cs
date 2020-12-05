using SP2.Emitters.Expressions;
using SP2.Tokens.Partial;

namespace SP2.Emitters.Statements
{
    class PartialVariableAssEmitter:Emitter
    {
        private VariableAss _var;

        public PartialVariableAssEmitter(VariableAss @var)
        {
            _var = var;
        }

        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(_var.Value).CodeI);
            code.Add($"mov #@{_var.Identifier}, eax");
        }
    }
}