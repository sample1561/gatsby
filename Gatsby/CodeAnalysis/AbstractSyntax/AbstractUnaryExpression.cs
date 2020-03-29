using System;

namespace Gatsby.CodeAnalysis.AbstractSyntax
{
    internal sealed class AbstractUnaryExpression : AbstractExpression
    {
        public AbstractUnaryExpression(AbstractUnaryOperatorKind operatorKind, AbstractExpression operand)
        {
            OperatorKind = operatorKind;
            Operand = operand;
        }

        public override AbstractNodeKind Kind => AbstractNodeKind.UnaryExpression;
        public override Type Type => Operand.Type;
        public AbstractUnaryOperatorKind OperatorKind { get; }
        public AbstractExpression Operand { get; }
    }
}
