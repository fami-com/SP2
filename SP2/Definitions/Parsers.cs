using System;
using System.Linq;
using SP2.Blocks;
using Sprache;

namespace SP2.Definitions
{
    internal static class Grammar
    {
        internal static readonly Parser<string> Identifier =
            (from head in Parse.Letter.Or(Parse.Char('_')).Once().Text()
                from tail in Parse.LetterOrDigit.Or(Parse.Char('_')).Many().Text()
                select head + tail).Token();

        internal static readonly Parser<string> Return = Parse.String(Keywords.Return).Text().Token();

        internal static readonly Parser<char> Char =
            (from qo in Parse.Char('\'')
                from c in Parse.CharExcept('\'')
                from qc in Parse.Char('\'')
                select c).Token();

        internal static readonly Parser<int> DecimalInt = Parse.Number.Token().Select(x => Convert.ToInt32(x));

        internal static readonly Parser<int> BinaryInt =
            (from prefix in Parse.String("0b")
                from num in Parse.Chars('0', '1').Many().Text()
                select Convert.ToInt32(num, 2)).Token();

        internal static readonly Parser<int> Int = DecimalInt.Or(BinaryInt);

        internal static readonly Parser<BasicExpression> BasicExpression =
            Int.Select(x => new BasicExpression(x)).Or(Char.Select(x => new BasicExpression(x)));

        internal static readonly Parser<Expression> Expression = BasicExpression;

        internal static readonly Parser<ExpressionStatement> ExpressionStatement =
            from expr in Expression.Token()
            from scol in Parse.Char(';').Once()
            select new ExpressionStatement {Expression = expr};

        internal static readonly Parser<ReturnStatement> ReturnStatement =
            from ret in Return.Token()
            from expr in Expression.Token()
            from scol in Parse.Char(';').Token()
            select new ReturnStatement {Expression = expr};

        internal static readonly Parser<Statement> Statement = ExpressionStatement.Or<Statement>(ReturnStatement).Or(Parse.Ref(() => Block));

        internal static readonly Parser<Block> Block =
            from obrace in Parse.Char('{').Once().Token()
            from statements in Statement.Token().Many()
            from cbrace in Parse.Char('}').Once().Token()
            select new Block {Statements = statements.ToList()};

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
    }
}