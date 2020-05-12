using System.Collections.Generic;
using Gatsby.Analysis.Syntax.Lexer;
using Gatsby.Analysis.Syntax.Tree;

namespace Gatsby.Analysis.Syntax.Expression
{
    public sealed class NameExpressionSyntax : ExpressionSyntax
    {
        public NameExpressionSyntax(SyntaxToken identifierToken)
        {
            IdentifierToken = identifierToken;
        }
        
        public SyntaxToken IdentifierToken { get; }
        public override TokenType Kind => TokenType.NameExpression;
        
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
        }
    }
}