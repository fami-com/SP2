using System;
using SP2.Definitions;
using SP2.Tokens.Expressions.AssignmentExpression;

namespace SP2.Emitters.Expressions.Assignment
{
    class CompoundAssignmentExpressionEmitter : Emitter
    {
        private readonly CompoundAssignmentExpression expr;
        public CompoundAssignmentExpressionEmitter(CompoundAssignmentExpression e) => expr = e;
        public override void Emit()
        {
            code.AddRange(new ExpressionEmitter(expr.Rvalue).CodeI);
            code.Add("push eax");
            code.AddRange(new LvalueExpressionEmitter(expr.Lvalue).CodeI);
            code.Add("pop ecx");
            string op = "";
            switch (expr.Operator.Op)
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
                case AssOperatorKind.AssDiv:
                    code.Add("mov eax, [eax]");
                    code.Add("cdq");
                    code.Add("idiv ecx");
                    if (expr.Operator.Op == AssOperatorKind.AssMod) goto case AssOperatorKind.AssMod;
                    else break;
                case AssOperatorKind.AssMod:
                    code.Add("mov eax, edx");
                    break;
                case AssOperatorKind.AssMul:
                    code.Add("mov eax, [eax]");
                    code.Add("cdq");
                    code.Add("imul ecx");
                    break;
                case AssOperatorKind.Ass:
                    code.Add($"{op} [eax], ecx");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}