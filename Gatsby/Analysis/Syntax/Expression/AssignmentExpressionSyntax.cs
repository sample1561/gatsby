using System.Collections.Generic;
using Gatsby.Analysis.Syntax.Lexer;
using Gatsby.Analysis.Syntax.Tree;

namespace Gatsby.Analysis.Syntax.Expression
{
    /*
     * One way to understand assignment in a C based language is that is they are expressions that
     * are right associative
    */
    
    public sealed class AssignmentExpressionSyntax : ExpressionSyntax
    {
        public SyntaxToken IdentifierToken { get; }
        public SyntaxToken EqualsToken { get; }
        public ExpressionSyntax Expression { get; }

        public AssignmentExpressionSyntax(SyntaxToken identifierToken, SyntaxToken equalsToken, ExpressionSyntax expression)
        {
            IdentifierToken = identifierToken;
            EqualsToken = equalsToken;
            Expression = expression;
        }

        public override TokenType Kind => TokenType.AssignmentExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            yield return EqualsToken;
            yield return Expression;
        }
    }
}