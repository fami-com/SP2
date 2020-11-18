using System;
using SP2.Tokens.Expressions;

namespace SP2.Emitters.Expressions
{
    internal class ValueExpressionEmitter : Emitter
    {
        private readonly ValueExpression _expression;

        private enum S { S, U }
        public ValueExpressionEmitter(ValueExpression expr)
        {
            _expression = expr;
        }

        public override void Emit()
        {
            var (op, n, ss) = _expression.ToReturn switch
            {
                byte b => ($"mov al, {b}", 1, S.U),
                sbyte b => ($"mov al, {b}", 1, S.S),
                short s => ($"mov ax, {s}", 2, S.S),
                ushort s => ($"mov ax, {s}", 2, S.U),
                int i => ($"mov eax, {i}", 4, S.S),
                uint i => ($"mov eax, {i}", 4, S.U),
                long l => ($"mov rax, {l}", 8, S.S),
                ulong l => ($"mov rax, {l}", 8, S.U),
                {} e => throw new Exception($"Unrecognized type: {e.GetType()}")
            };
            
            code.Add(op);
            if (n == 4) return;
            
            var inst = ss switch
            {
                S.S => "movsx",
                S.U => "movzx",
                _ => throw new Exception("Can't happen")
            };
            var src = n switch
            {
                1 => "al",
                2 => "ax",
                8 => throw new ArgumentOutOfRangeException("64 bit numbers aren't supported"),
                _ => throw new ArgumentOutOfRangeException(),
            };

            code.Add($"{inst} eax, {src}");
        }
    }
}