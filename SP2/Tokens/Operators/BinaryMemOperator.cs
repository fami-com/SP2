using System;
using SP2.Definitions;
namespace SP2.Tokens.Operators
{
    internal partial class BinaryMemOperator : BinaryOperator,IEquatable<BinaryMemOperator>
    {
        public readonly BinaryMemKind Op;

        public BinaryMemOperator(string @operator)
        {
            Op = @operator switch
            {
                "." => BinaryMemKind.Member,
                "->" => BinaryMemKind.Indirect,
                _ => throw new ArgumentException("Unsupported operator found")
            };
        }
        
        public override string ToString()
        {
            return Op switch
            {
                BinaryMemKind.Member => ".",
                BinaryMemKind.Indirect => "->",
                _ => throw new ArgumentException("Unsupported BinaryMemKind")
            };
        }

        public static bool operator ==(BinaryMemOperator lhs, BinaryMemOperator rhs) =>
            lhs.Op == rhs.Op;
        public static bool operator !=(BinaryMemOperator lhs, BinaryMemOperator rhs) =>
            lhs.Op != rhs.Op;

        public bool Equals(BinaryMemOperator? other) => other is {} t && this == t;

        public override bool Equals(object? obj) => obj is BinaryMemOperator o && this == o;
        public override int GetHashCode() => Op.GetHashCode();
    }
}