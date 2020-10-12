using System;
using SP2.Definitions;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class BinaryBitExpressionEmitter : Emitter
    {
        private readonly BinaryBitExpression expression;

        public BinaryBitExpressionEmitter(BinaryBitExpression expr)
        {
            expression = expr;
        }
        
        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(expression.Lhs).CodeI);
            code.Add("push eax");
            code.AddRange(new ExpressionEmitter(expression.Rhs).CodeI);
            code.Add("mov ecx, eax");
            code.Add("pop eax");

            var op = expression.Operator.Op switch
            {
                BinaryBitKind.Or => "or",
                BinaryBitKind.Xor => "xor",
                BinaryBitKind.And => "and",
                BinaryBitKind.Sl => "sal",
                BinaryBitKind.Sr => "sar",
                _ => throw new Exception("Unsupported BinaryBitKind")
            };
            
            if(op=="sal"||op=="sar") code.Add($"{op} eax, cl");
            else code.Add($"{op} eax, ecx");
        }
    }
}