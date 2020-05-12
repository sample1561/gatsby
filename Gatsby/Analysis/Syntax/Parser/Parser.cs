using System;
using System.Collections.Generic;
using System.Xml;
using Gatsby.Analysis.Syntax.Expression;
using Gatsby.Analysis.Syntax.Lexer;
using Gatsby.Analysis.Syntax.Tree;

namespace Gatsby.Analysis.Syntax.Parser
{
    internal sealed class Parser
    {
        private readonly SyntaxToken[] _tokens;

        private readonly List<string> _diagnostics = new List<string>();
        private int _position;

        public Parser(string text)
        {
            var tokens = new List<SyntaxToken>();

            var lexer = new Lexer.Lexer(text);
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
            return index >= _tokens.Length ? _tokens[^1] : _tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        //Return the current token but move the position
        private SyntaxToken NextToken()
        {
            var current = Current;
            _position++;
            return current;
        }

        private SyntaxToken MatchToken(TokenType kind)
        {
            //Move the position if we the correct expected token
            if (Current.Kind == kind)
                return NextToken();

            _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}>, " +
                             $"expected <{kind}>");
            return new SyntaxToken(kind, Current.Position, null, null);
        }

        public SyntaxTree Parse()
        {
            var expresion = ParseExpression();
            var endOfFileToken = MatchToken(TokenType.EndOfFile);
            return new SyntaxTree(expresion, endOfFileToken, _diagnostics);
        }

        private ExpressionSyntax ParseExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;
            var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
            
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseExpression(unaryOperatorPrecedence);
                
                //Create new Left branch for the current expression
                left = new UnaryExpressionSyntax(operatorToken, operand);
            }
            
            else
            {
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                var precedence = Current.Kind.GetBinaryOperatorPrecedence();
                
                /* Current is not a binary operator of the operand it weaker
                 * Then the previous operator, 1 * 2 + 3
                 * * > + hence it will return left, ie, 2 to create
                 *
                 *         +
                 *       /  \
                 *     *     3
                 *    / \     
                 *   1   2
                 */
                 
                
                if (precedence == 0 || precedence <= parentPrecedence)
                    break;

                var operatorToken = NextToken();
                var right = ParseExpression(precedence);
                
                //Substitute the left branch with a new subtree
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
                    
                    //Create virtual closed paren token
                    var right = MatchToken(TokenType.CloseParenthesis);
                    
                    return new ParenthesizedExpressionSyntax(left, 
                        expression, right);
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
                    //Create a virtual number token as that is the only possible node
                    var numberToken = MatchToken(TokenType.Number);
                    return new LiteralExpressionSyntax(numberToken);
                }
            }
        }

        public void PrintTokenization()
        {
            foreach(var token in _tokens)
            {
                Console.WriteLine($"Type = {token.Kind}\tText = '{token.Text}'\tPosition = {token.Position}\tValue = {token.Value}");
            }
        }
    }
}