using System.Collections.Generic;

namespace Gatsby.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private readonly string _text;
        private int _position;
        private List<string> _diagnostics = new List<string>();

        public Lexer(string text)
        {
            _text = text;
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private char Current
        {
            get
            {
                if (_position >= _text.Length)
                    return '\0';

                return _text[_position];
            }
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
            
            //TODO boolean support
            if (char.IsLetter(Current))
            {
                var start = _position;

                while (char.IsLetter(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                var kind = SyntaxRules.GetKeywordKind(text);
                return new SyntaxToken(kind, start, text, null);
            }

            switch (Current)
            {
                case '+':
                    return new SyntaxToken(TokenType.Plus, _position++, "+", null);
                case '-':
                    return new SyntaxToken(TokenType.Minus, _position++, "-", null);
                case '*':
                    return new SyntaxToken(TokenType.Star, _position++, "*", null);
                case '/':
                    return new SyntaxToken(TokenType.Slash, _position++, "/", null);
                case '%':
                    return new SyntaxToken(TokenType.Modulo, _position++, "%", null);
                case '^':
                    return new SyntaxToken(TokenType.Power, _position++, "^", null);
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