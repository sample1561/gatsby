using System;
using Gatsby.Analysis.Semantic.Node;
using Gatsby.Analysis.Semantic.Operator;

namespace Gatsby.Analysis.Semantic.Expression
{
    internal sealed class SemanticBinaryExpression : SemanticExpression
    {
        
        public override SemanticNodeKind Kind => SemanticNodeKind.BinaryExpression;
        public override Type Type => Operator.Type;
        public SemanticExpression Left { get; }
        public SemanticBinaryOperator Operator { get; }
        public SemanticExpression Right { get; }
        
        public SemanticBinaryExpression(SemanticExpression left, 
            SemanticBinaryOperator op, SemanticExpression right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }

    }
}
