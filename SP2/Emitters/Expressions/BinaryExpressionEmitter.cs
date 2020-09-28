using System;
using SP2.Tokens.Expressions;
using SP2.Tokens.Operators;

namespace SP2.Emitters.Expressions
{
    internal class BinaryExpressionEmitter : Emitter
    {
        private readonly BinaryExpression expression;

        public BinaryExpressionEmitter(BinaryExpression expr)
        {
            expression = expr;
        }
        
        public override void Emit()
        {
            code.AddRange(expression switch
            {
                BinaryValExpression ve => new BinaryValExpressionEmitter(ve).CodeI,
                BinaryBitExpression be => new BinaryBitExpressionEmitter(be).CodeI,
                BinaryCmpExpression ce => new BinaryCmpExpressionEmitter(ce).CodeI,
                BinaryLogExpression le => new BinaryLogExpressionEmitter(le).CodeI,
                _ => throw new Exception("Unsuppored BinaryExpression")
            });
        }
    }
}