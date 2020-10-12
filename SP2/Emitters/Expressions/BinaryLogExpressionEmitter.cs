using System;
using SP2.Definitions;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class BinaryLogExpressionEmitter: Emitter
    {
        private static int _counter;
        private static int Counter => _counter++;
        private readonly BinaryLogExpression expression;

        public BinaryLogExpressionEmitter(BinaryLogExpression expr)
        {
            expression = expr;
        }
        
        public override void Emit()
        {
            
            code.AddRange(new ExpressionEmitter(expression.Lhs).CodeI);
            code.Add("test eax, eax");
            switch (expression.Operator.Op)
            {
                case BinaryLogKind.And:
                    var t11 = Counter;
                    var t21 = Counter;

                    code.Add($@"jz @L{t11}");
                    code.AddRange(new ExpressionEmitter(expression.Rhs).CodeI);
                    code.Add("test eax, eax");
                    code.Add($@"jz @L{t11}");
                    code.Add("mov eax, 1");
                    code.Add($@"jmp @L{t21}");
                    code.Add($@"@L{t11}:");
                    code.Add("xor eax, eax");
                    code.Add($@"@L{t21}:");
                    break;
                case BinaryLogKind.Or:
                    var t12 = Counter;
                    var t22 = Counter;
                    var t32 = Counter;

                    code.Add($@"jnz @L{t12}");
                    code.AddRange(new ExpressionEmitter(expression.Rhs).CodeI);
                    code.Add("test eax, eax");
                    code.Add($@"jz @L{t22}");
                    code.Add($@"@L{t12}:");
                    code.Add("mov eax, 1");
                    code.Add($@"jmp @L{t32}");
                    code.Add($@"@L{t22}:");
                    code.Add("xor eax, eax");
                    code.Add($@"@L{t32}:");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}