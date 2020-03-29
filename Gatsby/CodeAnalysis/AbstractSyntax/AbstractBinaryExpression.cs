using System;

namespace Gatsby.CodeAnalysis.AbstractSyntax
{
    internal sealed class AbstractBinaryExpression : AbstractExpression
    {
        public AbstractBinaryExpression(AbstractExpression left, AbstractBinaryOperatorKind operatorKind, AbstractExpression right)
        {
            Left = left;
            OperatorKind = operatorKind;
            Right = right;
        }

        public override AbstractNodeKind Kind => AbstractNodeKind.UnaryExpression;
        public override Type Type => Left.Type;
        public AbstractExpression Left { get; }
        public AbstractBinaryOperatorKind OperatorKind { get; }
        public AbstractExpression Right { get; }
    }
}
