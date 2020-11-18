using System;
using SP2.Definitions;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class UnaryValExpressionEmitter : Emitter
    {
        private readonly UnaryValExpression _expression;

        public UnaryValExpressionEmitter(UnaryValExpression expr)
        {
            _expression = expr;
        }

        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(_expression.Expression).CodeI);

            if (_expression.Operator.Op == UnaryValKind.Plus) return;
            
            var op = _expression.Operator.Op switch
            {
                UnaryValKind.Bnot => "not",
                UnaryValKind.Lnot => "setnz",
                UnaryValKind.Minus => "neg",
                _ => throw new Exception("Unsupported UnaryValKind")
            };

            switch (op)
            {
                case "setnz":
                    code.Add("test eax, eax");
                    code.Add($"{op} al");
                    code.Add("movzx eax, al");
                    break;
                default:
                    code.Add($"{op} eax");
                    break;
            }
        }
    }
}