using System.Collections.Generic;
using Gatsby.Analysis.Syntax.Lexer;
using Gatsby.Analysis.Syntax.Tree;

namespace Gatsby.Analysis.Syntax.Expression
{
    public sealed class LiteralExpressionSyntax : ExpressionSyntax
    {
        public override TokenType Kind => TokenType.LiteralExpression;
        public SyntaxToken LiteralToken { get; }
        public object Value { get; }
        
        public LiteralExpressionSyntax(SyntaxToken literalToken)
            : this(literalToken, literalToken.Value)
        {
        }

        public LiteralExpressionSyntax(SyntaxToken literalToken, object value)
        {
            LiteralToken = literalToken;
            Value = value;
        }


        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LiteralToken;
        }
    }
}