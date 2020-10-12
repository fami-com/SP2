using System;
using System.Linq;
using SP2.Definitions;
using SP2.Tokens;
using SP2.Tokens.Expressions;
using SP2.Tokens.Expressions.AssignmentExpression;
using SP2.Tokens.Operators;
using SP2.Tokens.Statements;
using Sprache;
using Type = SP2.Tokens.Type;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

namespace SP2
{
    internal static class Grammar
    {
        #region Identifiers

        internal static readonly Parser<char> FirstIdentifierChar = Parse.Letter.Or(Parse.Char('_'));
        internal static readonly Parser<char> NextIdentifierChar = Parse.LetterOrDigit.Or(Parse.Char('_'));

        internal static readonly Parser<string> AnyIdentifier = Parse.Identifier(FirstIdentifierChar, NextIdentifierChar).Token().Named("Identifier");

        #region Keywords
        
        internal static readonly Parser<string> Return = Parse.String(Keywords.Return).Text().Token();
        internal static readonly Parser<string> If = Parse.String(Keywords.If).Text().Token();
        internal static readonly Parser<string> Else = Parse.String(Keywords.Else).Text().Token();
        internal static readonly Parser<string> For = Parse.String(Keywords.For).Text().Token();
        internal static readonly Parser<string> While = Parse.String(Keywords.While).Text().Token();
        internal static readonly Parser<string> Do = Parse.String(Keywords.Do).Text().Token();
        internal static readonly Parser<string> Break = Parse.String(Keywords.Break).Text().Token();
        internal static readonly Parser<string> Continue = Parse.String(Keywords.Continue).Text().Token();
        internal static readonly Parser<string> Goto = Parse.String(Keywords.Goto).Text().Token();
        internal static readonly Parser<string> Switch = Parse.String(Keywords.Switch).Text().Token();
        internal static readonly Parser<string> Case = Parse.String(Keywords.Case).Text().Token();
        internal static readonly Parser<string> Default = Parse.String(Keywords.Default).Text().Token();
        internal static readonly Parser<string> Auto = Parse.String(Keywords.Auto).Text().Token();
        internal static readonly Parser<string> Extern = Parse.String(Keywords.Extern).Text().Token();
        internal static readonly Parser<string> Static = Parse.String(Keywords.Static).Text().Token();
        internal static readonly Parser<string> Restrict = Parse.String(Keywords.Restrict).Text().Token();
        internal static readonly Parser<string> Sizeof = Parse.String(Keywords.Sizeof).Text().Token();
        internal static readonly Parser<string> IntK = Parse.String(Types.Int).Text().Token();
        internal static readonly Parser<string> ShortK = Parse.String(Types.Short).Text().Token();
        internal static readonly Parser<string> CharK = Parse.String(Types.Char).Text().Token();
        internal static readonly Parser<string> LongK = Parse.String(Types.Long).Text().Token();
        internal static readonly Parser<string> FloatK = Parse.String(Types.Float).Text().Token();
        internal static readonly Parser<string> DoubleK = Parse.String(Types.Double).Text().Token();
        internal static readonly Parser<string> Void = Parse.String(Types.Void).Text().Token();

        internal static readonly Parser<string> Keyword = Return.Or(Break).Or(Continue).Or(If).Or(Else).Or(Do).Or(While)
            .Or(For).Or(Goto).Or(Switch).Or(Case).Or(Default).Or(Auto).Or(Extern).Or(Static).Or(Restrict).Or(Sizeof)
            .Or(IntK).Or(ShortK).Or(CharK).Or(LongK).Or(FloatK).Or(DoubleK).Or(Void).Token();
        
        #endregion
        
        internal static readonly Parser<string> Identifier = AnyIdentifier.Except(Keyword).Token();

        #endregion
        
        
        #region Constants

        internal static readonly Parser<char> HexDigit = Extensions.CharsIgnoreCase("0123456789abcdef");
        internal static readonly Parser<char> OctDigit = Parse.Chars("01234567");
        internal static readonly Parser<char> BinDigit = Parse.Chars("01");
        
        #region Integers
        
        internal static readonly Parser<(string num, int b)>
            DecimalIntString = Parse.Number.Text().Select(x => (x, 10));

        internal static readonly Parser<(string num, int b)> BinaryIntString =
            from prefix in Parse.String("0b")
            from num in BinDigit.Many().Text()
            select (num, 2);

        internal static readonly Parser<(string num, int b)> OctalIntString =
            from prefix in Parse.Char('0')
            from num in OctDigit.Many().Text()
            select (num, 8);

        internal static readonly Parser<(string num, int b)> HexadecimalIntString =
            from prefix in Parse.Char('0').Then(_=>Parse.IgnoreCase('x'))
            from num in HexDigit.Many().Text()
            select (num, 16);

        internal static readonly Parser<(string num, int b)> UnsignedIntString =
            DecimalIntString.Or(BinaryIntString).Or(OctalIntString).Or(HexadecimalIntString);

        internal static readonly Parser<(string num, int b)> SignedIntString =
            from sign in Parse.Chars("+-").Optional()
            from number in UnsignedIntString
            select (sign.GetOr('+') + number.num, number.b);

        internal static readonly Parser<int> Int = SignedIntString.Select(x => Convert.ToInt32(x.num, x.b)).Token();

        internal static readonly Parser<char> UnsignedSuffix = Parse.IgnoreCase('u');

        internal static readonly Parser<char> LongSuffix = Parse.IgnoreCase('l');

        internal static readonly Parser<uint> UInt =
            (from num in UnsignedIntString
                from suffix in UnsignedSuffix
                select Convert.ToUInt32(num.num, num.b)).Token();

        internal static readonly Parser<long> Long =
            (from num in SignedIntString
                from suffix in LongSuffix.Repeat(1, 2)
                select Convert.ToInt64(num.num, num.b)).Token();

        internal static readonly Parser<ulong> ULong1 =
            from num in SignedIntString
            from suffix1 in LongSuffix.Repeat(1, 2)
            from suffix2 in UnsignedSuffix
            select Convert.ToUInt64(num.num, num.b);

        internal static readonly Parser<ulong> ULong2 =
            from num in SignedIntString
            from suffix1 in UnsignedSuffix
            from suffix2 in LongSuffix.Repeat(1, 2)
            select Convert.ToUInt64(num.num, num.b);

        internal static readonly Parser<ulong> ULong = ULong1.Or(ULong2).Token();

        internal static readonly Parser<object> Integer =
            ULong.Select(x => (object) x).Or(UInt.Select(x => (object) x))
                .Or(Long.Select(x => (object) x)).Or(Int.Select(x => (object) x)).Named("Integer literal");
        
        #endregion

        #region Floats

        internal static readonly Parser<int> DecExponentSuffix =
            from e in Parse.IgnoreCase('e')
            from num in Parse.Digit.Many().Text()
            select Convert.ToInt32(num);

        internal static readonly Parser<int> HexExponentSuffix =
            from e in Parse.IgnoreCase('p')
            from num in Parse.Digit.Many().Text()
            select Convert.ToInt32(num, 16);

        internal static readonly Parser<(string whole, string frac, int @base)> DecimalDoubleString =
            from whole in Parse.Digit.Many().Text()
            from dot in Parse.Char('.')
            from frac in Parse.Digit.Many().Text()
            select (whole, frac, 10);

        internal static readonly Parser<(string whole, string frac, int @base)> HexaecimalDoubleString =
            from prefix in Parse.Char('0').Then(_ => Parse.IgnoreCase('x'))
            from whole in HexDigit.Many().Text()
            from dot in Parse.Char('.')
            from frac in HexDigit.Many().Text()
            select (whole, frac, 16);

        internal static readonly Parser<(string whole, string frac, int @base)> BasicDoubleString =
            DecimalDoubleString.Or(HexaecimalDoubleString);
        
        internal static readonly Parser<(string whole, string frac, int exponent, int @base)> DecimalDoubleExponentString =
            from whole in Parse.Digit.Many().Text()
            from dot in Parse.Char('.')
            from frac in Parse.Digit.Many().Text()
            from exp in DecExponentSuffix 
            select (whole, frac, exp, 10);

        internal static readonly Parser<(string whole, string frac, int exponent, int @base)> HexaecimalDoubleExponentString =
            from prefix in Parse.Char('0').Then(_ => Parse.IgnoreCase('x'))
            from whole in HexDigit.Many().Text()
            from dot in Parse.Char('.')
            from frac in HexDigit.Many().Text()
            from exp in HexExponentSuffix 
            select (whole, frac, exp, 16);
        
        internal static readonly Parser<(string whole, string frac, int exponent, int @base)> ExponentDoubleString =
            DecimalDoubleExponentString.Or(HexaecimalDoubleExponentString);

        internal static readonly Parser<char> FloatSuffix = Parse.IgnoreCase('f');

        internal static readonly Parser<int> BasicDouble =
            BasicDoubleString.Select(n => Convert.ToInt32(n.whole, n.@base));

        internal static readonly Parser<int> ExponentDouble =
            ExponentDoubleString.Select(n => (int)Math.Pow(Convert.ToInt32(n.whole, n.@base), n.exponent));

        internal static readonly Parser<int> Double = BasicDouble.Or(ExponentDouble).Token();

        internal static readonly Parser<(string whole, string frac, int @base)> BasicFloatString =
            from s in BasicDoubleString
            from suffix in FloatSuffix
            select s;
        
        internal static readonly Parser<(string whole, string frac, int exponent, int @base)> ExponentFloatString =
            from s in ExponentDoubleString
            from suffix in FloatSuffix
            select s;
        
        internal static readonly Parser<int> BasicFloat =
            BasicFloatString.Select(n => Convert.ToInt32(n.whole, n.@base));

        internal static readonly Parser<int> ExponentFloat =
            ExponentFloatString.Select(n => (int)Math.Pow(Convert.ToInt32(n.whole, n.@base), n.exponent));

        internal static readonly Parser<int> Float = BasicFloat.Or(ExponentFloat);
        
        internal static readonly Parser<(string whole, string frac, int @base)> BasicLongDoubleString =
            from s in BasicDoubleString
            from suffix in LongSuffix
            select s;
            
        internal static readonly Parser<(string whole, string frac, int exponent, int @base)> ExponentLongDoubleString =
            from s in ExponentDoubleString
            from suffix in LongSuffix
            select s;
        
        internal static readonly Parser<int> BasicLongDouble =
            BasicLongDoubleString.Select(n => Convert.ToInt32(n.whole, n.@base));

        internal static readonly Parser<int> ExponentLongDouble =
            ExponentLongDoubleString.Select(n => (int)Math.Pow(Convert.ToInt32(n.whole, n.@base), n.exponent));

        internal static readonly Parser<int> LongDouble = BasicLongDouble.Or(ExponentLongDouble);

        internal static readonly Parser<int> Fractional = Float.Or(LongDouble).Or(Double).Named("Floating-point literal");
        
        #endregion

        #region Text

        #region Chars

        internal static readonly Parser<sbyte> CharSimple =
            Parse.CharExcept('\'').Select(x => Convert.ToSByte(x));

        internal static readonly Parser<sbyte> CharSimpleEscape =
            Parse.Char('\\').Then(_ => Parse.AnyChar).Select(x => x.TransformSimpleEscape());

        internal static readonly Parser<sbyte> CharOctalEscape =
            from bs in Parse.Char('\\')
            from c in OctDigit.Repeat(1, 3).Text()
            select Convert.ToSByte(c, 8);

        internal static readonly Parser<sbyte> CharHexEscape =
            from bs in Parse.Char('\\')
            from x in Parse.IgnoreCase('x')
            from c in HexDigit.Repeat(1, 2).Text()
            select Convert.ToSByte(c, 8);

        internal static readonly Parser<sbyte> CharRaw = CharSimpleEscape.Or(CharHexEscape).Or(CharOctalEscape).Or(CharSimple);

        internal static readonly Parser<sbyte> Char = CharRaw.Contained(Parse.Char('\''), Parse.Char('\'')).Token()
            .Named("Character literal");

        #endregion

        #region Strings

        internal static readonly Parser<sbyte[]> BasicString =
            CharRaw.Many().Select(x => x.ToArray()).Contained(Parse.Char('"'), Parse.Char('"'));

        internal static readonly Parser<sbyte[]> String = BasicString;

        #endregion
        
        #endregion

        internal static readonly Parser<object> NumericLiteral =
            Integer.Or(Fractional.Select(x => (object) x)).Or(Char.Select(x => (object) x)).Token();

        internal static readonly Parser<object> Literal = NumericLiteral.Or(String);
        
        #endregion


        #region Operators

        internal static readonly Parser<BinaryMemOperator> Op1Mem =
            Parse.String(".").Or(Parse.String("->")).Token().Text().Select(x => new BinaryMemOperator(x));

        internal static readonly Parser<PostfixIncDecOperator> Op1PostIncDec =
            Parse.String("++").Or(Parse.String("--")).Token().Text().Select(x => new PostfixIncDecOperator(x));

        internal static readonly Parser<PrefixIncDecOperator> Op2PreIncDec =
            Parse.String("++").Or(Parse.String("--")).Token().Text().Select(x => new PrefixIncDecOperator(x));

        internal static readonly Parser<UnaryValOperator> OpP2AddSub =
            Parse.Chars('+', '-').Token().Select(x => new UnaryValOperator(x.ToString()));

        internal static readonly Parser<UnaryValOperator> OpP2Not =
            Parse.Chars('!', '~').Token().Select(x => new UnaryValOperator(x.ToString()));

        internal static readonly Parser<UnaryMemOperator> Op2Mem =
            Parse.Chars('*', '&').Token().Select(x => new UnaryMemOperator(x.ToString()));

        internal static readonly Parser<UnaryValOperator> Op2UnaryVal = OpP2Not.Or(OpP2AddSub);

        internal static readonly Parser<UnaryOperator> Op2Unary = Op2UnaryVal.Or<UnaryOperator>(Op2PreIncDec);

        internal static readonly Parser<BinaryValOperator> OpP3TimesDivMod =
            Parse.Chars('*', '/', '%').Token().Select(x => new BinaryValOperator(x.ToString()));

        internal static readonly Parser<BinaryValOperator> OpP4AddSub =
            Parse.Chars('+', '-').Token().Select(x => new BinaryValOperator(x.ToString()));

        internal static readonly Parser<BinaryBitOperator> OpP5Shift =
            Parse.String("<<").Or(Parse.String(">>")).Token().Text().Select(x => new BinaryBitOperator(x));

        internal static readonly Parser<BinaryCmpOperator> OpP6Inequality =
            Parse.String("<=")
                .Or(Parse.String(">="))
                .Or(Parse.String(">"))
                .Or(Parse.String("<"))
                .Token().Text().Select(x => new BinaryCmpOperator(x));

        internal static readonly Parser<BinaryCmpOperator> OpP7Equality =
            Parse.String("==").Or(Parse.String("!=")).Token().Text().Select(x => new BinaryCmpOperator(x));

        internal static readonly Parser<BinaryBitOperator> OpP8BitAnd =
            Parse.Char('&').Token().Select(x => new BinaryBitOperator(x.ToString()));

        internal static readonly Parser<BinaryBitOperator> OpP9BitXor =
            Parse.Char('^').Token().Select(x => new BinaryBitOperator(x.ToString()));

        internal static readonly Parser<BinaryBitOperator> OpP10BitOr =
            Parse.Char('|').Token().Select(x => new BinaryBitOperator(x.ToString()));

        internal static readonly Parser<BinaryLogOperator> OpP11LogAnd =
            Parse.String("&&").Token().Text().Select(x => new BinaryLogOperator(x));

        internal static readonly Parser<BinaryLogOperator> OpP12LogOr =
            Parse.String("||").Token().Text().Select(x => new BinaryLogOperator(x));

        internal static readonly Parser<AssOperator> OpP14AssignmentBasic =
            Parse.Char('=').Token().Select(x => new AssOperator(x.ToString()));

        internal static readonly Parser<AssOperator> OpP14AssignmentCompound =
            (from op in Extensions.Strings("+", "-", "*", "/", "%", "|", "^", "&", "<<", ">>").Text()
                from ass in Parse.String("=").Text()
                select new AssOperator(op + ass)).Token();

        internal static readonly Parser<AssOperator> OpP14Assignment =
            OpP14AssignmentBasic.Or(OpP14AssignmentCompound).Token();

        internal static readonly Parser<BinaryValOperator> OpP15Comma =
            Parse.Char(',').Token().Select(x => new BinaryValOperator(x.ToString()));

        #endregion


        #region Expressions

        internal static readonly Parser<ValueExpression> ValueExpression =
            Literal.Select(x => new ValueExpression {ToReturn = x}).Token().Named("Value");

        internal static readonly Parser<ParExpression> ParExpression =
            (from opar in Parse.Char('(')
                from expr in Parse.Ref(() => RvalueExpression).Token()
                from cpar in Parse.Char(')')
                select new ParExpression {Expression = expr}).Token();
        
        internal static readonly Parser<VariableExpression> VarExpression =
            Identifier.Select(x => new VariableExpression {Identifier = x}).Token();

        internal static readonly Parser<RvalueExpression> Expr1P = ValueExpression.Or<RvalueExpression>(ParExpression).Or(VarExpression).Token();


        internal static readonly Parser<UnaryValExpression> Expr2PU =
            (from op in Op2UnaryVal.Token()
            from expr in Expr1P
            select new UnaryValExpression {Expression = expr, Operator = op}).Token();

        internal static readonly Parser<RvalueExpression> Expr2P = Expr2PU.Token();

        internal static readonly Parser<RvalueExpression> Expr12 = Expr2P.Or(Expr1P).Token();


        internal static readonly Parser<RvalueExpression> Expr3P =
            Parse.ChainOperator(OpP3TimesDivMod, Expr12,
                (op, lhs, rhs) => new BinaryValExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static readonly Parser<RvalueExpression> Expr13 = Expr3P.Or(Expr12).Token();


        internal static readonly Parser<RvalueExpression> Expr4P =
            Parse.ChainOperator(OpP4AddSub, Expr13,
                (op, lhs, rhs) => new BinaryValExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static readonly Parser<RvalueExpression> Expr14 = Expr4P.Or(Expr13).Token();


        internal static readonly Parser<RvalueExpression> Expr5P =
            Parse.ChainOperator(OpP5Shift, Expr14,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static readonly Parser<RvalueExpression> Expr15 = Expr5P.Or(Expr14).Token();


        internal static readonly Parser<RvalueExpression> Expr6P =
            Parse.ChainOperator(OpP6Inequality, Expr15,
                (op, lhs, rhs) => new BinaryCmpExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static readonly Parser<RvalueExpression> Expr16 = Expr6P.Or(Expr15).Token();


        internal static readonly Parser<RvalueExpression> Expr7P =
            Parse.ChainOperator(OpP7Equality, Expr16,
                (op, lhs, rhs) => new BinaryCmpExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static readonly Parser<RvalueExpression> Expr17 = Expr7P.Or(Expr16).Token();


        internal static readonly Parser<RvalueExpression> Expr8P =
            Parse.ChainOperator(OpP8BitAnd, Expr17,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static readonly Parser<RvalueExpression> Expr18 = Expr8P.Or(Expr17).Token();


        internal static readonly Parser<RvalueExpression> Expr9P =
            Parse.ChainOperator(OpP9BitXor, Expr18,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static readonly Parser<RvalueExpression> Expr19 = Expr9P.Or(Expr18).Token();


        internal static readonly Parser<RvalueExpression> Expr10P =
            Parse.ChainOperator(OpP10BitOr, Expr19,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static readonly Parser<RvalueExpression> Expr110 = Expr10P.Or(Expr19).Token();


        internal static readonly Parser<RvalueExpression> Expr11P =
            Parse.ChainOperator(OpP11LogAnd, Expr110,
                (op, lhs, rhs) => new BinaryLogExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static readonly Parser<RvalueExpression> Expr111 = Expr11P.Or(Expr110).Token();


        internal static readonly Parser<RvalueExpression> Expr12P =
            Parse.ChainOperator(OpP12LogOr, Expr111,
                (op, lhs, rhs) => new BinaryLogExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static readonly Parser<RvalueExpression> Expr112 = Expr12P.Or(Expr111).Token();


        //internal static readonly Parser<RvalueExpression> Expr13P

        internal static readonly Parser<RvalueExpression> Expr113 = Expr112.Token();

        internal static readonly Parser<RvalueExpression> Expr14PSimple =
            Parse.ChainRightOperator(OpP14AssignmentBasic, Expr113,
                (op, lhs, rhs) => new SimpleAssignmentExpression
                    {Lvalue = lhs is LvalueExpression lv ? lv : throw new Exception(), Rvalue = rhs}).Token();
        
        internal static readonly Parser<RvalueExpression> Expr14PCompound =
            Parse.ChainRightOperator(OpP14AssignmentCompound, Expr113,
                (op, lhs, rhs) => new CompoundAssignmentExpression
                    {Lvalue = lhs is LvalueExpression lv ? lv : throw new Exception(), Rvalue = rhs, Operator = op}).Token();

        internal static readonly Parser<RvalueExpression> Expr14P = Expr14PSimple.Or(Expr14PCompound).Token();
        
        internal static readonly Parser<RvalueExpression> Expr114 = Expr14P.Or(Expr113).Token();


        internal static readonly Parser<RvalueExpression> Expr15P =
            Parse.ChainOperator(OpP15Comma, Expr114,
                (op, lhs, rhs) => new BinaryValExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static readonly Parser<LvalueExpression> LvalueExpression = VarExpression.Token();
        internal static readonly Parser<RvalueExpression> RvalueExpression = Expr15P.Or(Expr114).Token();

        internal static readonly Parser<Expression> Expression = RvalueExpression.Or(LvalueExpression).Token().Named("Expression");

        #endregion
        
        
        #region Types

        internal static readonly Parser<string> BasicSimpleIntegralType =
            Parse.String(Types.Char).Or(Parse.String(Types.Int)).Or(Parse.String(Types.LongLong))
                .Or(Parse.String(Types.Long)).Or(Parse.String(Types.Short)).Token().Text();

        /*internal static readonly Parser<string> BasicComplexIntegralType =
            (from conc in Parse.String(Types.LongLong).Or(Parse.String(Types.Long)).Or(Parse.String(Types.Short))
                    .Token()
                    .Text()
                from oint in Parse.String(Types.Int).Token().Text().Optional()
                select conc).Token();*/

        internal static readonly Parser<string> BasicFloatingType =
            Parse.String(Types.Double).Or(Parse.String(Types.Float)).Or(Parse.String(Types.LongDouble))
                .Token().Text();

        internal static readonly Parser<string>
            BasicIntegralType = /*BasicComplexIntegralType.Or(*/BasicSimpleIntegralType/*)*/;

        internal static readonly Parser<string> BasicType =
            BasicIntegralType.Or(BasicFloatingType).Or(Parse.String(Types.Void))
                .Token().Text();

        /*internal static readonly Parser<string> TypeMods =
            Parse.String(TypeModifiers.Signed)
                .Or(Parse.String(TypeModifiers.Unsigned))
                .Or(Parse.String(TypeModifiers.Register))
                .Or(Parse.String(TypeModifiers.Volatile))
                .Or(Parse.String(TypeModifiers.Const))
                .Or(Parse.String(TypeModifiers.Enum))
                .Or(Parse.String(TypeModifiers.Struct))
                .Or(Parse.String(TypeModifiers.Union)).Token().Text();

        internal static readonly Parser<Type> Type =
            (from mods in TypeMods.Token().Many()
            from type in BasicType
            select new Type(type, mods)).Named("Type declaration");*/

        internal static readonly Parser<Type> Type =
            BasicType.Select(t => new Type(t)).Token();

        #endregion

     
        #region Variables

        internal static readonly Parser<VariableDeclaration> VarDeclaration =
            (from type in Type.Token()
            from iden in Identifier.Token()
            from sc in Parse.Char(';').Token()
            select new VariableDeclaration {Type = type, Identifier = iden}).Token();

        public static Parser<VariableDeclarationAss> VarDeclarationAss =
            from type in Type.Token()
            from iden in Identifier.Token()
            from eq in Parse.Char('=').Token()
            from rv in Expression.Token()
            from sc in Parse.Char(';').Token()
            select new VariableDeclarationAss {Identifier = iden, Rvalue = rv, Type = type};

        #endregion
        

        #region Statements

        internal static readonly Parser<ExpressionStatement> ExpressionStatement =
            from expr in Expression.Token()
            from scol in Parse.Char(';').Token()
            select new ExpressionStatement {Expression = expr};

        internal static readonly Parser<ReturnStatement> ReturnStatement =
            (from ret in Return.Token()
            from expr in Expression.Token()
            from scol in Parse.Char(';').Token()
            select new ReturnStatement {Expression = expr}).Named("Return statement");

        internal static readonly Parser<Statement> Statement =
            ExpressionStatement.Or<Statement>(ReturnStatement).Or(VarDeclaration).Or(VarDeclarationAss)
                .Or(Parse.Ref(() => Block)).Named("Statement");

        internal static readonly Parser<Block> Block = Statement.Token().AtLeastOnce()
            .Contained(Parse.Char('{').Token(), Parse.Char('}').Token())
            .Select(s => new Block {Statements = s.ToList()}).Named("Block");

        #endregion


        #region TopLevel

        internal static readonly Parser<FunctionDefinition> FunctionDefinition =
            from type in Type
            from name in Identifier.Token()
            from oparen in Parse.Char('(').Token()
            from cparen in Parse.Char(')').Token()
            select new FunctionDefinition {Name = name, Type = type};

        internal static readonly Parser<Function> Function =
            from def in FunctionDefinition.Token()
            from body in Block.Token()
            select new Function {Body = body, Definition = def};

        internal static readonly Parser<TopLevel> TopLevel = Function;

        internal static readonly Parser<Program> Program =
            from topl in TopLevel.XAtLeastOnce().End()
            select new Program {All = topl.ToList()};

        #endregion
    }
}