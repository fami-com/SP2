using System;
using Microsoft.VisualBasic;
using SP2.Definitions;
using SP2.Tokens.Expressions.AssignmentExpression;

namespace SP2.Emitters.Expressions.Assignment
{
    internal class CompoundAssignmentExpressionEmitter : Emitter
    {
        private readonly CompoundAssignmentExpression _expr;
        public CompoundAssignmentExpressionEmitter(CompoundAssignmentExpression e) => _expr = e;
        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(_expr.Rvalue).CodeI);
            code.Add("push eax");
            var tmp = new LvalueExpressionEmitter(_expr.Lvalue); tmp.Emit();
            var addr = tmp.Addr;
            code.Add($"mov eax, {addr}");
            code.Add("pop ecx");
            Console.WriteLine(string.Join('\n', code));
            var op = "";
            switch (_expr.Operator.Op)
            {
                case AssOperatorKind.AssAdd:
                    op = "add";
                    goto case AssOperatorKind.Ass;
                case AssOperatorKind.AssSub:
                    op = "sub";
                    goto case AssOperatorKind.Ass;
                case AssOperatorKind.AssAnd:
                    op = "and";
                    goto case AssOperatorKind.Ass;
                case AssOperatorKind.AssOr:
                    op = "or";
                    goto case AssOperatorKind.Ass;
                case AssOperatorKind.AssXor:
                    op = "xor";
                    goto case AssOperatorKind.Ass;
                case AssOperatorKind.AssSl:
                    op = "sal";
                    goto case AssOperatorKind.Ass;
                case AssOperatorKind.AssSr:
                    op = "sar";
                    goto case AssOperatorKind.Ass;
                case AssOperatorKind.AssMod:
                case AssOperatorKind.AssDiv:
                    code.Add("cdq");
                    code.Add("idiv ecx");
                    if (_expr.Operator.Op == AssOperatorKind.AssMod) code.Add("mov eax, edx");
                    code.Add($"mov {addr}, eax");
                    break;
                case AssOperatorKind.AssMul:
                    code.Add("cdq");
                    code.Add("imul ecx");
                    code.Add($"mov {addr}, eax");
                    break;
                case AssOperatorKind.Ass:
                    code.Add($"{op} eax, ecx");
                    code.Add($"mov {addr}, eax");
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}