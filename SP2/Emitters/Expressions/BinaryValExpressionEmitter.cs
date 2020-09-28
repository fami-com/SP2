using System;
using SP2.Definitions;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class BinaryValExpressionEmitter : Emitter
    {
        private readonly BinaryValExpression expression;

        public BinaryValExpressionEmitter(BinaryValExpression expr)
        {
            expression = expr;
        }

        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(expression.Lhs).CodeI);
            code.Add("push eax");
            code.AddRange(new ExpressionEmitter(expression.Rhs).CodeI);
            code.Add("mov edx, eax");
            code.Add("pop eax");

            var op = expression.Operator.Op switch
            {
                BinaryValKind.Add => "add",
                BinaryValKind.Sub => "sub",
                BinaryValKind.Mul => "mul",
                BinaryValKind.Div => "div", BinaryValKind.Mod => "div",
                BinaryValKind.Comma => null,
                _ => throw new Exception("Unsupported BinaryValKind")
            };

            if (op is null) return;
            
            if (expression.Operator.Op == BinaryValKind.Add || expression.Operator.Op == BinaryValKind.Sub)
            {
                code.Add($"{op} eax, edx");
            }
            else
            {
                code.Add($"{op} edx");
                if (expression.Operator.Op == BinaryValKind.Mod) code.Add("mov eax, edx");
            }
        }
    }
}