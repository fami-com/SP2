using System;
using SP2.Tokens.Expressions.AssignmentExpression;

namespace SP2.Emitters.Expressions.Assignment
{
    internal class AssignmentExpressionEmitter : Emitter
    {
        private AssignmentExpression _expr;

        public AssignmentExpressionEmitter(AssignmentExpression e) => _expr = e;
        
        public override void Emit()
        {
            code.AddRange(_expr switch
            {
                CompoundAssignmentExpression cae => new CompoundAssignmentExpressionEmitter(cae).CodeI,
                SimpleAssignmentExpression sae => new SimpleAssignmentExpressionEmitter(sae).CodeI,
                _ => throw new ArgumentOutOfRangeException(nameof(_expr))
            });
        }
    }
}