using System;
using System.ComponentModel;
using System.Linq;
using SP2.Definitions;

namespace SP2
{
    class Token
    {
        public TokenKind Kind;
        public int Row;
        public int Column;
        public object Data;

        public Token(string input, int row, int column)
        {
            Row = row;
            Column = column - input.Length;
            (Kind,Data) = input switch
            {
                "+"=>(TokenKind.Plus,null),
                "-"=>(TokenKind.Minus,null),
                "/"=>(TokenKind.Div,null),
                "*"=>(TokenKind.Mul,null),
                "%"=>(TokenKind.Mod,null),
                "<<"=>(TokenKind.ShiftLeft,null),
                ">>"=>(TokenKind.ShiftRight,null),
                "="=>(TokenKind.Assign,null),
                "+="=>(TokenKind.AddAssign,null),
                "-="=>(TokenKind.SubAssign,null),
                "*="=>(TokenKind.MulAssign,null),
                "/="=>(TokenKind.DivAssign,null),
                "%="=>(TokenKind.ModAssign,null),
                ","=>(TokenKind.Comma,null),
                ";"=>(TokenKind.Semicolon,null),
                ":"=>(TokenKind.Colon,null),
                "|"=>(TokenKind.BitOr,null),
                "&"=>(TokenKind.BitAnd,null),
                "~"=>(TokenKind.BitNot,null),
                "&&"=>(TokenKind.LogAnd,null),
                "||"=>(TokenKind.LogOr,null),
                "!"=>(TokenKind.LogNot,null),
                "=="=>(TokenKind.Equal,null),
                "!="=>(TokenKind.NotEqual,null),
                "<"=>(TokenKind.Less,null),
                "<="=>(TokenKind.LessEqual,null),
                ">"=>(TokenKind.Greater, null),
                ">="=>(TokenKind.GreaterEqual,null),
                "("=>(TokenKind.LParen,null),
                ")"=>(TokenKind.RParen,null),
                "["=>(TokenKind.LBracket,null),
                "]"=>(TokenKind.RBracket,null),
                "{"=>(TokenKind.LBrace,null),
                "}"=>(TokenKind.RBrace,null),
                {} s when s == "int" => (TokenKind.Int,null),
                {} s when s == "return" => (TokenKind.Return,null),
                {} s when s == "if" => (TokenKind.If,null),
                {} s when s == "else" => (TokenKind.Else,null),
                {} s when s == "or" => (TokenKind.For,null),
                {} s when s.IsValidIdentifier() => (TokenKind.Identifier,input),
                {} s when int.TryParse(s, out var k) => (TokenKind.Number,(object)k),
                _ => throw new ArgumentOutOfRangeException(nameof(input), $"{string.Join(", ",input.Select(x=>Convert.ToInt32(x)))} is invalid")
            };
        }

        public override string ToString()
        {
            var t = Data is null ? "" : $"({Data})";
            return $"{Enum.GetName(typeof(TokenKind), Kind)}{t} at {Row} : {Column}";
        }
    }
}