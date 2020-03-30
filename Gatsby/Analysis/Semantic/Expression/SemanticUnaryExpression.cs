using System;
using Gatsby.Analysis.Semantic.Node;
using Gatsby.Analysis.Semantic.Operator;

namespace Gatsby.Analysis.Semantic.Expression
{
    internal sealed class SemanticUnaryExpression : SemanticExpression
    {
        
        public override SemanticNodeKind Kind => SemanticNodeKind.UnaryExpression;
        public override Type Type => Operator.ResultType;
        public SemanticUnaryOperator Operator { get; }
        public SemanticExpression Operand { get; }
        
        public SemanticUnaryExpression(SemanticUnaryOperator op, 
            SemanticExpression operand)
        {
            Operator = op;
            Operand = operand;
        }

    }
}
