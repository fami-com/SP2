using System;
using System.ComponentModel;
using SP2.Definitions;

namespace SP2.Tokens
{
    class Type : IEquatable<Type>
    {
        public readonly TypeKind Kind;

        public int Size => Kind switch
        {
            TypeKind.Char => 1,
            TypeKind.Short => 2,
            TypeKind.Int => 4,
            TypeKind.Long => 8,
            TypeKind.LongLong => 8,
            TypeKind.Void => 0,
            TypeKind.Float => 4,
            TypeKind.Double => 8,
            TypeKind.LongDouble => 10,
            _ => throw new ArgumentOutOfRangeException()
        };

        public string Ptr => Kind switch
        {
            TypeKind.Char =>"byte",
            TypeKind.Short =>"word",
            TypeKind.Int =>"dword",
            TypeKind.Long =>"qword",
            TypeKind.LongLong =>"qword",
            TypeKind.Void =>"",
            TypeKind.Float =>"dword",
            TypeKind.Double =>"qword",
            TypeKind.LongDouble =>"tword",
            _ => throw new ArgumentOutOfRangeException()
        };

        public Type(string type)
        {
            Kind = type switch
            {
                "char" => TypeKind.Char,
                "short" => TypeKind.Short,
                "int" => TypeKind.Int,
                "long" => TypeKind.Long,
                "long long" => TypeKind.LongLong,
                "float" => TypeKind.Float,
                "double" => TypeKind.Double,
                "long double" => TypeKind.LongDouble,
                "void" => TypeKind.Void,
                "u" => TypeKind.Unknown,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override string ToString() => Kind switch
        {
            TypeKind.Char => "char",
            TypeKind.Short => "short",
            TypeKind.Int => "int",
            TypeKind.Long => "long",
            TypeKind.LongLong => "long long",
            TypeKind.Float => "float",
            TypeKind.Double => "double",
            TypeKind.LongDouble => "long double",
            TypeKind.Void => "void",
            TypeKind.Unknown => "u",
            _ => throw new ArgumentOutOfRangeException()
        };

        public static bool operator ==(Type lhs, Type rhs) => lhs.Kind == rhs.Kind;
        public static bool operator !=(Type lhs, Type rhs) => lhs.Kind != rhs.Kind;

        public bool Equals(Type? other) => other is {} t && this == t;

        public override bool Equals(object? obj) => obj is Type t && this == t;

        public override int GetHashCode()
        {
            return (int) Kind;
        }
    }
}