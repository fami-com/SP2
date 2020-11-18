using System;
using SP2.Definitions;

namespace SP2.Tokens.Operators
{
    internal partial class UnaryValOperator : PrefixOperator, IEquatable<UnaryValOperator>
    {
        public readonly UnaryValKind Op;

        public UnaryValOperator(string @operator)
        {
            Op = @operator switch
            {
                "~" => UnaryValKind.Bnot, "compl" => UnaryValKind.Bnot,
                "!" => UnaryValKind.Lnot, "not" => UnaryValKind.Lnot,
                "+" => UnaryValKind.Plus,
                "-" => UnaryValKind.Minus,
                Keywords.Sizeof => UnaryValKind.Sizeof,
                _ => throw new ArgumentException("Invalid operator")
            };
        }

        public override string ToString()
        {
            return Op switch
            {
                UnaryValKind.Bnot => "~",
                UnaryValKind.Lnot => "!",
                UnaryValKind.Plus => "+",
                UnaryValKind.Minus => "-",
                UnaryValKind.Sizeof=>Keywords.Sizeof,
                _ => throw new ArgumentException("Unknown UnaryValKind")
            };
        }

        public static bool operator ==(UnaryValOperator lhs, UnaryValOperator rhs) =>
            rhs is { } && lhs is { } && lhs.Op == rhs.Op;

        public static bool operator !=(UnaryValOperator lhs, UnaryValOperator rhs) =>
            rhs is { } && lhs is { } && lhs.Op != rhs.Op;

        public bool Equals(UnaryValOperator other) => other is {} t && this == t;

        public override bool Equals(object obj) => obj is UnaryValOperator o && this == o;
        public override int GetHashCode() => Op.GetHashCode();
    }
}