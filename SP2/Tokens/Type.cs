using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SP2.Definitions;

namespace SP2.Tokens
{
    internal class Type : IEquatable<Type>
    {
        public readonly bool Volatile;
        public readonly bool Const;
        public readonly bool Signed;
        public readonly bool Unsigned;
        public readonly bool Register;
        
        public readonly bool Enum;
        public readonly bool Union;
        public readonly bool Struct;

        public readonly string Name;

        public Type(string name, IEnumerable<string> modifiers)
        {
            Name = name;

            var enumerable = modifiers.ToList();
            
            Enum = enumerable.Contains(TypeModifiers.Enum);
            Const = enumerable.Contains(TypeModifiers.Const);
            Signed = enumerable.Contains(TypeModifiers.Signed);
            Unsigned = enumerable.Contains(TypeModifiers.Unsigned);
            Volatile = enumerable.Contains(TypeModifiers.Volatile);
            Register = enumerable.Contains(TypeModifiers.Register);
            Union = enumerable.Contains(TypeModifiers.Union);
            Struct = enumerable.Contains(TypeModifiers.Struct);

            if ((Enum || Union || Struct) && (Signed || Unsigned))
                throw new ConstraintException("The type can't be compound and specify signedness");
            
            if (Signed && Unsigned) throw new ConstraintException("The type can't be both signed and unsigned");
            if (Union && Struct) throw new ConstraintException("The type can't be both a union and a struct");
            if (Enum && Struct) throw new ConstraintException("The type can't be both an enum and a struct");
            if (Enum && Union) throw new ConstraintException("The type can't be both an enum and a union");
        }

        public override string ToString()
        {
            var hold = new List<string>(9);

            if (Volatile) hold.Add(TypeModifiers.Volatile);
            if (Register) hold.Add(TypeModifiers.Register);
            if (Const) hold.Add(TypeModifiers.Const);
            if (Signed) hold.Add(TypeModifiers.Signed);
            if (Unsigned) hold.Add(TypeModifiers.Unsigned);
            if (Enum) hold.Add(TypeModifiers.Enum);
            if (Union) hold.Add(TypeModifiers.Union);
            if (Struct) hold.Add(TypeModifiers.Struct);
            hold.Add(Name);

            return string.Join(' ', hold);
        }

        public static bool operator ==(Type lhs, Type rhs) => lhs.Volatile == rhs.Volatile && lhs.Const == rhs.Const &&
                                                              lhs.Signed == rhs.Signed &&
                                                              lhs.Unsigned == rhs.Unsigned &&
                                                              lhs.Register == rhs.Register && lhs.Enum == rhs.Enum &&
                                                              lhs.Struct == rhs.Struct && lhs.Union == rhs.Union &&
                                                              lhs.Name == rhs.Name;
        
        public static bool operator !=(Type lhs, Type rhs) => lhs.Volatile != rhs.Volatile || lhs.Const != rhs.Const ||
                                                              lhs.Signed != rhs.Signed ||
                                                              lhs.Unsigned != rhs.Unsigned ||
                                                              lhs.Register != rhs.Register || lhs.Enum != rhs.Enum ||
                                                              lhs.Struct != rhs.Struct || lhs.Union != rhs.Union ||
                                                              lhs.Name != rhs.Name;

        public override bool Equals(object? obj) => obj is Type t && this == t;

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Volatile);
            hashCode.Add(Const);
            hashCode.Add(Signed);
            hashCode.Add(Unsigned);
            hashCode.Add(Register);
            hashCode.Add(Enum);
            hashCode.Add(Union);
            hashCode.Add(Struct);
            hashCode.Add(Name);
            return hashCode.ToHashCode();
        }

        public bool Equals(Type? other) => other is { } t && this == t;
    }
}