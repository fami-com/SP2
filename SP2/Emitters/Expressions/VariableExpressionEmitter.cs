using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class VariableExpressionEmitter : Emitter
    {
        private readonly VariableExpression _expr;

        public VariableExpressionEmitter(VariableExpression e) => _expr = e;
        
        public override void Emit()
        {
            code.Add($"mov eax, #@{_expr.Identifier}");
        }
    }
}