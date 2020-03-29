using System;

namespace Gatsby.Analysis.AbstractSyntax
{
    internal sealed class AbstractBinaryExpression : AbstractExpression
    {
        public AbstractBinaryExpression(AbstractExpression left, AbstractBinaryOperator op, AbstractExpression right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }

        public override AbstractNodeKind Kind => AbstractNodeKind.UnaryExpression;
        public override Type Type => Left.Type;
        public AbstractExpression Left { get; }
        public AbstractBinaryOperator Operator { get; }
        public AbstractExpression Right { get; }
    }
}
