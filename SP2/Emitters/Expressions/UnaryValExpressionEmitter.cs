using System;
using SP2.Definitions;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class UnaryValExpressionEmitter : Emitter
    {
        private readonly UnaryValExpression expression;

        public UnaryValExpressionEmitter(UnaryValExpression expr)
        {
            expression = expr;
        }

        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(expression.Expression).CodeI);

            var op = expression.Operator.Op switch
            {
                UnaryValKind.Bnot => "not",
                UnaryValKind.Lnot => "setz",
                UnaryValKind.Minus => "neg",
                UnaryValKind.Plus => "",
                _ => throw new Exception("Unsupported UnaryValKind")
            };

            if (op == "") return;
            
            switch (op)
            {
                case "setz":
                    code.Add("test eax, eax");
                    goto default;
                default:
                    code.Add($"{op} eax");
                    break;
            }
        }
    }
}