using System;
using System.Collections.Generic;
using Gatsby.Analysis.Syntax;

namespace Gatsby.Analysis.AbstractSyntax
{
    internal sealed class AbstractBinder
    {
        private readonly List<string> _diagnostics = new List<string>();

        public IEnumerable<string> Diagnostics => _diagnostics;

        public AbstractExpression AbstractExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case TokenType.LiteralExpression:
                    return AbstractLiteralExpression((LiteralExpressionSyntax)syntax);
                case TokenType.UnaryExpression:
                    return AbstractUnaryExpression((UnaryExpressionSyntax)syntax);
                case TokenType.BinaryExpression:
                    return AbstractBinaryExpression((BinaryExpressionSyntax)syntax);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        private AbstractExpression AbstractLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var value = syntax.Value ?? 0;
            return new AbstractLiteralExpression(value);
        }

        private AbstractExpression AbstractUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperand = AbstractExpression(syntax.Operand);
            var boundOperator = AbstractUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);

            if (boundOperator == null)
            {
                _diagnostics.Add($"Unrecognized Unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}.");
                return boundOperand;
            }

            return new AbstractUnaryExpression(boundOperator, boundOperand);
        }

        private AbstractExpression AbstractBinaryExpression(BinaryExpressionSyntax syntax)
        {
            //Console.Write(syntax.Left.GetType());
            var boundLeft = AbstractExpression(syntax.Left);
            var boundRight = AbstractExpression(syntax.Right);
            var boundOperator = AbstractBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

            if (boundOperator == null)
            {
                _diagnostics.Add($"Binary operator '{syntax.OperatorToken.Text}' is not defined for types {boundLeft.Type} and {boundRight.Type}.");
                return boundLeft;
            }

            return new AbstractBinaryExpression(boundLeft, boundOperator, boundRight);
        }
    }
}
