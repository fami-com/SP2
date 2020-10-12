using System.ComponentModel;

namespace SP2.Definitions
{
    enum TypeKind
    {
        [Description("char")]
        Char,
        
        [Description("short")]
        Short,
        
        [Description("int")]
        Int,
        
        [Description("long")]
        Long,
        
        [Description("long long")]
        LongLong,
        
        [Description("void")]
        Void,
        
        [Description("float")]
        Float,
        
        [Description("double")]
        Double,
        
        [Description("long double")]
        LongDouble
    }
}