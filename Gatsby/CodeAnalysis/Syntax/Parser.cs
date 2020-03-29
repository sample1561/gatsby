using System;
using System.Collections.Generic;

namespace Gatsby.CodeAnalysis.Syntax
{
    internal sealed class Parser
    {
        private readonly SyntaxToken[] _tokens;

        private List<string> _diagnostics = new List<string>();
        private int _position;

        public Parser(string text)
        {
            var tokens = new List<SyntaxToken>();

            var lexer = new Lexer(text);
            SyntaxToken token;
            do
            {
                token = lexer.Lex();

                if (token.Kind != TokenType.Whitespace &&
                    token.Kind != TokenType.Bad)
                {
                    tokens.Add(token);   
                }
            } while (token.Kind != TokenType.EndOfFile);

            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private SyntaxToken Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _tokens.Length)
                return _tokens[_tokens.Length - 1];

            return _tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken()
        {
            var current = Current;
            _position++;
            return current;
        }

        private SyntaxToken MatchToken(TokenType kind)
        {
            if (Current.Kind == kind)
                return NextToken();

            _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}>, expected <{kind}>");
            return new SyntaxToken(kind, Current.Position, null, null);
        }

        public SyntaxTree Parse()
        {
            var expresion = ParseExpression();
            var endOfFileToken = MatchToken(TokenType.EndOfFile);
            return new SyntaxTree(_diagnostics, expresion, endOfFileToken);
        }

        private ExpressionSyntax ParseExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;
            var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseExpression(unaryOperatorPrecedence);
                left = new UnaryExpressionSyntax(operatorToken, operand);
            }
            else
            {
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                var precedence = Current.Kind.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence)
                    break;

                var operatorToken = NextToken();
                var right = ParseExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            switch (Current.Kind)
            {
                case TokenType.OpenParenthesis:
                {
                    var left = NextToken();
                    var expression = ParseExpression();
                    var right = MatchToken(TokenType.CloseParenthesis);
                    return new ParenthesizedExpressionSyntax(left, expression, right);
                }

                case TokenType.TrueKeyword:
                case TokenType.FalseKeyword:
                {
                    var keywordToken = NextToken();
                    var value = keywordToken.Kind == TokenType.TrueKeyword;
                    return new LiteralExpressionSyntax(keywordToken, value);
                }
                    
                default:
                {
                    var numberToken = MatchToken(TokenType.Number);
                    return new LiteralExpressionSyntax(numberToken);
                }
            }
        }

        public void PrintTokenization()
        {
            foreach(var token in _tokens)
            {
                Console.Write($"{token.Kind}\t{token.Text}+\t{token.Position}\t{token.Value}");
            }
        }
    }
}