using System;
using System.Linq;
using SP2.Definitions;
using SP2.Tokens;
using SP2.Tokens.Expressions;
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
        
        internal static readonly Parser<string> Identifier =
            (from head in Parse.Letter.Or(Parse.Char('_')).Once().Text()
                from tail in Parse.LetterOrDigit.Or(Parse.Char('_')).Many().Text()
                select head + tail).Token();

        internal static readonly Parser<string> Return = Parse.String(Keywords.Return).Text().Token();
        
        #endregion
        

        #region Constants
        
        internal static readonly Parser<char> Char =
            (from qo in Parse.Char('\'')
                from c in Parse.CharExcept('\'')
                from qc in Parse.Char('\'')
                select c).Token();

        internal static readonly Parser<int> DecimalInt = Parse.Number.Text().Select(Convert.ToInt32);

        internal static readonly Parser<int> BinaryInt =
            (from prefix in Parse.String("0b")
                from num in Parse.Chars('0', '1').Many().Text()
                select Convert.ToInt32(num, 2)).Token();

        internal static readonly Parser<int> NonDecimalInt = BinaryInt;

        internal static readonly Parser<int> Int = NonDecimalInt.Or(DecimalInt);
        
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
            Parse.Chars('*', '/', '%').Token().Select(x=>new BinaryValOperator(x.ToString()));

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


        internal static readonly Parser<BinaryValOperator> OpP15Comma =
            Parse.Char(',').Token().Select(x => new BinaryValOperator(x.ToString()));

        #endregion


        #region Expressions

        internal static readonly Parser<ValueExpression> ValueExpression =
            Int.Select(x => new ValueExpression(x)).Or(Char.Select(x => new ValueExpression(x)));

        internal static readonly Parser<ParExpression> ParExpression =
            (from opar in Parse.Char('(')
                from expr in Parse.Ref(() => Expression).Token()
                from cpar in Parse.Char(')')
                select new ParExpression {Expression = expr}).Token();

        internal static readonly Parser<Expression> Expr1P = ValueExpression.Or<Expression>(ParExpression); 
        
        
        internal static readonly Parser<UnaryValExpression> Expr2PU =
            from op in Op2UnaryVal.Token()
            from expr in Expr1P
            select new UnaryValExpression {Expression = expr, Operator = op};

        internal static readonly Parser<Expression> Expr2P = Expr2PU;

        internal static readonly Parser<Expression> Expr12 = Expr2P.Or(Expr1P);


        internal static readonly Parser<Expression> Expr3P =
            Parse.ChainOperator(OpP3TimesDivMod, Expr12,
                (op, lhs, rhs) => new BinaryValExpression {Lhs = lhs, Rhs = rhs, Operator = op});

        internal static readonly Parser<Expression> Expr13 = Expr3P.Or(Expr12);

        
        internal static readonly Parser<Expression> Expr4P =
            Parse.ChainOperator(OpP4AddSub, Expr13,
                (op, lhs, rhs) => new BinaryValExpression {Lhs = lhs, Rhs = rhs, Operator = op});

        internal static readonly Parser<Expression> Expr14 = Expr4P.Or(Expr13);


        internal static readonly Parser<Expression> Expr5P =
            Parse.ChainOperator(OpP5Shift, Expr14,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op});
        
        internal static readonly Parser<Expression> Expr15 = Expr5P.Or(Expr14);


        internal static readonly Parser<Expression> Expr6P =
            Parse.ChainOperator(OpP6Inequality, Expr15,
                (op, lhs, rhs) => new BinaryCmpExpression {Lhs = lhs, Rhs = rhs, Operator = op});

        internal static readonly Parser<Expression> Expr16 = Expr6P.Or(Expr15);


        internal static readonly Parser<Expression> Expr7P =
            Parse.ChainOperator(OpP7Equality, Expr16,
                (op, lhs, rhs) => new BinaryCmpExpression {Lhs = lhs, Rhs = rhs, Operator = op});
        
        internal static readonly Parser<Expression> Expr17 = Expr7P.Or(Expr16);
        
        
        internal static readonly Parser<Expression> Expr8P =
            Parse.ChainOperator(OpP8BitAnd, Expr17,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op});
        
        internal static readonly Parser<Expression> Expr18 = Expr8P.Or(Expr17);
        
        
        internal static readonly Parser<Expression> Expr9P =
            Parse.ChainOperator(OpP9BitXor, Expr18,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op});
        
        internal static readonly Parser<Expression> Expr19 = Expr9P.Or(Expr18);
        
        
        internal static readonly Parser<Expression> Expr10P =
            Parse.ChainOperator(OpP10BitOr, Expr19,
                (op, lhs, rhs) => new BinaryBitExpression {Lhs = lhs, Rhs = rhs, Operator = op});
        
        internal static readonly Parser<Expression> Expr110 = Expr10P.Or(Expr19);
        
        
        internal static readonly Parser<Expression> Expr11P =
            Parse.ChainOperator(OpP11LogAnd, Expr110,
                (op, lhs, rhs) => new BinaryLogExpression {Lhs = lhs, Rhs = rhs, Operator = op});
        
        internal static readonly Parser<Expression> Expr111 = Expr11P.Or(Expr110);
        
        
        internal static readonly Parser<Expression> Expr12P =
            Parse.ChainOperator(OpP12LogOr, Expr111,
                (op, lhs, rhs) => new BinaryLogExpression {Lhs = lhs, Rhs = rhs, Operator = op});
        
        internal static readonly Parser<Expression> Expr112 = Expr12P.Or(Expr111);
        
        
        //internal static readonly Parser<Expression> Expr13P

        internal static readonly Parser<Expression> Expr113 = Expr112;
        
        //internal static readonly Parser<Expression> Expr14P
        
        internal static readonly Parser<Expression> Expr114 = Expr113;
        
        
        internal static readonly Parser<Expression> Expr15P =
            Parse.ChainOperator(OpP15Comma, Expr114,
                (op, lhs, rhs) => new BinaryValExpression {Lhs = lhs, Rhs = rhs, Operator = op});
        
        
        internal static readonly Parser<Expression> Expression = Expr15P.Or(Expr114);
        
        #endregion
        
        
        #region Types
        
        internal static readonly Parser<string> BasicSimpleIntegralType =
            Parse.String(Types.Char).Or(Parse.String(Types.Int)).Or(Parse.String(Types.LongLong))
                .Or(Parse.String(Types.Long)).Or(Parse.String(Types.Short)).Token().Text();

        internal static readonly Parser<string> BasicComplexIntegralType =
            (from conc in Parse.String(Types.LongLong).Or(Parse.String(Types.Long)).Or(Parse.String(Types.Short))
                    .Token()
                    .Text()
                from oint in Parse.String(Types.Int).Token().Text().Optional()
                select conc).Token();

        internal static readonly Parser<string> BasicFloatingType =
            Parse.String(Types.Double).Or(Parse.String(Types.Float)).Or(Parse.String(Types.LongDouble))
                .Token().Text();

        internal static readonly Parser<string>
            BasicIntegralType = BasicComplexIntegralType.Or(BasicSimpleIntegralType);
        
        internal static readonly Parser<string> BasicType =
            BasicIntegralType.Or(BasicFloatingType).Or(Parse.String(Types.Void))
                .Token().Text();

        internal static readonly Parser<string> TypeMods =
            Parse.String(TypeModifiers.Signed)
                .Or(Parse.String(TypeModifiers.Unsigned))
                .Or(Parse.String(TypeModifiers.Register))
                .Or(Parse.String(TypeModifiers.Volatile))
                .Or(Parse.String(TypeModifiers.Const))
                .Or(Parse.String(TypeModifiers.Enum))
                .Or(Parse.String(TypeModifiers.Struct))
                .Or(Parse.String(TypeModifiers.Union)).Token().Text();

        internal static readonly Parser<Type> Type =
            from mods in TypeMods.Token().Many()
            from type in BasicType
            select new Type(type, mods);
        
        #endregion
        
        
        #region Statements

        internal static readonly Parser<ExpressionStatement> ExpressionStatement =
            from expr in Expression.Token()
            from scol in Parse.Char(';').Once()
            select new ExpressionStatement {Expression = expr};

        internal static readonly Parser<ReturnStatement> ReturnStatement =
            from ret in Return.Token()
            from expr in Expression.Token()
            from scol in Parse.Char(';').Token()
            select new ReturnStatement {Expression = expr};

        internal static readonly Parser<Statement> Statement =
            ExpressionStatement.Or<Statement>(ReturnStatement).Or(Parse.Ref(() => Block));

        internal static readonly Parser<Block> Block =
            from obrace in Parse.Char('{').Once().Token()
            from statements in Statement.Token().Many()
            from cbrace in Parse.Char('}').Once().Token()
            select new Block {Statements = statements.ToList()};

        #endregion

        
        #region TopLevel
        
        internal static readonly Parser<FunctionDefinition> FunctionDefinition =
            from type in Identifier.Token()
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