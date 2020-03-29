using System;
using Gatsby.Analysis.SemanticSyntax.Node;

namespace Gatsby.Analysis.SemanticSyntax.Expression
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
