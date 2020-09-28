using System;
using SP2.Definitions;

namespace SP2.Tokens.Operators
{
    internal class PostfixIncDecOperator : PostfixOperator, IEquatable<PostfixIncDecOperator>
    {
        public readonly IncDecKind Op;

        public PostfixIncDecOperator(string @operator) => Op = @operator switch
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

        public static bool operator ==(PostfixIncDecOperator lhs, PostfixIncDecOperator rhs) =>
            lhs.Op == rhs.Op;
        
        public static bool operator !=(PostfixIncDecOperator lhs, PostfixIncDecOperator rhs) =>
            lhs.Op != rhs.Op;

        public bool Equals(PostfixIncDecOperator? other) => other is {} t && this == t;

        public override bool Equals(object? obj) => obj is PostfixIncDecOperator o && this == o;
        public override int GetHashCode() => Op.GetHashCode();
    }
}