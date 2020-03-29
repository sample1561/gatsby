using System;
using Gatsby.Analysis.SemanticSyntax.Node;
using Gatsby.Analysis.SemanticSyntax.Operator;

namespace Gatsby.Analysis.SemanticSyntax.Expression
{
    internal sealed class SemanticBinaryExpression : SemanticExpression
    {
        
        public override SemanticNodeKind Kind => SemanticNodeKind.UnaryExpression;
        public override Type Type => Operator.Type;
        public SemanticExpression Left { get; }
        public SemanticBinaryOperator Operator { get; }
        public SemanticExpression Right { get; }
        
        public SemanticBinaryExpression(SemanticExpression left, SemanticBinaryOperator op, SemanticExpression right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }

    }
}
