using System.Collections.Generic;
using Gatsby.Analysis.Syntax.Lexer;
using Gatsby.Analysis.Syntax.Tree;

namespace Gatsby.Analysis.Syntax.Expression
{
    public sealed class BinaryExpressionSyntax : ExpressionSyntax
    {
        public override TokenType Kind => TokenType.BinaryExpression;
        public ExpressionSyntax Left { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Right { get; }
        
        public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }
}