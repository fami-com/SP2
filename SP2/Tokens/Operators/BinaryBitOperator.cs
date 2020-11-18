using System;
using SP2.Definitions;

namespace SP2.Tokens.Operators
{
    internal class BinaryBitOperator : BinaryOperator,IEquatable<BinaryBitOperator>
    {
        public readonly BinaryBitKind Op;

        public BinaryBitOperator(string @operator)
        {
            Op = @operator switch
            {
                "&" => BinaryBitKind.And, "bitand" => BinaryBitKind.And,
                "|" => BinaryBitKind.Or, "bitor" => BinaryBitKind.Or,
                "^" => BinaryBitKind.Xor, "bitxor" => BinaryBitKind.Xor,
                ">>" => BinaryBitKind.Sr,
                "<<" => BinaryBitKind.Sl,
                _ => throw new ArgumentException("Unsupported operator found")
            };
        }

        public override string ToString()
        {
            return Op switch
            {
                BinaryBitKind.And => "&",
                BinaryBitKind.Or => "|",
                BinaryBitKind.Xor => "^",
                BinaryBitKind.Sr => ">>",
                BinaryBitKind.Sl => "<<",
                _ => throw new ArgumentException("Unsupported BinaryBinKind")
            };
        }
        
        public static bool operator ==(BinaryBitOperator lhs, BinaryBitOperator rhs) =>
            rhs is { } && lhs is { } && lhs.Op == rhs.Op;

        public static bool operator !=(BinaryBitOperator lhs, BinaryBitOperator rhs) =>
            rhs is { } && lhs is { } && lhs.Op != rhs.Op;

        public bool Equals(BinaryBitOperator other) => other is {} t && this == t;

        public override bool Equals(object obj) => obj is BinaryBitOperator o && this == o;
        public override int GetHashCode() => Op.GetHashCode();
    }
}