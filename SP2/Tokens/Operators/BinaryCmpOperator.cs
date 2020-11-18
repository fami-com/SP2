using System;
using SP2.Definitions;

namespace SP2.Tokens.Operators
{
    internal partial class BinaryCmpOperator : BinaryOperator,IEquatable<BinaryCmpOperator>
    {
        public readonly BinaryCmpKind Op;

        public BinaryCmpOperator(string @operator)
        {
            Op = @operator switch
            {
                "==" => BinaryCmpKind.Eq,
                "!=" => BinaryCmpKind.Neq,
                ">" =>  BinaryCmpKind.Gt,
                ">=" => BinaryCmpKind.Ge,
                "<" =>  BinaryCmpKind.Lt,
                "<=" => BinaryCmpKind.Le,
                _ => throw new ArgumentException("Unsupported operator")
            };
        }

        public override string ToString()
        {
            return Op switch
            {
                BinaryCmpKind.Eq => "==",
                BinaryCmpKind.Neq => "!=",
                BinaryCmpKind.Gt => ">",
                BinaryCmpKind.Ge => ">=",
                BinaryCmpKind.Lt => "<",
                BinaryCmpKind.Le => "<=",
                _ => throw new ArgumentException("Unsupported BinaryCmpKind")
            };
        }
        
        public static bool operator ==(BinaryCmpOperator lhs, BinaryCmpOperator rhs) =>
            rhs is { } && lhs is { } && lhs.Op == rhs.Op;

        public static bool operator !=(BinaryCmpOperator lhs, BinaryCmpOperator rhs) =>
            rhs is { } && lhs is { } && lhs.Op != rhs.Op;

        public bool Equals(BinaryCmpOperator other) => other is {} t && this == t;

        public override bool Equals(object obj) => obj is BinaryCmpOperator o && this == o;
        public override int GetHashCode() => Op.GetHashCode();
    }
}