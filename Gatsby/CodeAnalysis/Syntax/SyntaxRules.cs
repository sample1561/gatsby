using System;

namespace Gatsby.CodeAnalysis.Syntax
{
    internal static class SyntaxRules
    {
        public static int GetUnaryOperatorPrecedence(this TokenType kind)
        {
            switch (kind)
            {
                case TokenType.Plus:
                case TokenType.Minus:
                    return 4;

                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(this TokenType kind)
        {
            switch (kind)
            {
                case TokenType.Plus:
                case TokenType.Minus:
                    return 1;
                
                case TokenType.Star:
                case TokenType.Slash:
                case TokenType.Modulo:
                    return 2;
                
                case TokenType.Power:
                    return 3;

                

                default:
                    return 0;
            }
        }

        public static TokenType GetKeywordKind(string text)
        {
            text = text.ToLower();
            
            switch (text)
            {
                case "true":
                    return TokenType.TrueKeyword;
                case "false":
                    return TokenType.FalseKeyword;
                default:
                    return TokenType.Identifier;
            }
        }
    }
}