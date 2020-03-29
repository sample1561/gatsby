using System;

namespace Gatsby.Analysis.AbstractSyntax
{
    internal sealed class AbstractLiteralExpression : AbstractExpression
    {
        public AbstractLiteralExpression(object value)
        {
            Value = value;
        }

        public override AbstractNodeKind Kind => AbstractNodeKind.LiteralExpression;
        public override Type Type => Value.GetType();
        public object Value { get; }
    }
}
