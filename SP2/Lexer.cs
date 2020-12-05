using System.Collections.Generic;
using System.Linq;
using SP2.Definitions;

namespace SP2
{
    class Lexer
    {
        private string _input;
        private int _row;
        private int _column;
        public List<Token> Tokens;

        public Lexer(string input)
        {
            _input = input;
            _row = 1;
            _column = 1;
            Tokens = new List<Token>();
        }

        public void Tokenize()
        {
            var token = "";
            bool? commentMultiline = null;
            for (var i = 0; i < _input.Length; i++)
            {
                var current = _input[i].ToString(); 
                var peek = _input.Length > i + 1 ? _input[i + 1].ToString() : "";

                _column++;

                if (current == "\n")
                {
                    _row++;
                    _column = 1;
                    if (commentMultiline == false)
                    {
                        commentMultiline = null;
                    }
                }

                if (current == "\n" || current == "\t" || current == " " || current == "\r")
                {
                    token = "";
                    continue;
                }
                

                token += current;
                if (current == "/" && peek == "/" ||current == "/" && peek == "*" ) continue;
                if (current == "*" && peek == "/")
                {
                    commentMultiline = null;
                }
                if (token == "//")
                {
                    commentMultiline = false;
                }
                
                if (commentMultiline is {}) continue;
                
                if (new[] {"+", "-", "*", "/", "%", "<", ">", "!", "="}.Contains(current) && peek == "=") continue;
                if (current == peek && (peek == "|" || peek == "&")) continue;
                if (token.IsValidIdentifier() && peek[0].IsValidIdentifierCont()) continue;
                if (token.IsNumeric() && peek.IsNumeric()) continue;

                Tokens.Add(new Token(token, _row, _column));
                token = "";
            }
        }
    }
}