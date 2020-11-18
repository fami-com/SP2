using System;
using SP2.Definitions;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class BinaryValExpressionEmitter : Emitter
    {
        private readonly BinaryValExpression _expression;

        public BinaryValExpressionEmitter(BinaryValExpression expr)
        {
            _expression = expr;
        }

        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(_expression.Lhs).CodeI);
            code.Add("push eax");
            code.AddRange(new ExpressionEmitter(_expression.Rhs).CodeI);
            code.Add("mov ecx, eax");
            code.Add("pop eax");

            var op = _expression.Operator.Op switch
            {
                BinaryValKind.Add => "add",
                BinaryValKind.Sub => "sub",
                BinaryValKind.Mul => "imul",
                BinaryValKind.Div => "idiv", BinaryValKind.Mod => "idiv",
                BinaryValKind.Comma => null,
                _ => throw new Exception("Unsupported BinaryValKind")
            };

            if (op is null) return;
            
            if (_expression.Operator.Op == BinaryValKind.Add || _expression.Operator.Op == BinaryValKind.Sub)
            {
                code.Add($"{op} eax, ecx");
            }
            else
            {
                code.Add("cdq");
                code.Add($"{op} ecx");
                if (_expression.Operator.Op == BinaryValKind.Mod) code.Add("mov eax, edx");
            }
        }
    }
}