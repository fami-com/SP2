using System;
using SP2.Tokens.Expressions.AssignmentExpression;

namespace SP2.Emitters.Expressions.Assignment
{
    class AssignmentExpressionEmitter : Emitter
    {
        private AssignmentExpression expr;

        public AssignmentExpressionEmitter(AssignmentExpression e) => expr = e;
        
        public override void Emit()
        {
            code.AddRange(expr switch
            {
                CompoundAssignmentExpression cae => new CompoundAssignmentExpressionEmitter(cae).CodeI,
                SimpleAssignmentExpression sae => new SimpleAssignmentExpressionEmitter(sae).CodeI,
                _ => throw new ArgumentOutOfRangeException(nameof(expr))
            });
        }
    }
}