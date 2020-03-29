using System;
using System.Collections.Generic;
using Gatsby.Analysis.SemanticSyntax.Expression;
using Gatsby.Analysis.SemanticSyntax.Operator;
using Gatsby.Analysis.Syntax.Expression;
using Gatsby.Analysis.Syntax.Lexer;

namespace Gatsby.Analysis.SemanticSyntax
{
    internal sealed class SemanticBinder
    {
        private readonly List<string> _diagnostics = new List<string>();

        public IEnumerable<string> Diagnostics => _diagnostics;

        public SemanticExpression SemanticExpression(ExpressionSyntax syntax)
        {
            return syntax.Kind switch
            {
                TokenType.LiteralExpression => SemanticLiteralExpression((LiteralExpressionSyntax) syntax),
                TokenType.UnaryExpression => SemanticUnaryExpression((UnaryExpressionSyntax) syntax),
                TokenType.BinaryExpression => SemanticBinaryExpression((BinaryExpressionSyntax) syntax),
                _ => throw new Exception($"Unexpected syntax {syntax.Kind}")
            };
        }

        private SemanticExpression SemanticLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var value = syntax.Value ?? 0;
            return new SemanticLiteralExpression(value);
        }

        private SemanticExpression SemanticUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperand = SemanticExpression(syntax.Operand);
            var boundOperator = SemanticUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);

            if (boundOperator == null)
            {
                _diagnostics.Add($"Unrecognized Unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}.");
                return boundOperand;
            }

            return new SemanticUnaryExpression(boundOperator, boundOperand);
        }

        private SemanticExpression SemanticBinaryExpression(BinaryExpressionSyntax syntax)
        {
            //Console.Write(syntax.Left.GetType());
            var boundLeft = SemanticExpression(syntax.Left);
            var boundRight = SemanticExpression(syntax.Right);
            var boundOperator = SemanticBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

            if (boundOperator == null)
            {
                _diagnostics.Add($"Binary operator '{syntax.OperatorToken.Text}' is not defined for types {boundLeft.Type} and {boundRight.Type}.");
                return boundLeft;
            }

            return new SemanticBinaryExpression(boundLeft, boundOperator, boundRight);
        }
    }
}
