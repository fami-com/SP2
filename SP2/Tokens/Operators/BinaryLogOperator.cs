using System;
using SP2.Definitions;
namespace SP2.Tokens.Operators
{
    internal partial class BinaryLogOperator : BinaryOperator,IEquatable<BinaryLogOperator>
    {
        public readonly BinaryLogKind Op;

        public BinaryLogOperator(string @operator)
        {
            Op = @operator switch
            {
                "&&" => BinaryLogKind.And,
                "||" => BinaryLogKind.Or,
                _ => throw new ArgumentException("Unsupported operator found")
            };
        }

        public override string ToString()
        {
            return Op switch
            {
                BinaryLogKind.And => "&&",
                BinaryLogKind.Or => "||",
                _ => throw new ArgumentException("Unsupported BinaryLogKind")
            };
        }
        
        public static bool operator ==(BinaryLogOperator lhs, BinaryLogOperator rhs) =>
            lhs.Op == rhs.Op;

        public static bool operator !=(BinaryLogOperator lhs, BinaryLogOperator rhs) =>
            lhs.Op != rhs.Op;

        public bool Equals(BinaryLogOperator? other) => other is {} t && this == t;

        public override bool Equals(object? obj) => obj is BinaryLogOperator o && this == o;
        public override int GetHashCode() => Op.GetHashCode();
    }
}