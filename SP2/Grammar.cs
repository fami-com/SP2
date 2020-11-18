using System;
using System.Collections.Generic;
using System.Linq;
using SP2.Definitions;
using SP2.Emitters.Expressions;
using SP2.Tokens;
using SP2.Tokens.Expressions;
using SP2.Tokens.Expressions.AssignmentExpression;
using SP2.Tokens.Operators;
using SP2.Tokens.Partial;
using SP2.Tokens.Statements;
using Sprache;
using Type = SP2.Tokens.Type;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable CommentTypo

namespace SP2
{
    internal static class Grammar
    {
        private static Dictionary<int, Dictionary<string, (int offset, Type t)>> _symbolTable =
            new Dictionary<int, Dictionary<string, (int, Type)>>();

        private static Dictionary<int, int> _children = new Dictionary<int, int>();
        private static int _currentBlock;
        private static int _offset;
        private static int _blockId;
        private static Dictionary<string, (int, Type)> _functionalVariables;
        private static int BlockId => _blockId++;

        private static void EnterContext(Dictionary<string, (int, Type)> f = null)
        {
            _symbolTable.TryAdd(_blockId - 1, f ?? new Dictionary<string, (int, Type)>());
            _currentBlock = _blockId - 1;
        }

        private static void ResetTable()
        {
            _children = new Dictionary<int, int>();
            _symbolTable = new Dictionary<int, Dictionary<string, (int, Type)>>();
            _functionalVariables = null;
            _offset = 0;
        }

        #region Identifiers

        internal static Parser<char> FirstIdentifierChar = Parse.Letter.Or(Parse.Char('_'));
        internal static Parser<char> NextIdentifierChar = Parse.LetterOrDigit.Or(Parse.Char('_'));

        internal static Parser<string> AnyIdentifier =
            Parse.Identifier(FirstIdentifierChar, NextIdentifierChar).Token().Named("Identifier");

        #region Keywords

        internal static Parser<string> Return = Parse.String(Keywords.Return).Text().Token();
        internal static Parser<string> If = Parse.String(Keywords.If).Text().Token();
        internal static Parser<string> Else = Parse.String(Keywords.Else).Text().Token();
        internal static Parser<string> For = Parse.String(Keywords.For).Text().Token();
        internal static Parser<string> While = Parse.String(Keywords.While).Text().Token();
        internal static Parser<string> Do = Parse.String(Keywords.Do).Text().Token();
        internal static Parser<string> Break = Parse.String(Keywords.Break).Text().Token();
        internal static Parser<string> Continue = Parse.String(Keywords.Continue).Text().Token();
        internal static Parser<string> Goto = Parse.String(Keywords.Goto).Text().Token();
        internal static Parser<string> Switch = Parse.String(Keywords.Switch).Text().Token();
        internal static Parser<string> Case = Parse.String(Keywords.Case).Text().Token();
        internal static Parser<string> Default = Parse.String(Keywords.Default).Text().Token();
        internal static Parser<string> Auto = Parse.String(Keywords.Auto).Text().Token();
        internal static Parser<string> Extern = Parse.String(Keywords.Extern).Text().Token();
        internal static Parser<string> Static = Parse.String(Keywords.Static).Text().Token();
        internal static Parser<string> Restrict = Parse.String(Keywords.Restrict).Text().Token();
        internal static Parser<string> Sizeof = Parse.String(Keywords.Sizeof).Text().Token();
        internal static Parser<string> IntK = Parse.String(Types.Int).Text().Token();
        internal static Parser<string> ShortK = Parse.String(Types.Short).Text().Token();
        internal static Parser<string> CharK = Parse.String(Types.Char).Text().Token();
        internal static Parser<string> LongK = Parse.String(Types.Long).Text().Token();
        internal static Parser<string> FloatK = Parse.String(Types.Float).Text().Token();
        internal static Parser<string> DoubleK = Parse.String(Types.Double).Text().Token();
        internal static Parser<string> Void = Parse.String(Types.Void).Text().Token();

        internal static Parser<string> Keyword = Return.Or(Break).Or(Continue).Or(If).Or(Else).Or(Do).Or(While)
            .Or(For).Or(Goto).Or(Switch).Or(Case).Or(Default).Or(Auto).Or(Extern).Or(Static).Or(Restrict).Or(Sizeof)
            .Or(IntK).Or(ShortK).Or(CharK).Or(LongK).Or(FloatK).Or(DoubleK).Or(Void).Token();

        #endregion

        internal static Parser<string> Identifier = AnyIdentifier.Except(Keyword).Token();

        #endregion


        #region Constants

        internal static Parser<char> HexDigit = Extensions.CharsIgnoreCase("0123456789abcdef");
        internal static Parser<char> OctDigit = Parse.Chars("01234567");
        internal static Parser<char> BinDigit = Parse.Chars("01");

        #region Integers

        internal static Parser<(string num, int b)>
            DecimalIntString = Parse.Number.Text().Select(x => (x, 10));

        internal static Parser<(string num, int b)> BinaryIntString =
            from prefix in Parse.String("0b")
            from num in BinDigit.Many().Text()
            select (num, 2);

        internal static Parser<(string num, int b)> OctalIntString =
            from prefix in Parse.Char('0')
            from num in OctDigit.Many().Text()
            select (num, 8);

        internal static Parser<(string num, int b)> HexadecimalIntString =
            from prefix in Parse.Char('0').Then(_ => Parse.IgnoreCase('x'))
            from num in HexDigit.Many().Text()
            select (num, 16);

        internal static Parser<(string num, int b)> UnsignedIntString =
            DecimalIntString.Or(BinaryIntString).Or(OctalIntString).Or(HexadecimalIntString);

        internal static Parser<(string num, int b)> SignedIntString =
            from sign in Parse.Chars("+-").Optional()
            from number in UnsignedIntString
            select (sign.GetOr('+') + number.num, number.b);

        internal static Parser<int> Int = SignedIntString.Select(x => Convert.ToInt32(x.num, x.b)).Token();

        internal static Parser<char> UnsignedSuffix = Parse.IgnoreCase('u');

        internal static Parser<char> LongSuffix = Parse.IgnoreCase('l');

        internal static Parser<uint> UInt =
            (from num in UnsignedIntString
                from suffix in UnsignedSuffix
                select Convert.ToUInt32(num.num, num.b)).Token();

        internal static Parser<long> Long =
            (from num in SignedIntString
                from suffix in LongSuffix.Repeat(1, 2)
                select Convert.ToInt64(num.num, num.b)).Token();

        internal static Parser<ulong> ULong1 =
            from num in SignedIntString
            from suffix1 in LongSuffix.Repeat(1, 2)
            from suffix2 in UnsignedSuffix
            select Convert.ToUInt64(num.num, num.b);

        internal static Parser<ulong> ULong2 =
            from num in SignedIntString
            from suffix1 in UnsignedSuffix
            from suffix2 in LongSuffix.Repeat(1, 2)
            select Convert.ToUInt64(num.num, num.b);

        internal static Parser<ulong> ULong = ULong1.Or(ULong2).Token();

        internal static Parser<object> Integer =
            ULong.Select(x => (object) x).Or(UInt.Select(x => (object) x))
                .Or(Long.Select(x => (object) x)).Or(Int.Select(x => (object) x)).Named("Integer literal");

        #endregion

        #region Floats

        internal static Parser<int> DecExponentSuffix =
            from e in Parse.IgnoreCase('e')
            from num in Parse.Digit.Many().Text()
            select Convert.ToInt32(num);

        internal static Parser<int> HexExponentSuffix =
            from e in Parse.IgnoreCase('p')
            from num in Parse.Digit.Many().Text()
            select Convert.ToInt32(num, 16);

        internal static Parser<(string whole, string frac, int @base)> DecimalDoubleString =
            from whole in Parse.Digit.Many().Text()
            from dot in Parse.Char('.')
            from frac in Parse.Digit.Many().Text()
            select (whole, frac, 10);

        internal static Parser<(string whole, string frac, int @base)> HexaecimalDoubleString =
            from prefix in Parse.Char('0').Then(_ => Parse.IgnoreCase('x'))
            from whole in HexDigit.Many().Text()
            from dot in Parse.Char('.')
            from frac in HexDigit.Many().Text()
            select (whole, frac, 16);

        internal static Parser<(string whole, string frac, int @base)> BasicDoubleString =
            DecimalDoubleString.Or(HexaecimalDoubleString);

        internal static Parser<(string whole, string frac, int exponent, int @base)>
            DecimalDoubleExponentString =
                from whole in Parse.Digit.Many().Text()
                from dot in Parse.Char('.')
                from frac in Parse.Digit.Many().Text()
                from exp in DecExponentSuffix
                select (whole, frac, exp, 10);

        internal static Parser<(string whole, string frac, int exponent, int @base)>
            HexaecimalDoubleExponentString =
                from prefix in Parse.Char('0').Then(_ => Parse.IgnoreCase('x'))
                from whole in HexDigit.Many().Text()
                from dot in Parse.Char('.')
                from frac in HexDigit.Many().Text()
                from exp in HexExponentSuffix
                select (whole, frac, exp, 16);

        internal static Parser<(string whole, string frac, int exponent, int @base)> ExponentDoubleString =
            DecimalDoubleExponentString.Or(HexaecimalDoubleExponentString);

        internal static Parser<char> FloatSuffix = Parse.IgnoreCase('f');

        internal static Parser<int> BasicDouble =
            BasicDoubleString.Select(n => Convert.ToInt32(n.whole, n.@base));

        internal static Parser<int> ExponentDouble =
            ExponentDoubleString.Select(n => (int) Math.Pow(Convert.ToInt32(n.whole, n.@base), n.exponent));

        internal static Parser<int> Double = BasicDouble.Or(ExponentDouble).Token();

        internal static Parser<(string whole, string frac, int @base)> BasicFloatString =
            from s in BasicDoubleString
            from suffix in FloatSuffix
            select s;

        internal static Parser<(string whole, string frac, int exponent, int @base)> ExponentFloatString =
            from s in ExponentDoubleString
            from suffix in FloatSuffix
            select s;

        internal static Parser<int> BasicFloat =
            BasicFloatString.Select(n => Convert.ToInt32(n.whole, n.@base));

        internal static Parser<int> ExponentFloat =
            ExponentFloatString.Select(n => (int) Math.Pow(Convert.ToInt32(n.whole, n.@base), n.exponent));

        internal static Parser<int> Float = BasicFloat.Or(ExponentFloat);

        internal static Parser<(string whole, string frac, int @base)> BasicLongDoubleString =
            from s in BasicDoubleString
            from suffix in LongSuffix
            select s;

        internal static Parser<(string whole, string frac, int exponent, int @base)> ExponentLongDoubleString =
            from s in ExponentDoubleString
            from suffix in LongSuffix
            select s;

        internal static Parser<int> BasicLongDouble =
            BasicLongDoubleString.Select(n => Convert.ToInt32(n.whole, n.@base));

        internal static Parser<int> ExponentLongDouble =
            ExponentLongDoubleString.Select(n => (int) Math.Pow(Convert.ToInt32(n.whole, n.@base), n.exponent));

        internal static Parser<int> LongDouble = BasicLongDouble.Or(ExponentLongDouble);

        internal static Parser<int> Fractional =
            Float.Or(LongDouble).Or(Double).Named("Floating-point literal");

        #endregion

        #region Text

        #region Chars

        internal static Parser<sbyte> CharSimple =
            Parse.CharExcept('\'').Select(Convert.ToSByte);

        internal static Parser<sbyte> CharSimpleEscape =
            Parse.Char('\\').Then(_ => Parse.AnyChar).Select(x => x.TransformSimpleEscape());

        internal static Parser<sbyte> CharOctalEscape =
            from bs in Parse.Char('\\')
            from c in OctDigit.Repeat(1, 3).Text()
            select Convert.ToSByte(c, 8);

        internal static Parser<sbyte> CharHexEscape =
            from bs in Parse.Char('\\')
            from x in Parse.IgnoreCase('x')
            from c in HexDigit.Repeat(1, 2).Text()
            select Convert.ToSByte(c, 8);

        internal static Parser<sbyte> CharRaw =
            CharSimpleEscape.Or(CharHexEscape).Or(CharOctalEscape).Or(CharSimple);

        internal static Parser<sbyte> Char = CharRaw.Contained(Parse.Char('\''), Parse.Char('\'')).Token()
            .Named("Character literal");

        #endregion

        #region Strings

        internal static Parser<sbyte[]> BasicString =
            CharRaw.Many().Select(x => x.ToArray()).Contained(Parse.Char('"'), Parse.Char('"'));

        internal static Parser<sbyte[]> String = BasicString;

        #endregion

        #endregion

        internal static Parser<object> NumericLiteral =
            Integer.Or(Fractional.Select(x => (object) x)).Or(Char.Select(x => (object) x)).Token();

        internal static Parser<object> Literal => NumericLiteral.Or(String);

        #endregion


        #region Operators

        internal static Parser<BinaryMemOperator> Op1Mem =>
            Parse.String(".").Or(Parse.String("->")).Token().Text().Select(x => new BinaryMemOperator(x));

        internal static Parser<PostfixIncDecOperator> Op1PostIncDec =>
            Parse.String("++").Or(Parse.String("--")).Token().Text().Select(x => new PostfixIncDecOperator(x));

        internal static Parser<PrefixIncDecOperator> Op2PreIncDec =>
            Parse.String("++").Or(Parse.String("--")).Token().Text().Select(x => new PrefixIncDecOperator(x));

        internal static Parser<UnaryValOperator> OpP2AddSub =>
            Parse.Chars('+', '-').Token().Select(x => new UnaryValOperator(x.ToString()));

        internal static Parser<UnaryValOperator> OpP2Not =>
            Parse.Chars('!', '~').Token().Select(x => new UnaryValOperator(x.ToString()));

        internal static Parser<UnaryMemOperator> Op2Mem =>
            Parse.Chars('*', '&').Token().Select(x => new UnaryMemOperator(x.ToString()));

        internal static Parser<UnaryValOperator> Op2UnaryVal => OpP2Not.Or(OpP2AddSub);

        internal static Parser<UnaryOperator> Op2Unary => Op2UnaryVal.Or<UnaryOperator>(Op2PreIncDec);

        internal static Parser<BinaryValOperator> OpP3TimesDivMod =>
            Parse.Chars('*', '/', '%').Token().Select(x => new BinaryValOperator(x.ToString()));

        internal static Parser<BinaryValOperator> OpP4AddSub =>
            Parse.Chars('+', '-').Token().Select(x => new BinaryValOperator(x.ToString()));

        internal static Parser<BinaryBitOperator> OpP5Shift =>
            Parse.String("<<").Or(Parse.String(">>")).Token().Text().Select(x => new BinaryBitOperator(x));

        internal static Parser<BinaryCmpOperator> OpP6Inequality =>
            Parse.String("<=")
                .Or(Parse.String(">="))
                .Or(Parse.String(">"))
                .Or(Parse.String("<"))
                .Token().Text().Select(x => new BinaryCmpOperator(x));

        internal static Parser<BinaryCmpOperator> OpP7Equality =>
            Parse.String("==").Or(Parse.String("!=")).Token().Text().Select(x => new BinaryCmpOperator(x));

        internal static Parser<BinaryBitOperator> OpP8BitAnd =>
            Parse.Char('&').Token().Select(x => new BinaryBitOperator(x.ToString()));

        internal static Parser<BinaryBitOperator> OpP9BitXor =>
            Parse.Char('^').Token().Select(x => new BinaryBitOperator(x.ToString()));

        internal static Parser<BinaryBitOperator> OpP10BitOr =>
            Parse.Char('|').Token().Select(x => new BinaryBitOperator(x.ToString()));

        internal static Parser<BinaryLogOperator> OpP11LogAnd =>
            Parse.String("&&").Token().Text().Select(x => new BinaryLogOperator(x));

        internal static Parser<BinaryLogOperator> OpP12LogOr =>
            Parse.String("||").Token().Text().Select(x => new BinaryLogOperator(x));

        internal static Parser<AssOperator> OpP14AssignmentBasic =>
            Parse.Char('=').Token().Select(x => new AssOperator(x.ToString()));

        internal static Parser<AssOperator> OpP14AssignmentCompound =>
            (from op in Extensions.Strings("+", "-", "*", "/", "%", "|", "^", "&", "<<", ">>").Text()
                from ass in Parse.String("=").Text()
                select new AssOperator(op + ass)).Token();

        internal static Parser<AssOperator> OpP14Assignment =>
            OpP14AssignmentBasic.Or(OpP14AssignmentCompound).Token();

        internal static Parser<BinaryValOperator> OpP15Comma =>
            Parse.Char(',').Token().Select(x => new BinaryValOperator(x.ToString()));

        #endregion


        #region Expressions

        internal static Parser<ValueExpression> ValueExpression =>
            Literal.Select(x => new ValueExpression {ToReturn = x}).Token().Named("Value");

        internal static Parser<ParExpression> ParExpression =>
            (from opar in Parse.Char('(')
                from expr in Parse.Ref(() => RvalueExpression).Token()
                from cpar in Parse.Char(')')
                select new ParExpression {Expression = expr}).Token();

        internal static Parser<VariableExpression> _varExpression()
        {
            return Identifier.Select(x =>
            {
                var tmp = new List<int>();
                var c = _currentBlock;
                while (_children.ContainsKey(c))
                {
                    tmp.Add(c);
                    c = _children[c];
                }

                var k = tmp.Any(t => _symbolTable[t].ContainsKey(x));
                //if (!k) throw new Exception($"Use of undefined variable {x}");
                return new VariableExpression {Identifier = x};
            }).Token();
        }

        internal static Parser<VariableExpression> VarExpression => _varExpression();

        internal static Parser<FunctionCall> FuncCall =>
            from func in Identifier.Token()
            from op in Parse.Char('(').Token()
            from args in Parse.Ref(() => Expression).DelimitedBy(Parse.Char(',').Token()).Optional()
            from cp in Parse.Char(')').Token()
            select new FunctionCall {Identifier = func, Arguments = args.GetOrOther(new List<Expression>()).ToList()};

        internal static Parser<RvalueExpression> Expr1P = FuncCall.Or<RvalueExpression>(ValueExpression).Or(ParExpression)
            .Or(VarExpression).Token();


        internal static Parser<UnaryValExpression> Expr2PU =>
            (from op in Op2UnaryVal.Token()
                from expr in Expr1P
                select new UnaryValExpression {Expression = expr, Operator = op}).Token();

        internal static Parser<RvalueExpression> Expr2P => Expr2PU.Token();

        internal static Parser<RvalueExpression> Expr12 => Expr2P.Or(Expr1P).Token();


        internal static Parser<RvalueExpression> Expr3P =>
            Parse.ChainOperator(OpP3TimesDivMod, Expr12,
                (op, lhs, rhs) => new BinaryValExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static Parser<RvalueExpression> Expr13 => Expr3P.Or(Expr12).Token();


        internal static Parser<RvalueExpression> Expr4P =>
            Parse.ChainOperator(OpP4AddSub, Expr13,
                (op, lhs, rhs) => new BinaryValExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static Parser<RvalueExpression> Expr14 => Expr4P.Or(Expr13).Token();


        internal static Parser<RvalueExpression> Expr5P =>
            Parse.ChainOperator(OpP5Shift, Expr14,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static Parser<RvalueExpression> Expr15 => Expr5P.Or(Expr14).Token();


        internal static Parser<RvalueExpression> Expr6P =>
            Parse.ChainOperator(OpP6Inequality, Expr15,
                (op, lhs, rhs) => new BinaryCmpExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static Parser<RvalueExpression> Expr16 => Expr6P.Or(Expr15).Token();


        internal static Parser<RvalueExpression> Expr7P =>
            Parse.ChainOperator(OpP7Equality, Expr16,
                (op, lhs, rhs) => new BinaryCmpExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static Parser<RvalueExpression> Expr17 => Expr7P.Or(Expr16).Token();


        internal static Parser<RvalueExpression> Expr8P =>
            Parse.ChainOperator(OpP8BitAnd, Expr17,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static Parser<RvalueExpression> Expr18 => Expr8P.Or(Expr17).Token();


        internal static Parser<RvalueExpression> Expr9P =>
            Parse.ChainOperator(OpP9BitXor, Expr18,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static Parser<RvalueExpression> Expr19 => Expr9P.Or(Expr18).Token();


        internal static Parser<RvalueExpression> Expr10P =>
            Parse.ChainOperator(OpP10BitOr, Expr19,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static Parser<RvalueExpression> Expr110 => Expr10P.Or(Expr19).Token();


        internal static Parser<RvalueExpression> Expr11P =>
            Parse.ChainOperator(OpP11LogAnd, Expr110,
                (op, lhs, rhs) => new BinaryLogExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static Parser<RvalueExpression> Expr111 => Expr11P.Or(Expr110).Token();


        internal static Parser<RvalueExpression> Expr12P =>
            Parse.ChainOperator(OpP12LogOr, Expr111,
                (op, lhs, rhs) => new BinaryLogExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static Parser<RvalueExpression> Expr112 => Expr12P.Or(Expr111).Token();


        //internal static readonly Parser<RvalueExpression> Expr13P

        internal static Parser<RvalueExpression> Expr113 => Expr112.Token();

        internal static Parser<RvalueExpression> Expr14PSimple =>
            Parse.ChainRightOperator(OpP14AssignmentBasic, Expr113,
                (op, lhs, rhs) => new SimpleAssignmentExpression
                    {Lvalue = lhs is LvalueExpression lv ? lv : throw new Exception(), Rvalue = rhs}).Token();

        internal static Parser<RvalueExpression> Expr14PCompound =>
            Parse.ChainRightOperator(OpP14AssignmentCompound, Expr113,
                    (op, lhs, rhs) => new CompoundAssignmentExpression
                        {Lvalue = lhs is LvalueExpression lv ? lv : throw new Exception(), Rvalue = rhs, Operator = op})
                .Token();

        internal static Parser<RvalueExpression> Expr14P => Expr14PCompound.Or(Expr14PSimple).Token();

        internal static Parser<RvalueExpression> Expr114 => Expr14P.Or(Expr113).Token();


        internal static Parser<RvalueExpression> Expr15P =>
            Parse.ChainOperator(OpP15Comma, Expr114,
                (op, lhs, rhs) => new BinaryValExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();

        internal static Parser<LvalueExpression> LvalueExpression => VarExpression.Token();
        internal static Parser<RvalueExpression> RvalueExpression => Expr114.Token();

        internal static Parser<Expression> Expression =>
            RvalueExpression.Or(LvalueExpression).Token().Named("Expression");

        #endregion


        #region Types

        internal static Parser<string> BasicSimpleIntegralType =>
            Parse.String(Types.Char).Or(Parse.String(Types.Int)).Or(Parse.String(Types.LongLong))
                .Or(Parse.String(Types.Long)).Or(Parse.String(Types.Short)).Token().Text();

        /*internal static readonly Parser<string> BasicComplexIntegralType =>
            (from conc in Parse.String(Types.LongLong).Or(Parse.String(Types.Long)).Or(Parse.String(Types.Short))
                    .Token()
                    .Text()
                from oint in Parse.String(Types.Int).Token().Text().Optional()
                select conc).Token();*/

        internal static Parser<string> BasicFloatingType =>
            Parse.String(Types.Double).Or(Parse.String(Types.Float)).Or(Parse.String(Types.LongDouble))
                .Token().Text();

        internal static Parser<string>
            BasicIntegralType => /*BasicComplexIntegralType.Or(*/BasicSimpleIntegralType /*)*/;

        internal static Parser<string> BasicType =>
            BasicIntegralType.Or(BasicFloatingType).Or(Parse.String(Types.Void))
                .Token().Text();

        /*internal static readonly Parser<string> TypeMods =>
            Parse.String(TypeModifiers.Signed)
                .Or(Parse.String(TypeModifiers.Unsigned))
                .Or(Parse.String(TypeModifiers.Register))
                .Or(Parse.String(TypeModifiers.Volatile))
                .Or(Parse.String(TypeModifiers.Const))
                .Or(Parse.String(TypeModifiers.Enum))
                .Or(Parse.String(TypeModifiers.Struct))
                .Or(Parse.String(TypeModifiers.Union)).Token().Text();

        internal static readonly Parser<Type> Type =>
            (from mods in TypeMods.Token().Many()
            from type in BasicType
            select new Type(type, mods)).Named("Type declaration");*/

        internal static Parser<Type> Type =>
            BasicType.Select(t => new Type(t)).Token();

        #endregion


        #region Variables

        internal static Parser<VariableDeclaration> _varDeclaration()
        {
            return Type.Token()
                .SelectMany(type => Identifier.Token(), (type, iden) => new {type, iden})
                .SelectMany(t => Parse.Char(';').Token(),
                    (t, sc) =>
                    {
                        _offset -= t.type.Size;
                        if (!_symbolTable[_blockId - 1].TryAdd(t.iden, (_offset, t.type)))
                        {
                            _offset += t.type.Size;
                        }

                        return new VariableDeclaration {Type = t.type, Identifier = t.iden};
                    }).Token();
        }

        internal static Parser<VariableDeclaration> VarDeclaration => _varDeclaration();

        internal static Parser<Variable> PartialVariable =>
            from type in Type.Token()
            from iden in Identifier
            select new Variable {Identifier = iden, Type = type};

        //internal static Parser<Variable> PartialVariable => _partialVariable();

        internal static Parser<VariableDeclarationAss> _varDeclarationAss()
        {
            return Type.Token()
                .SelectMany(type => Identifier.Token(), (type, iden) => new {type, iden})
                .SelectMany(t => Parse.Char('=').Token(), (t, eq) => new {t.type, t.iden})
                .SelectMany(t => Expression.Token(), (t, rv) => new {t.type, t.iden, rv})
                .SelectMany(t => Parse.Char(';').Token(),
                    (t, sc) =>
                    {
                        _offset -= t.type.Size;
                        if (!_symbolTable[_blockId - 1].TryAdd(t.iden, (_offset, t.type)))
                        {
                            _offset += t.type.Size;
                        }

                        return new VariableDeclarationAss
                        {
                            Identifier = t.iden, Rvalue = t.rv, Type = t.type
                        };
                    }).Token();
        }

        internal static Parser<VariableDeclarationAss> VarDeclarationAss => _varDeclarationAss();

        #endregion


        #region Statements

        internal static Parser<ExpressionStatement> ExpressionStatement =>
            from expr in Expression.Token()
            from scol in Parse.Char(';').Token()
            select new ExpressionStatement {Expression = expr};

        internal static Parser<ReturnStatement> ReturnStatement =>
            (from ret in Return.Token()
                from expr in Expression.Token()
                from scol in Parse.Char(';').Token()
                select new ReturnStatement {Expression = expr}).Named("Return statement");

        internal static Parser<Statement> Statement =>
            ExpressionStatement.Or<Statement>(ReturnStatement).Or(VarDeclaration).Or(VarDeclarationAss)
                .Or(Parse.Ref(() => Block)).Or(Parse.Ref(() => ConditionalStatement)).Named("Statement");

        private static Parser<Block> _block()
        {
            var x = BlockId;
            _children.Add(x, _currentBlock);
            var f = Statement.Many()
                .Contained(Parse.Char('{').Token(), Parse.Char('}').Token())
                .Select(s => new Block
                    {Statements = s.ToList(), BlockID = x}).Token()
                .Named("Block")!;
            EnterContext();
            return f;
        }

        internal static Parser<Block> Block => _block();

        internal static Parser<ConditionalStatement> ConditionalStatement1Branch =>
            from @if in If
            from expr in Expression.Contained(Parse.Char('(').Token(), Parse.Char(')').Token()).Token()
            from stmt in Statement
            select new ConditionalStatement {Condition = expr, IfTrue = stmt};

        internal static Parser<ConditionalStatement> ConditionalStatement2Branches =>
            from @if in If
            from expr in Expression.Contained(Parse.Char('(').Token(), Parse.Char(')').Token()).Token()
            from stmt in Statement
            from @else in Else
            from stmt2 in Statement
            select new ConditionalStatement {Condition = expr, IfTrue = stmt, IfFalse = stmt2};

        internal static Parser<ConditionalStatement> ConditionalStatement =>
            ConditionalStatement2Branches.Or(ConditionalStatement1Branch).Named("Conditional statement");

        #endregion


        #region TopLevel

        internal static Parser<FunctionDefinition> FunctionDefinition =
            from type in Type.Token()
            from name in Identifier.Token()
            from oparen in Parse.Char('(').Token()
            from vars in PartialVariable.Token().DelimitedBy(Parse.Char(',').Token()).Optional()
            from cparen in Parse.Char(')').Token()
            select new FunctionDefinition
            {
                Name = name, Type = type, Variables = vars.GetOrOther(new List<Variable>()).ToList()
            };

        private static Parser<Function> _function()
        {
            _functionalVariables = new Dictionary<string, (int, Type)>();
            var x = BlockId;
            EnterContext(_functionalVariables);
            var t =
                from def in FunctionDefinition.Token()
                from body in Block.Token()
                select new Function {Body = body, Definition = def, SymbolTable = _symbolTable, Associations = _children};
            ResetTable();
            return t;
        }

        internal static Parser<Function> Function => _function().Token();

        internal static readonly Parser<FunctionDeclaration> FuncDecl =
            from def in FunctionDefinition.Token()
            from sc in Parse.Char(';').Token()
            select new FunctionDeclaration {Definition = def};
        
        internal static Parser<TopLevel> TopLevel => FuncDecl.Or<TopLevel>(Function).Token();

        internal static Parser<Program> Program =>
            from topl in TopLevel.Token().XAtLeastOnce().End()
            select new Program {All = topl.ToList()};

        #endregion
    }
}