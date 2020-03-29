using System.Collections.Generic;

namespace Gatsby.Analysis.Syntax
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
            
            if (index >= _text.Length)
                    return '\0';

            return _text[index];
        }

        private void Next()
        {
            _position++;
        }

        public SyntaxToken Lex()
        {
            if (_position >= _text.Length)
                return new SyntaxToken(TokenType.EndOfFile, _position, "\0", null);

            if (char.IsDigit(Current))
            {
                var start = _position;

                while (char.IsDigit(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                if (!int.TryParse(text, out var value))
                    _diagnostics.Add($"The number {_text} isn't valid Int32.");

                return new SyntaxToken(TokenType.Number, start, text, value);
            }

            if (char.IsWhiteSpace(Current))
            {
                var start = _position;

                while (char.IsWhiteSpace(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                return new SyntaxToken(TokenType.Whitespace, start, text, null);
            }
            
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
                
                //TODO maybe add inverse divide a\b == b/a
                
                case '%':
                    return new SyntaxToken(TokenType.Modulo, _position++, "%", null);
                
                case '^':
                    return new SyntaxToken(TokenType.Power, _position++, "^", null);
                
                //TODO a | b means a divides b. 
                
                //Boolean Operators
                //TODO can this be use as a factorial operator?
                case '!':
                    return new SyntaxToken(TokenType.Negation, _position++, "!", null);

                case '&':
                {
                    if (Ahead == '&')
                    {
                        return new SyntaxToken(TokenType.LogicalAnd, _position += 2, "&&", null);
                    }

                    break;
                }

                case '|':
                {
                    if (Ahead == '|')
                    {
                        return new SyntaxToken(TokenType.LogicalOr, _position += 2, "||", null);
                    }

                    break;
                }
                
                //TODO bitwise operators
                // - ~x	bitwise not
                // - x & y	bitwise and
                // - x | y	bitwise or
                
                //TODO Comparative Operators
                //- ==
                //- !=
                // <=
                // >= 
                // > 
                // <
                

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