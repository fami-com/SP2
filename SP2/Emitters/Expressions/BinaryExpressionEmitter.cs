using System;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class BinaryExpressionEmitter : Emitter
    {
        private readonly BinaryExpression _expression;

        public BinaryExpressionEmitter(BinaryExpression expr)
        {
            _expression = expr;
        }
        
        public override void Emit()
        {
            code.AddRange(_expression switch
            {
                AssignmentExpression ae => new AssignmentExpressionEmitter(ae).CodeI,
                BinaryValExpression ve => new BinaryValExpressionEmitter(ve).CodeI,
                BinaryBitExpression be => new BinaryBitExpressionEmitter(be).CodeI,
                BinaryCmpExpression ce => new BinaryCmpExpressionEmitter(ce).CodeI,
                BinaryLogExpression le => new BinaryLogExpressionEmitter(le).CodeI,
                _ => throw new Exception("Unsuppored BinaryExpression")
            });
        }
    }
}