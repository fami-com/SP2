using System;
using SP2.Definitions;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class AssignmentExpressionEmitter : Emitter
    {
        private AssignmentExpression _expr;

        public AssignmentExpressionEmitter(AssignmentExpression e) => _expr = e;
        
        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(_expr.Rvalue).CodeI);
            code.Add("push eax");
            var tmp = new LvalueExpressionEmitter(_expr.Lvalue); tmp.Emit();
            var addr = tmp.Addr;
            code.Add($"mov eax, {addr}");
            code.Add("pop ecx");
            var op = "";
            switch (_expr.Operator.Op)
            {
                case AssOperatorKind.AssAdd:
                    op = "add";
                    goto default;
                case AssOperatorKind.AssSub:
                    op = "sub";
                    goto default;
                case AssOperatorKind.AssAnd:
                    op = "and";
                    goto default;
                case AssOperatorKind.AssOr:
                    op = "or";
                    goto default;
                case AssOperatorKind.AssXor:
                    op = "xor";
                    goto default;
                case AssOperatorKind.AssSl:
                    op = "sal";
                    goto default;
                case AssOperatorKind.AssSr:
                    op = "sar";
                    goto default;
                case AssOperatorKind.AssMod:
                case AssOperatorKind.AssDiv:
                    code.Add("cdq");
                    code.Add("idiv ecx");
                    if (_expr.Operator.Op == AssOperatorKind.AssMod) code.Add("mov eax, edx");
                    goto case AssOperatorKind.Ass;
                case AssOperatorKind.AssMul:
                    code.Add("cdq");
                    code.Add("imul ecx");
                    goto case AssOperatorKind.Ass;
                case AssOperatorKind.Ass:
                    code.Add($"mov {addr}, eax");
                    return;
                default:
                    code.Add($"{op} eax, ecx");
                    goto case AssOperatorKind.Ass;
            }
        }
    }
}