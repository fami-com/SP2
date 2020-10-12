using SP2.Tokens.Expressions.AssignmentExpression;

namespace SP2.Emitters.Expressions.Assignment
{
    class SimpleAssignmentExpressionEmitter : Emitter
    {
        private readonly SimpleAssignmentExpression expr;

        public SimpleAssignmentExpressionEmitter(SimpleAssignmentExpression e) => expr = e;
        public override void Emit()
        {
            var lv = new LvalueExpressionEmitter(expr.Lvalue);
            lv.Emit();
            var addr = lv.Addr;
            code.AddRange(new ExpressionEmitter(expr.Rvalue).CodeI);
            code.Add($"mov {addr}, eax");
        }
    }
}