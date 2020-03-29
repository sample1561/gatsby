using System;
using Gatsby.Analysis.SemanticSyntax.Node;
using Gatsby.Analysis.SemanticSyntax.Operator;

namespace Gatsby.Analysis.SemanticSyntax.Expression
{
    internal sealed class SemanticUnaryExpression : SemanticExpression
    {
        
        public override SemanticNodeKind Kind => SemanticNodeKind.UnaryExpression;
        public override Type Type => Operator.ResultType;
        public SemanticUnaryOperator Operator { get; }
        public SemanticExpression Operand { get; }
        
        public SemanticUnaryExpression(SemanticUnaryOperator op, SemanticExpression operand)
        {
            Operator = op;
            Operand = operand;
        }

    }
}
