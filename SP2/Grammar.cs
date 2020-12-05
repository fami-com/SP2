using System;
using System.Collections.Generic;
using System.Linq;
using SP2.Definitions;
using SP2.Emitters.Expressions;
using SP2.Tokens;
using SP2.Tokens.Expressions;
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
    internal class Grammar
    {
        public IComment CommentParser { get; } = new CommentParser();

        private Dictionary<int, List<string>> _symbolTable = new Dictionary<int, List<string>>();
        private int _offset;
        private int _blockId;
        private List<string> _funcs = new List<string>();
        private Dictionary<string, (int, Type)> _functionalVariables;

        private bool AddSymbol(string symbol, int blockId)
        {
            if (!_symbolTable.ContainsKey(blockId))
                _symbolTable.Add(blockId, new List<string>());
            if (_symbolTable[blockId].Contains(symbol)) return false;
            _symbolTable[blockId].Add(symbol);
            return true;
        }

        private bool HasVar(string symbol, int blockId)
        {
            if (!_symbolTable.ContainsKey(blockId)) return false;
            return !_symbolTable[blockId].Contains(symbol);
        }

        private void EnterContext(int blockId)
        {
            _symbolTable.Add(blockId, new List<string>());
        }

        private Parser<Block> _block()
        {
            var f = Statement.Many().Contained(LBrace, RBrace)
                .Select(s => new Block {Statements = s.ToList(), BlockID = 0}).Token().Named("Block")!;
            return f;
        }

        internal Parser<Block> Block => _block().Commented(CommentParser).Select(x=>x.Value);

        internal Parser<VariableDeclarationAss> _varDeclarationAss()
        {
            return Type.Token().SelectMany(type => Identifier.Token(), (type, iden) => new {type, iden})
                .SelectMany(t => Parse.Char('=').Token(), (t, eq) => new {t.type, t.iden})
                .SelectMany(t => Expression.Token(), (t, rv) => new {t.type, t.iden, rv}).SelectMany(
                    t => Semicolon, (t, sc) =>
                    {
                        if (!AddSymbol(t.iden.Name, _blockId))
                        {
                            throw new ParseException($"Redeclaration of variable {t.iden}", t.iden.StartPos);
                        }

                        return new VariableDeclarationAss {Identifier = t.iden, Rvalue = t.rv, Type = t.type};
                    }).Token();
        }

        internal Parser<VariableDeclarationAss> VarDeclarationAss => _varDeclarationAss();

        internal Parser<VariableExpression> _varExpression()
        {
            return Identifier.Select(x =>
            {
                if (!HasVar(x.Name, _blockId))
                {
                    //throw new ParseException($"Use of undeclared variable {x}", x.StartPos);
                }

                return new VariableExpression {Identifier = x};
            }).Token();
        }
        
        internal Parser<VariableExpression> VarExpression => _varExpression();

        private Parser<Function> _function()
        {
            _functionalVariables = new Dictionary<string, (int, Type)>();
            EnterContext(_blockId);
            var t = from def in FunctionDefinition.Token()
                from body in Block.Token()
                select new Function {Body = body, Definition = def, Variables = _symbolTable[_blockId]};
            return t;
        }

        internal Parser<Function> Function => _function().Token();

        internal Parser<VariableDeclaration> _varDeclaration()
        {
            return Type.Token().SelectMany(type => Identifier.Token(), (type, iden) => new {type, iden}).SelectMany(
                t => Semicolon.Token(), (t, sc) =>
                {
                    if (!AddSymbol(t.iden.Name, _blockId))
                    {
                        throw new ParseException($"Redeclaration of variable {t.iden}", t.iden.StartPos);
                    }

                    return new VariableDeclaration {Type = t.type, Identifier = t.iden};
                }).Token();
        }

        internal Parser<VariableAss> _partVass()
        {
            return Type.Token()
                .SelectMany(type => Identifier.Token(), (type, iden) => new {type, iden})
                .SelectMany(t => Parse.Char('=').Token(), (t, eq) => new {t, eq})
                .SelectMany(t => Expression.Token(),
                    (t, expr) =>
                    {
                        if (!AddSymbol(t.t.iden.Name, _blockId))
                        {
                            throw new ParseException($"Redeclaration of variable {t.t.iden}", t.t.iden.StartPos);
                        }
                        return new VariableAss {Identifier = t.t.iden.Name, Type = t.t.type, Value = expr};
                    });
        }

        internal Parser<VariableAss> PartialVariableAss => _partVass();

        internal Parser<VariableDeclaration> VarDeclaration => _varDeclaration();
        internal readonly Parser<char> Semicolon;
        internal readonly Parser<char> RBrace;
        internal readonly Parser<char> LBrace;
        internal readonly Parser<char> FirstIdentifierChar;
        internal readonly Parser<char> NextIdentifierChar;
        internal readonly Parser<string> AnyIdentifier;
        internal readonly Parser<string> Return;
        internal readonly Parser<string> If;
        internal readonly Parser<string> Else;
        internal readonly Parser<string> For;
        internal readonly Parser<string> While;
        internal readonly Parser<string> Do;
        internal readonly Parser<string> Break;
        internal readonly Parser<string> Continue;
        internal readonly Parser<string> Goto;
        internal readonly Parser<string> Switch;
        internal readonly Parser<string> Case;
        internal readonly Parser<string> Default;
        internal readonly Parser<string> Auto;
        internal readonly Parser<string> Extern;
        internal readonly Parser<string> Static;
        internal readonly Parser<string> Restrict;
        internal readonly Parser<string> Sizeof;
        internal readonly Parser<string> IntK;
        internal readonly Parser<string> ShortK;
        internal readonly Parser<string> CharK;
        internal readonly Parser<string> LongK;
        internal readonly Parser<string> FloatK;
        internal readonly Parser<string> DoubleK;
        internal readonly Parser<string> Void;
        internal readonly Parser<string> Keyword;
        internal readonly Parser<Identifier> Identifier;
        internal readonly Parser<char> HexDigit;
        internal readonly Parser<char> OctDigit;
        internal readonly Parser<char> BinDigit;
        internal readonly Parser<(string num, int b)> DecimalIntString;
        internal readonly Parser<(string num, int b)> BinaryIntString;
        internal readonly Parser<(string num, int b)> OctalIntString;
        internal readonly Parser<(string num, int b)> HexadecimalIntString;
        internal readonly Parser<(string num, int b)> UnsignedIntString;
        internal readonly Parser<(string num, int b)> SignedIntString;
        internal readonly Parser<int> Int;
        internal readonly Parser<char> UnsignedSuffix;
        internal readonly Parser<char> LongSuffix;
        internal readonly Parser<uint> UInt;
        internal readonly Parser<long> Long;
        internal readonly Parser<ulong> ULong1;
        internal readonly Parser<ulong> ULong2;
        internal readonly Parser<ulong> ULong;
        internal readonly Parser<object> Integer;
        internal readonly Parser<int> DecExponentSuffix;
        internal readonly Parser<int> HexExponentSuffix;
        internal readonly Parser<(string whole, string frac, int @base)> DecimalDoubleString;
        internal readonly Parser<(string whole, string frac, int @base)> HexaecimalDoubleString;
        internal readonly Parser<(string whole, string frac, int @base)> BasicDoubleString;
        internal readonly Parser<(string whole, string frac, int exponent, int @base)> DecimalDoubleExponentString;
        internal readonly Parser<(string whole, string frac, int exponent, int @base)> HexaecimalDoubleExponentString;
        internal readonly Parser<(string whole, string frac, int exponent, int @base)> ExponentDoubleString;
        internal readonly Parser<char> FloatSuffix;
        internal readonly Parser<int> BasicDouble;
        internal readonly Parser<int> ExponentDouble;
        internal readonly Parser<int> Double;
        internal readonly Parser<(string whole, string frac, int @base)> BasicFloatString;
        internal readonly Parser<(string whole, string frac, int exponent, int @base)> ExponentFloatString;
        internal readonly Parser<int> BasicFloat;
        internal readonly Parser<int> ExponentFloat;
        internal readonly Parser<int> Float;
        internal readonly Parser<(string whole, string frac, int @base)> BasicLongDoubleString;
        internal readonly Parser<(string whole, string frac, int exponent, int @base)> ExponentLongDoubleString;
        internal readonly Parser<int> BasicLongDouble;
        internal readonly Parser<int> ExponentLongDouble;
        internal readonly Parser<int> LongDouble;
        internal readonly Parser<int> Fractional;
        internal readonly Parser<sbyte> CharSimple;
        internal readonly Parser<sbyte> CharSimpleEscape;
        internal readonly Parser<sbyte> CharOctalEscape;
        internal readonly Parser<sbyte> CharHexEscape;
        internal readonly Parser<sbyte> CharRaw;
        internal readonly Parser<sbyte> Char;
        internal readonly Parser<sbyte[]> BasicString;
        internal readonly Parser<sbyte[]> String;
        internal readonly Parser<object> NumericLiteral;
        internal readonly Parser<object> Literal;
        //internal readonly Parser<BinaryMemOperator> Op1Mem;
        //internal readonly Parser<PostfixIncDecOperator> Op1PostIncDec;
        //internal readonly Parser<PrefixIncDecOperator> Op2PreIncDec;
        internal readonly Parser<UnaryValOperator> OpP2AddSub;
        internal readonly Parser<UnaryValOperator> OpP2Not;
        //internal readonly Parser<UnaryMemOperator> Op2Mem;
        internal readonly Parser<UnaryValOperator> Op2UnaryVal;
        //internal readonly Parser<UnaryOperator> Op2Unary;
        internal readonly Parser<BinaryValOperator> OpP3TimesDivMod;
        internal readonly Parser<BinaryValOperator> OpP4AddSub;
        internal readonly Parser<BinaryBitOperator> OpP5Shift;
        internal readonly Parser<BinaryCmpOperator> OpP6Inequality;
        internal readonly Parser<BinaryCmpOperator> OpP7Equality;
        internal readonly Parser<BinaryBitOperator> OpP8BitAnd;
        internal readonly Parser<BinaryBitOperator> OpP9BitXor;
        internal readonly Parser<BinaryBitOperator> OpP10BitOr;
        internal readonly Parser<BinaryLogOperator> OpP11LogAnd;
        internal readonly Parser<BinaryLogOperator> OpP12LogOr;
        internal readonly Parser<AssOperator> OpP14AssignmentBasic;
        internal readonly Parser<AssOperator> OpP14AssignmentCompound;
        internal readonly Parser<AssOperator> OpP14Assignment;
        //internal readonly Parser<BinaryValOperator> OpP15Comma;
        internal readonly Parser<ValueExpression> ValueExpression;
        internal readonly Parser<ParExpression> ParExpression;
        internal readonly Parser<FunctionCall> FuncCall;
        internal readonly Parser<RvalueExpression> Expr1P;
        internal readonly Parser<UnaryValExpression> Expr2PU;
        internal readonly Parser<RvalueExpression> Expr2P;
        internal readonly Parser<RvalueExpression> Expr12;
        internal readonly Parser<RvalueExpression> Expr3P;
        internal readonly Parser<RvalueExpression> Expr13;
        internal readonly Parser<RvalueExpression> Expr4P;
        internal readonly Parser<RvalueExpression> Expr14;
        internal readonly Parser<RvalueExpression> Expr5P;
        internal readonly Parser<RvalueExpression> Expr15;
        internal readonly Parser<RvalueExpression> Expr6P;
        internal readonly Parser<RvalueExpression> Expr16;
        internal readonly Parser<RvalueExpression> Expr7P;
        internal readonly Parser<RvalueExpression> Expr17;
        internal readonly Parser<RvalueExpression> Expr8P;
        internal readonly Parser<RvalueExpression> Expr18;
        internal readonly Parser<RvalueExpression> Expr9P;
        internal readonly Parser<RvalueExpression> Expr19;
        internal readonly Parser<RvalueExpression> Expr10P;
        internal readonly Parser<RvalueExpression> Expr110;
        internal readonly Parser<RvalueExpression> Expr11P;
        internal readonly Parser<RvalueExpression> Expr111;
        internal readonly Parser<RvalueExpression> Expr12P;
        internal readonly Parser<RvalueExpression> Expr112;
        internal readonly Parser<RvalueExpression> Expr113;
        internal readonly Parser<RvalueExpression> Expr14P;
        internal readonly Parser<RvalueExpression> Expr114;
        //internal readonly Parser<RvalueExpression> Expr15P;
        internal readonly Parser<LvalueExpression> LvalueExpression;
        internal readonly Parser<RvalueExpression> RvalueExpression;
        internal readonly Parser<Expression> Expression;
        internal readonly Parser<string> BasicSimpleIntegralType;
        internal readonly Parser<string> BasicFloatingType;
        internal readonly Parser<string> BasicIntegralType;
        internal readonly Parser<string> BasicType;
        internal readonly Parser<Type> Type;
        internal readonly Parser<Variable> PartialVariable;
        internal readonly Parser<BreakStatement> BreakStmt;
        internal readonly Parser<ContinueStatement> ContStmt;
        internal readonly Parser<ExpressionStatement> ExpressionStatement;
        internal readonly Parser<ReturnStatement> ReturnStatement;
        internal readonly Parser<Statement> Statement;
        internal readonly Parser<ConditionalStatement> ConditionalStatement1Branch;
        internal readonly Parser<ConditionalStatement> ConditionalStatement2Branches;
        internal readonly Parser<ConditionalStatement> ConditionalStatement;
        internal readonly Parser<ForLoopInit> ForLoopInit;
        internal readonly Parser<ForStatement> ForLoop;
        internal readonly Parser<FunctionDefinition> FunctionDefinition;
        internal readonly Parser<FunctionDeclaration> FuncDecl;
        internal readonly Parser<TopLevel> TopLevel;
        internal readonly Parser<Program> Program;

        public Grammar()
        {
            Semicolon = Parse.Char(';').Commented(CommentParser).Select(x => x.Value).Token();
            LBrace = Parse.Char('{').Commented(CommentParser).Select(x => x.Value).Token();
            RBrace = Parse.Char('}').Commented(CommentParser).Select(x => x.Value).Token();
            
            FirstIdentifierChar = Parse.Letter.Or(Parse.Char('_'));
            NextIdentifierChar = Parse.LetterOrDigit.Or(Parse.Char('_'));
            AnyIdentifier = Parse.Identifier(FirstIdentifierChar, NextIdentifierChar).Token().Named("Identifier");
            Return = Parse.String(Keywords.Return).Text().Token();
            If = Parse.String(Keywords.If).Text().Token();
            Else = Parse.String(Keywords.Else).Text().Token();
            For = Parse.String(Keywords.For).Text().Token();
            While = Parse.String(Keywords.While).Text().Token();
            Do = Parse.String(Keywords.Do).Text().Token();
            Break = Parse.String(Keywords.Break).Text().Token();
            Continue = Parse.String(Keywords.Continue).Text().Token();
            Goto = Parse.String(Keywords.Goto).Text().Token();
            Switch = Parse.String(Keywords.Switch).Text().Token();
            Case = Parse.String(Keywords.Case).Text().Token();
            Default = Parse.String(Keywords.Default).Text().Token();
            Auto = Parse.String(Keywords.Auto).Text().Token();
            Extern = Parse.String(Keywords.Extern).Text().Token();
            Static = Parse.String(Keywords.Static).Text().Token();
            Restrict = Parse.String(Keywords.Restrict).Text().Token();
            Sizeof = Parse.String(Keywords.Sizeof).Text().Token();
            IntK = Parse.String(Types.Int).Text().Token();
            ShortK = Parse.String(Types.Short).Text().Token();
            CharK = Parse.String(Types.Char).Text().Token();
            LongK = Parse.String(Types.Long).Text().Token();
            FloatK = Parse.String(Types.Float).Text().Token();
            DoubleK = Parse.String(Types.Double).Text().Token();
            Void = Parse.String(Types.Void).Text().Token();
            Keyword = Return.Or(Break).Or(Continue).Or(If).Or(Else).Or(Do).Or(While).Or(For).Or(Goto).Or(Switch)
                .Or(Case).Or(Default).Or(Auto).Or(Extern).Or(Static).Or(Restrict).Or(Sizeof).Or(IntK).Or(ShortK)
                .Or(CharK).Or(LongK).Or(FloatK).Or(DoubleK).Or(Void).Token();
            Identifier = AnyIdentifier.Except(Keyword).Token().Select(x => new Identifier {Name = x}).Positioned();
            HexDigit = Extensions.CharsIgnoreCase("0123456789abcdef");
            OctDigit = Parse.Chars("01234567");
            BinDigit = Parse.Chars("01");
            DecimalIntString = Parse.Number.Text().Select(x => (x, 10));
            BinaryIntString = from prefix in Parse.String("0b") from num in BinDigit.Many().Text() select (num, 2);
            OctalIntString = from prefix in Parse.Char('0') from num in OctDigit.Many().Text() select (num, 8);
            HexadecimalIntString = from prefix in Parse.Char('0').Then(_ => Parse.IgnoreCase('x'))
                from num in HexDigit.Many().Text()
                select (num, 16);
            UnsignedIntString = DecimalIntString.Or(BinaryIntString).Or(OctalIntString).Or(HexadecimalIntString);
            SignedIntString = from sign in Parse.Chars("+-").Optional()
                from number in UnsignedIntString
                select (sign.GetOr('+') + number.num, number.b);
            Int = SignedIntString.Select(x => Convert.ToInt32(x.num, x.b)).Token();
            UnsignedSuffix = Parse.IgnoreCase('u');
            LongSuffix = Parse.IgnoreCase('l');
            UInt = (from num in UnsignedIntString from suffix in UnsignedSuffix select Convert.ToUInt32(num.num, num.b))
                .Token();
            Long = (from num in SignedIntString
                from suffix in LongSuffix.Repeat(1, 2)
                select Convert.ToInt64(num.num, num.b)).Token();
            ULong1 = from num in SignedIntString
                from suffix1 in LongSuffix.Repeat(1, 2)
                from suffix2 in UnsignedSuffix
                select Convert.ToUInt64(num.num, num.b);
            ULong2 = from num in SignedIntString
                from suffix1 in UnsignedSuffix
                from suffix2 in LongSuffix.Repeat(1, 2)
                select Convert.ToUInt64(num.num, num.b);
            ULong = ULong1.Or(ULong2).Token();
            Integer = ULong.Select(x => (object) x).Or(UInt.Select(x => (object) x)).Or(Long.Select(x => (object) x))
                .Or(Int.Select(x => (object) x)).Named("Integer literal");
            DecExponentSuffix = from e in Parse.IgnoreCase('e')
                from num in Parse.Digit.Many().Text()
                select Convert.ToInt32(num);
            HexExponentSuffix = from e in Parse.IgnoreCase('p')
                from num in Parse.Digit.Many().Text()
                select Convert.ToInt32(num, 16);
            DecimalDoubleString = from whole in Parse.Digit.Many().Text()
                from dot in Parse.Char('.')
                from frac in Parse.Digit.Many().Text()
                select (whole, frac, 10);
            HexaecimalDoubleString = from prefix in Parse.Char('0').Then(_ => Parse.IgnoreCase('x'))
                from whole in HexDigit.Many().Text()
                from dot in Parse.Char('.')
                from frac in HexDigit.Many().Text()
                select (whole, frac, 16);
            BasicDoubleString = DecimalDoubleString.Or(HexaecimalDoubleString);
            DecimalDoubleExponentString = from whole in Parse.Digit.Many().Text()
                from dot in Parse.Char('.')
                from frac in Parse.Digit.Many().Text()
                from exp in DecExponentSuffix
                select (whole, frac, exp, 10);
            HexaecimalDoubleExponentString = from prefix in Parse.Char('0').Then(_ => Parse.IgnoreCase('x'))
                from whole in HexDigit.Many().Text()
                from dot in Parse.Char('.')
                from frac in HexDigit.Many().Text()
                from exp in HexExponentSuffix
                select (whole, frac, exp, 16);
            ExponentDoubleString = DecimalDoubleExponentString.Or(HexaecimalDoubleExponentString);
            FloatSuffix = Parse.IgnoreCase('f');
            BasicDouble = BasicDoubleString.Select(n => Convert.ToInt32(n.whole, n.@base));
            ExponentDouble =
                ExponentDoubleString.Select(n => (int) Math.Pow(Convert.ToInt32(n.whole, n.@base), n.exponent));
            Double = BasicDouble.Or(ExponentDouble).Token();
            BasicFloatString = from s in BasicDoubleString from suffix in FloatSuffix select s;
            ExponentFloatString = from s in ExponentDoubleString from suffix in FloatSuffix select s;
            BasicFloat = BasicFloatString.Select(n => Convert.ToInt32(n.whole, n.@base));
            ExponentFloat =
                ExponentFloatString.Select(n => (int) Math.Pow(Convert.ToInt32(n.whole, n.@base), n.exponent));
            Float = BasicFloat.Or(ExponentFloat);
            BasicLongDoubleString = from s in BasicDoubleString from suffix in LongSuffix select s;
            ExponentLongDoubleString = from s in ExponentDoubleString from suffix in LongSuffix select s;
            BasicLongDouble = BasicLongDoubleString.Select(n => Convert.ToInt32(n.whole, n.@base));
            ExponentLongDouble =
                ExponentLongDoubleString.Select(n => (int) Math.Pow(Convert.ToInt32(n.whole, n.@base), n.exponent));
            LongDouble = BasicLongDouble.Or(ExponentLongDouble);
            Fractional = Float.Or(LongDouble).Or(Double).Named("Floating-point literal");
            CharSimple = Parse.CharExcept('\'').Select(Convert.ToSByte);
            CharSimpleEscape = Parse.Char('\\').Then(_ => Parse.AnyChar).Select(x => x.TransformSimpleEscape());
            CharOctalEscape = from bs in Parse.Char('\\')
                from c in OctDigit.Repeat(1, 3).Text()
                select Convert.ToSByte(c, 8);
            CharHexEscape = from bs in Parse.Char('\\')
                from x in Parse.IgnoreCase('x')
                from c in HexDigit.Repeat(1, 2).Text()
                select Convert.ToSByte(c, 8);
            CharRaw = CharSimpleEscape.Or(CharHexEscape).Or(CharOctalEscape).Or(CharSimple);
            Char = CharRaw.Contained(Parse.Char('\''), Parse.Char('\'')).Token().Named("Character literal");
            BasicString = CharRaw.Many().Select(x => x.ToArray()).Contained(Parse.Char('"'), Parse.Char('"'));
            String = BasicString;
            NumericLiteral = Integer.Or(Fractional.Select(x => (object) x)).Or(Char.Select(x => (object) x)).Token();
            Literal = NumericLiteral.Or(String);
            //Op1Mem = Parse.String(".").Or(Parse.String("->")).Token().Text().Select(x => new BinaryMemOperator(x));
            /*Op1PostIncDec = Parse.String("++").Or(Parse.String("--")).Token().Text()
                .Select(x => new PostfixIncDecOperator(x));*/
            /*Op2PreIncDec = Parse.String("++").Or(Parse.String("--")).Token().Text()
                .Select(x => new PrefixIncDecOperator(x));*/
            OpP2AddSub = Parse.Chars('+', '-').Token().Select(x => new UnaryValOperator(x.ToString()));
            OpP2Not = Parse.Chars('!', '~').Token().Select(x => new UnaryValOperator(x.ToString()));
            //Op2Mem = Parse.Chars('*', '&').Token().Select(x => new UnaryMemOperator(x.ToString()));
            Op2UnaryVal = OpP2Not.Or(OpP2AddSub);
            //Op2Unary = Op2UnaryVal.Or<UnaryOperator>(Op2PreIncDec);
            OpP3TimesDivMod = Parse.Chars('*', '/', '%').Token().Select(x => new BinaryValOperator(x.ToString()));
            OpP4AddSub = Parse.Chars('+', '-').Token().Select(x => new BinaryValOperator(x.ToString()));
            OpP5Shift = Parse.String("<<").Or(Parse.String(">>")).Token().Text().Select(x => new BinaryBitOperator(x));
            OpP6Inequality = Parse.String("<=").Or(Parse.String(">=")).Or(Parse.String(">")).Or(Parse.String("<"))
                .Token().Text().Select(x => new BinaryCmpOperator(x));
            OpP7Equality = Parse.String("==").Or(Parse.String("!=")).Token().Text()
                .Select(x => new BinaryCmpOperator(x));
            OpP8BitAnd = Parse.Char('&').Token().Select(x => new BinaryBitOperator(x.ToString()));
            OpP9BitXor = Parse.Char('^').Token().Select(x => new BinaryBitOperator(x.ToString()));
            OpP10BitOr = Parse.Char('|').Token().Select(x => new BinaryBitOperator(x.ToString()));
            OpP11LogAnd = Parse.String("&&").Token().Text().Select(x => new BinaryLogOperator(x));
            OpP12LogOr = Parse.String("||").Token().Text().Select(x => new BinaryLogOperator(x));
            OpP14AssignmentBasic = Parse.Char('=').Token().Select(x => new AssOperator(x.ToString()));
            OpP14AssignmentCompound =
                (from op in Extensions.Strings("+", "-", "*", "/", "%", "|", "^", "&", "<<", ">>").Text()
                    from ass in Parse.String("=").Text()
                    select new AssOperator(op + ass)).Token();
            OpP14Assignment = OpP14AssignmentBasic.Or(OpP14AssignmentCompound).Token();
            //OpP15Comma = Parse.Char(',').Token().Select(x => new BinaryValOperator(x.ToString()));
            ValueExpression = Literal.Select(x => new ValueExpression {ToReturn = x}).Token().Named("Value");
            ParExpression = (from opar in Parse.Char('(')
                from expr in Parse.Ref(() => RvalueExpression).Token()
                from cpar in Parse.Char(')')
                select new ParExpression {Expression = expr}).Token();
            FuncCall = Identifier.Token().SelectMany(func => Parse.Char('(').Token(), (func, op) => new {func, op})
                .SelectMany(t => Parse.Ref(() => Expression).DelimitedBy(Parse.Char(',').Token()).Optional(),
                    (t, args) => new {t, args}).SelectMany(t => Parse.Char(')').Token(), (t, cp) =>
                {
                    if (!_funcs.Contains(t.t.func.Name))
                    {
                        //throw new ParseException($"Call of undeclared function {t.t.func}", t.t.func.StartPos);
                    }

                    return new FunctionCall
                    {
                        Identifier = t.t.func.Name, Arguments = t.args.GetOrOther(new List<Expression>()).ToList()
                    };
                });
            Expr1P = FuncCall.Or<RvalueExpression>(ValueExpression).Or(ParExpression).Or(VarExpression).Token();
            Expr2PU = (from op in Op2UnaryVal.Token()
                from expr in Expr1P
                select new UnaryValExpression {Expression = expr, Operator = op}).Token();
            Expr2P = Expr2PU.Token();
            Expr12 = Expr2P.Or(Expr1P).Token();
            Expr3P = Parse.ChainOperator(OpP3TimesDivMod, Expr12,
                (op, lhs, rhs) => new BinaryValExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();
            Expr13 = Expr3P.Or(Expr12).Token();
            Expr4P = Parse.ChainOperator(OpP4AddSub, Expr13,
                (op, lhs, rhs) => new BinaryValExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();
            Expr14 = Expr4P.Or(Expr13).Token();
            Expr5P = Parse.ChainOperator(OpP5Shift, Expr14,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();
            Expr15 = Expr5P.Or(Expr14).Token();
            Expr6P = Parse.ChainOperator(OpP6Inequality, Expr15,
                (op, lhs, rhs) => new BinaryCmpExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();
            Expr16 = Expr6P.Or(Expr15).Token();
            Expr7P = Parse.ChainOperator(OpP7Equality, Expr16,
                (op, lhs, rhs) => new BinaryCmpExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();
            Expr17 = Expr7P.Or(Expr16).Token();
            Expr8P = Parse.ChainOperator(OpP8BitAnd, Expr17,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();
            Expr18 = Expr8P.Or(Expr17).Token();
            Expr9P = Parse.ChainOperator(OpP9BitXor, Expr18,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();
            Expr19 = Expr9P.Or(Expr18).Token();
            Expr10P = Parse.ChainOperator(OpP10BitOr, Expr19,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();
            Expr110 = Expr10P.Or(Expr19).Token();
            Expr11P = Parse.ChainOperator(OpP11LogAnd, Expr110,
                (op, lhs, rhs) => new BinaryLogExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();
            Expr111 = Expr11P.Or(Expr110).Token();
            Expr12P = Parse.ChainOperator(OpP12LogOr, Expr111,
                (op, lhs, rhs) => new BinaryLogExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();
            Expr112 = Expr12P.Or(Expr111).Token();
// Expr13P
            Expr113 = Expr112.Token();
            Expr14P = Parse.ChainRightOperator(OpP14Assignment, Expr113,
                (op, lhs, rhs) => new AssignmentExpression
                {
                    Lvalue = lhs is LvalueExpression lv ? lv : throw new Exception(), Rvalue = rhs, Operator = op
                }).Token();
            Expr114 = Expr14P.Or(Expr113).Token();
            /*Expr15P = Parse.ChainOperator(OpP15Comma, Expr114,
                (op, lhs, rhs) => new BinaryValExpression {Lhs = lhs, Rhs = rhs, Operator = op}).Token();*/
            LvalueExpression = VarExpression.Token();
            RvalueExpression = Expr114.Token();
            Expression = RvalueExpression.Or(LvalueExpression).Token().Named("Expression");
            BasicSimpleIntegralType = Parse.String(Types.Char).Or(Parse.String(Types.Int))
                .Or(Parse.String(Types.LongLong)).Or(Parse.String(Types.Long)).Or(Parse.String(Types.Short)).Token()
                .Text();
            BasicFloatingType = Parse.String(Types.Double).Or(Parse.String(Types.Float))
                .Or(Parse.String(Types.LongDouble)).Token().Text();
            BasicIntegralType = BasicSimpleIntegralType;
            BasicType = BasicIntegralType.Or(BasicFloatingType).Or(Parse.String(Types.Void)).Token().Text();
            Type = BasicType.Select(t => new Type(t)).Token();
            PartialVariable = from type in Type.Token()
                from iden in Identifier
                select new Variable {Identifier = iden, Type = type};
//internal readonly  Parser<Variable> PartialVariable => _partialVariable();
            BreakStmt = from k in Break.Token() from s in Semicolon select new BreakStatement();
            ContStmt = from k in Break.Token() from s in Semicolon select new ContinueStatement();
            ExpressionStatement = from expr in Expression.Token()
                from scol in Semicolon
                select new ExpressionStatement {Expression = expr};
            ReturnStatement =
                (from ret in Return.Token()
                    from expr in Expression.Token()
                    from scol in Semicolon
                    select new ReturnStatement {Expression = expr}).Named("Return statement");
            Statement = ExpressionStatement.Or<Statement>(ReturnStatement).Or(VarDeclaration).Or(VarDeclarationAss)
                .Or(Parse.Ref(() => Block)).Or(Parse.Ref(() => ConditionalStatement)).Or(Parse.Ref(() => ForLoop))
                .Or(BreakStmt).Or(ContStmt).Named("Statement");
            ConditionalStatement1Branch = from @if in If
                from expr in Expression.Contained(Parse.Char('(').Token(), Parse.Char(')').Token()).Token()
                from stmt in Statement
                select new ConditionalStatement {Condition = expr, IfTrue = stmt};
            ConditionalStatement2Branches = from @if in If
                from expr in Expression.Contained(Parse.Char('(').Token(), Parse.Char(')').Token()).Token()
                from stmt in Statement
                from @else in Else
                from stmt2 in Statement
                select new ConditionalStatement {Condition = expr, IfTrue = stmt, IfFalse = stmt2};
            ConditionalStatement = ConditionalStatement2Branches.Or(ConditionalStatement1Branch)
                .Named("Conditional statement");
            ForLoopInit = Expression.Token().Select(expr => new ForLoopInit {Init2 = expr})
                .Or(PartialVariableAss.Select(x => new ForLoopInit {Init1 = x}));
            ForLoop = from kwrd in For.Token()
                from op in Parse.Char('(').Token()
                from decl in ForLoopInit.Token().Optional()
                from sc1 in Semicolon
                from cond in Expression.Token().Optional()
                from sc2 in Semicolon
                from iter in Expression.Token().Optional()
                from cp in Parse.Char(')').Token()
                from body in Statement.Token()
                select new ForStatement
                {
                    Condition = cond.GetOrElse(null),
                    Init = decl.GetOrElse(null),
                    Iteration = iter.GetOrElse(null),
                    Statement = body
                };
            FunctionDefinition = Type.Token().SelectMany(type => Identifier.Token(), (type, name) => new {type, name})
                .SelectMany(t => Parse.Char('(').Token(), (t, oparen) => new {t, oparen})
                .SelectMany(t => PartialVariable.Token().DelimitedBy(Parse.Char(',').Token()).Optional(),
                    (t, vars) => new {t, vars}).SelectMany(t => Parse.Char(')').Token(), (t, cparen) =>
                {
                    _blockId++;
                    if (_funcs.Contains(t.t.t.name.Name))
                    {
                        throw new ParseException($"Redeclaration of variable {t.t.t.name}", t.t.t.name.StartPos);
                    }

                    var duplicates = t.vars.GetOrOther(new List<Variable>()).ToList().GroupBy(i => i)
                        .Where(g => g.Count() > 1).Select(g => g.Key);
                    var enumerable = duplicates.ToList();
                    if (enumerable.Count != 0)
                    {
                        throw new ParseException($"Redeclaration of argument {enumerable[0]}",
                            enumerable[0].Identifier.StartPos);
                    }

                    return new FunctionDefinition
                    {
                        Name = t.t.t.name,
                        Type = t.t.t.type,
                        Variables = t.vars.GetOrOther(new List<Variable>()).ToList()
                    };
                });
            FuncDecl = from def in FunctionDefinition.Token()
                from sc in Semicolon
                select new FunctionDeclaration {Definition = def};
            TopLevel = FuncDecl.Or<TopLevel>(Function).Token();
            Program = from topl in TopLevel.Token().XAtLeastOnce().End() select new Program {All = topl.ToList()};
        }
    }
}