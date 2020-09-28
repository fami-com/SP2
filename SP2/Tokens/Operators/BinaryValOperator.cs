using System;
using SP2.Definitions;

namespace SP2.Tokens.Operators
{
    internal partial class BinaryValOperator : BinaryOperator,IEquatable<BinaryValOperator>
    {
        public readonly BinaryValKind Op;

        public BinaryValOperator(string @operator) =>
            Op = @operator switch
            {
                "+" => BinaryValKind.Add,
                "-" => BinaryValKind.Sub,
                "*" => BinaryValKind.Mul,
                "/" => BinaryValKind.Div,
                "%" => BinaryValKind.Mod,
                "," => BinaryValKind.Comma,
                _ => throw new ArgumentException("Unsupported operator found")
            };

        public override string ToString() =>
            Op switch
            {
                BinaryValKind.Add => "+",
                BinaryValKind.Sub => "-",
                BinaryValKind.Mul => "*",
                BinaryValKind.Div => "/",
                BinaryValKind.Mod => "%",
                BinaryValKind.Comma => ",",
                _ => throw new ArgumentException("Unsupported BinaryOperatorKind")
            };

        public static bool operator ==(BinaryValOperator lhs, BinaryValOperator rhs) =>
            lhs.Op == rhs.Op;

        public static bool operator !=(BinaryValOperator lhs, BinaryValOperator rhs) =>
            lhs.Op != rhs.Op;

        public bool Equals(BinaryValOperator? other) => other is {} t && this == t;

        public override bool Equals(object? obj) => obj is BinaryValOperator o && this == o;
        public override int GetHashCode() => Op.GetHashCode();
    }
}