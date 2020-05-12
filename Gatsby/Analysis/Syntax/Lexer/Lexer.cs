using System.Collections.Generic;
using Gatsby.Analysis.Diagnostics;
using Gatsby.Analysis.Syntax.Parser;

namespace Gatsby.Analysis.Syntax.Lexer
{
    internal sealed class Lexer
    {
        private readonly string _text;
        private int _position;
        private DiagnosticBag _diagnostics = new DiagnosticBag();

        public Lexer(string text)
        {
            _text = text;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

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
            
            var start = _position;

            //Integer
            if (char.IsDigit(Current))
            {
                

                while (char.IsDigit(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                if (!int.TryParse(text, out var value))
                    _diagnostics.ReportInvalidNumber(new TextSpan(start, length), _text, typeof(int));

                return new SyntaxToken(TokenType.Number, start, text, value);
            }

            //Whitespace
            if (char.IsWhiteSpace(Current))
            {
                while (char.IsWhiteSpace(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                return new SyntaxToken(TokenType.Whitespace, start, text, null);
            }
            
            //True-False and catch-all identifiers
            if (char.IsLetter(Current))
            {
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
                    switch (Ahead)
                    {
                        case '=':
                            _position += 2;
                            return new SyntaxToken(TokenType.Negation, start, "!=", null);
                        
                        default:
                            return new SyntaxToken(TokenType.Negation, start, "!", null);
                    }
                }

                case '&':
                {
                    switch (Ahead)
                    {
                        case '&':
                            _position += 2;
                            return new SyntaxToken(TokenType.LogicalAnd, start, "&&", null);
                        
                        default:
                            return new SyntaxToken(TokenType.BitwiseAnd, _position++, "&", null);
                    }
                }

                case '|':
                {
                    switch (Ahead)
                    {
                        case '|':
                            _position += 2;
                            return new SyntaxToken(TokenType.LogicalOr, start, "||", null);
                        
                        default:
                            return new SyntaxToken(TokenType.BitwiseOr, _position++, "&", null);
                    }
                }
                
                case '=':
                {
                    if (Ahead == '=')
                    {
                        _position += 2;
                        return new SyntaxToken(TokenType.EqualsTo, start, "==", null);
                    }
                    
                    //We haven't implemented the assignment operator
                    else
                        break;
                }

                case '>':
                {
                    switch (Ahead)
                    {
                        case '=':
                            _position += 2;
                            return new SyntaxToken(TokenType.GreaterThanEquals, start, ">=", null);
                        
                        case '>':
                            _position += 2;
                            return new SyntaxToken(TokenType.RightShift, start, ">>", null);
                        
                        default:
                            return new SyntaxToken(TokenType.GreaterThan, _position++, ">", null);
                    }
                }
                
                case '<':
                {
                    switch (Ahead)
                    {
                        case '=':
                            _position += 2;
                            return new SyntaxToken(TokenType.LessThanEquals, start, "<=", null);
                        
                        case '<':
                            _position += 2;
                            return new SyntaxToken(TokenType.LeftShift, start, "<<", null);
                        
                        default:
                            return new SyntaxToken(TokenType.LessThan, _position++, "<", null);
                    }
                }
                
                case '~':
                    return new SyntaxToken(TokenType.BitwiseNegation, _position++, "~", null);

                case '(':
                    return new SyntaxToken(TokenType.OpenParenthesis, _position++, "(", null);
                
                case ')':
                    return new SyntaxToken(TokenType.CloseParenthesis, _position++, ")", null);
            }

            _diagnostics.ReportBadCharacter(_position, Current);
            return new SyntaxToken(TokenType.Bad, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}