using System;

namespace Gatsby.Analysis.AbstractSyntax
{
    internal sealed class AbstractUnaryExpression : AbstractExpression
    {
        public AbstractUnaryExpression(AbstractUnaryOperator op, AbstractExpression operand)
        {
            Operator = op;
            Operand = operand;
        }

        public override AbstractNodeKind Kind => AbstractNodeKind.UnaryExpression;
        public override Type Type => Operand.Type;
        public AbstractUnaryOperator Operator { get; }
        public AbstractExpression Operand { get; }
    }
}
