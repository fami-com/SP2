using System;
#pragma warning disable 8618

namespace SP2.Tokens.Statements
{
    internal class VariableDeclaration : Statement, IEquatable<VariableDeclaration>
    {
        public Type Type;
        public Identifier Identifier;

        public override string ToString() => $"{Type} {Identifier};";

        public static bool operator ==(VariableDeclaration lhs, VariableDeclaration rhs) =>
            rhs is { } && lhs is { } && lhs.Type == rhs.Type && lhs.Identifier == rhs.Identifier;
        public static bool operator !=(VariableDeclaration lhs, VariableDeclaration rhs) =>
            rhs is { } && lhs is { } && (lhs.Type != rhs.Type || lhs.Identifier != rhs.Identifier);

        public override bool Equals(object obj) => obj is VariableDeclaration v && this == v;
        
        public bool Equals(VariableDeclaration other) => other is {} v && this == v;
        
        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Identifier);
        }
    }
}