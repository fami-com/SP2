using System;
using SP2.Definitions;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class BinaryCmpExpressionEmitter: Emitter
    {
        private readonly BinaryCmpExpression expression;

        public BinaryCmpExpressionEmitter(BinaryCmpExpression expr)
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
            
            code.Add("cmp eax, ecx");

            var op = expression.Operator.Op switch
            {
                BinaryCmpKind.Eq => "sete",
                BinaryCmpKind.Neq => "setne",
                BinaryCmpKind.Gt => "seta",
                BinaryCmpKind.Ge => "setae",
                BinaryCmpKind.Lt => "setb",
                BinaryCmpKind.Le => "setbe",
                _ => throw new Exception("Unsupported BinaryCmpKind")
            };
            
            code.Add("pushf");
            code.Add("xor eax, eax");
            code.Add("popf");
            code.Add($"{op} al");
        }
    }
}