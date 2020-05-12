using System.Collections.Generic;
using Gatsby.Analysis.Syntax.Parser;

namespace Gatsby.Analysis.Syntax.Lexer
{
    internal sealed class Lexer
    {
        private readonly string _text;
        private int _position;
        private readonly List<string> _diagnostics = new List<string>();

        public Lexer(string text)
        {
            _text = text;
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private char Current => Peek(0);

        private char Ahead => Peek(1);

        private char Peek(int offset)
        {
            var index = _position + offset;
            return index >= _text.Length ? '\0' : _text[index];
        }

        private void Next()
        {
            _position++;
        }

        //Break input into corresponding lexemes
        public SyntaxToken Lex()
        {
            //End of File 
            if (_position >= _text.Length)
                return new SyntaxToken(TokenType.EndOfFile, _position, "\0", null);

            //Integer
            if (char.IsDigit(Current))
            {
                var start = _position;

                while (char.IsDigit(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                if (!int.TryParse(text, out var value))
                    _diagnostics.Add($"The number {_text} isn't INT32");

                return new SyntaxToken(TokenType.Number, start, text, value);
            }

            //Whitespace
            if (char.IsWhiteSpace(Current))
            {
                var start = _position;

                while (char.IsWhiteSpace(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                return new SyntaxToken(TokenType.Whitespace, start, text, null);
            }
            
            //True-False and catch-all identifiers
            if (char.IsLetter(Current))
            {
                var start = _position;

                while (char.IsLetter(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                var kind = ParserRules.GetKeywordKind(text);
                return new SyntaxToken(kind, start, text, null);
            }

            //TODO refactor this for readability & scalability
            //Operators - Arithmetic & Binary
            switch (Current)
            {
                //Arithmetic Operators
                case '+':
                    return new SyntaxToken(TokenType.Plus, _position++, "+", null);
                
                case '-':
                    return new SyntaxToken(TokenType.Minus, _position++, "-", null);
                
                case '*':
                    return new SyntaxToken(TokenType.Star, _position++, "*", null);
                
                case '/':
                    return new SyntaxToken(TokenType.Slash, _position++, "/", null);
                
                case '\\':
                    return new SyntaxToken(TokenType.ReverseSlash, _position++, "/", null);

                case '%':
                    return new SyntaxToken(TokenType.Modulo, _position++, "%", null);
                
                case '^':
                    return new SyntaxToken(TokenType.BitwiseXor, _position++, "^", null);
                
                case '#':
                    return new SyntaxToken(TokenType.Power, _position++, "^", null);

                //Boolean Operators

                case '!':
                {
                    if (Ahead != '=')
                        return new SyntaxToken(TokenType.Negation, _position, "!", null);

                    if (Ahead == '=')
                        return new SyntaxToken(TokenType.NotEqualsTo, _position += 2, "!=", null);

                    break;
                }

                case '&':
                {
                    return Ahead switch
                    {
                        '&' => new SyntaxToken(TokenType.LogicalAnd, _position += 2, "&&", null),
                        _ => new SyntaxToken(TokenType.BitwiseAnd, _position++, "&", null)
                    };
                }

                case '|':
                {
                    return Ahead switch
                    {
                        '|' => new SyntaxToken(TokenType.LogicalOr, _position += 2, "||", null),
                        _ => new SyntaxToken(TokenType.BitwiseOr, _position++, "&", null)
                    };
                }
                
                case '=':
                {
                    if (Ahead == '=')
                        return new SyntaxToken(TokenType.EqualsTo, _position += 2, "==",null);

                    //We haven't implemented the assignment operator
                    break;
                }

                case '>':
                {
                    return Ahead switch
                    {
                        '=' => new SyntaxToken(TokenType.GreaterThanEquals, _position += 2, ">=", null),
                        '>' => new SyntaxToken(TokenType.RightShift, _position += 2, ">>", null),
                        _ => new SyntaxToken(TokenType.GreaterThan, _position++, ">", null)
                    };
                }
                
                case '<':
                {
                    return Ahead switch
                    {
                        '=' => new SyntaxToken(TokenType.LessThanEquals, _position += 2, "<=", null),
                        '<' => new SyntaxToken(TokenType.LeftShift, _position += 2, "<<", null),
                        _ => new SyntaxToken(TokenType.LessThan, _position++, "<", null)
                    };
                }
                
                case '~':
                    return new SyntaxToken(TokenType.BitwiseNegation, _position++, "~", null);

                case '(':
                    return new SyntaxToken(TokenType.OpenParenthesis, _position++, "(", null);
                
                case ')':
                    return new SyntaxToken(TokenType.CloseParenthesis, _position++, ")", null);
            }

            _diagnostics.Add($"ERROR: Unrecognized Input: '{Current}'");
            return new SyntaxToken(TokenType.Bad, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}