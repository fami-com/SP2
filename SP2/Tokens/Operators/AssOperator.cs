using System;
using SP2.Definitions;

namespace SP2.Tokens.Operators
{
    internal class AssOperator : BinaryOperator,IEquatable<AssOperator>
    {
        public readonly AssOperatorKind Op;

        public AssOperator(string @operator)
        {
            Op = @operator switch
            {
                "=" => AssOperatorKind.Ass,
                "+=" => AssOperatorKind.AssAdd,
                "-=" => AssOperatorKind.AssSub,
                "*=" => AssOperatorKind.AssMul,
                "/=" => AssOperatorKind.AssDiv,
                "%=" => AssOperatorKind.AssMod,
                "&=" => AssOperatorKind.AssAnd,
                "|=" => AssOperatorKind.AssOr,
                "^=" => AssOperatorKind.AssXor,
                "<<=" => AssOperatorKind.AssSl,
                ">>=" => AssOperatorKind.AssSr,
                _ => throw new ArgumentException($"Unsupported assignment operator: {@operator}")
            };
        }

        public override string ToString()
        {
            return Op switch
            {
                AssOperatorKind.Ass => "=",
                AssOperatorKind.AssAdd => "+=",
                AssOperatorKind.AssSub => "-=",
                AssOperatorKind.AssMul => "*=",
                AssOperatorKind.AssDiv => "/=",
                AssOperatorKind.AssMod => "%=",
                AssOperatorKind.AssAnd => "&=",
                AssOperatorKind.AssOr => "|=",
                AssOperatorKind.AssXor => "^=",
                AssOperatorKind.AssSl => "<<=",
                AssOperatorKind.AssSr => ">>=",
                _ => throw new ArgumentException($"Unsupported assignment operator: {Op.ToString()}")
            };
        }

        public static bool operator ==(AssOperator lhs, AssOperator rhs) =>
            lhs.Op == rhs.Op;
        
        public static bool operator !=(AssOperator lhs, AssOperator rhs) =>
            lhs.Op != rhs.Op;

        public bool Equals(AssOperator other) => other is {} t && this == t;

        public override bool Equals(object obj) => obj is AssOperator o && this == o;
        public override int GetHashCode() => Op.GetHashCode();
    }
}