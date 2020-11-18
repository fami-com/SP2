using System;
using SP2.Definitions;

namespace SP2.Tokens.Operators
{
    internal class PrefixIncDecOperator : PrefixOperator, IEquatable<PrefixIncDecOperator>
    {
        public readonly IncDecKind Op;

        public PrefixIncDecOperator(string @operator) => Op = @operator switch
        {
            "++" => IncDecKind.Inc,
            "--" => IncDecKind.Dec,
            _ => throw new ArgumentException("Unsupported operator found")
        };

        public override string ToString() => Op switch
        {
            IncDecKind.Inc => "++",
            IncDecKind.Dec => "--",
            _ => throw new ArgumentException("Unsupported IncDecKind")
        };

        public static bool operator ==(PrefixIncDecOperator lhs, PrefixIncDecOperator rhs) =>
            rhs is { } && lhs is { } && lhs.Op == rhs.Op;
        
        public static bool operator !=(PrefixIncDecOperator lhs, PrefixIncDecOperator rhs) =>
            rhs is { } && lhs is { } && lhs.Op != rhs.Op;

        public bool Equals(PrefixIncDecOperator other) => other is {} t && this == t;

        public override bool Equals(object obj) => obj is PrefixIncDecOperator o && this == o;
        public override int GetHashCode() => Op.GetHashCode();
    }
}