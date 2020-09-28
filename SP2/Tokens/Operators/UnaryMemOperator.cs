using System;
using SP2.Definitions;

namespace SP2.Tokens.Operators
{
    internal class UnaryMemOperator : UnaryOperator, IEquatable<UnaryMemOperator>
    {
        public readonly UnaryMemKind Op;

        public UnaryMemOperator(string @operator)
        {
            throw new Exception("Unimplemented, need to decouple this");
            // ReSharper disable once HeuristicUnreachableCode
            Op = @operator switch
            {
                "*" => UnaryMemKind.Deref,
                "&" => UnaryMemKind.Ref,
                _ => throw new ArgumentException("Unsupported operator found")
            };
        }

        public override string ToString() => Op switch
        {
            UnaryMemKind.Deref => "*",
            UnaryMemKind.Ref => "&",
            _ => throw new ArgumentException("Unknown UnaryOperatorKind")
        };

        public static bool operator ==(UnaryMemOperator lhs, UnaryMemOperator rhs) =>
            lhs.Op == rhs.Op;
        public static bool operator !=(UnaryMemOperator lhs, UnaryMemOperator rhs) =>
            lhs.Op != rhs.Op;

        public bool Equals(UnaryMemOperator? other) => other is {} t && this == t;

        public override bool Equals(object? obj) => obj is UnaryMemOperator o && this == o;
        public override int GetHashCode() => Op.GetHashCode();
    }
}