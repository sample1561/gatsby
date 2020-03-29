using System.Collections.Generic;
using Gatsby.Analysis.Syntax.Lexer;
using Gatsby.Analysis.Syntax.Tree;

namespace Gatsby.Analysis.Syntax.Expression
{
    public sealed class UnaryExpressionSyntax : ExpressionSyntax
    {
        public override TokenType Kind => TokenType.UnaryExpression;
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Operand { get; }
        
        public UnaryExpressionSyntax(SyntaxToken operatorToken, ExpressionSyntax operand)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }
        
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OperatorToken;
            yield return Operand;
        }
    }

}