using SP2.Tokens.Expressions.AssignmentExpression;

namespace SP2.Emitters.Expressions.Assignment
{
    internal class SimpleAssignmentExpressionEmitter : Emitter
    {
        private readonly SimpleAssignmentExpression _expr;

        public SimpleAssignmentExpressionEmitter(SimpleAssignmentExpression e) => _expr = e;
        public override void Emit()
        {
            var lv = new LvalueExpressionEmitter(_expr.Lvalue);
            lv.Emit();
            var addr = lv.Addr;
            code.AddRange(new ExpressionEmitter(_expr.Rvalue).CodeI);
            code.Add($"mov {addr}, eax");
        }
    }
}