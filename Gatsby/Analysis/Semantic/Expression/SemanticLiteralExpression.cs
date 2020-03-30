using System;
using Gatsby.Analysis.Semantic.Node;

namespace Gatsby.Analysis.Semantic.Expression
{
    internal sealed class SemanticLiteralExpression : SemanticExpression
    {
        
        public override SemanticNodeKind Kind => SemanticNodeKind.LiteralExpression;
        public override Type Type => Value.GetType();
        public object Value { get; }
        
        public SemanticLiteralExpression(object value)
        {
            Value = value;
        }

    }
}
