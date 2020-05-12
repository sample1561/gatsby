using System;
using System.Collections.Generic;
using Gatsby.Analysis.Diagnostics;
using Gatsby.Analysis.Semantic.Expression;
using Gatsby.Analysis.Semantic.Operator;
using Gatsby.Analysis.Syntax.Expression;
using Gatsby.Analysis.Syntax.Lexer;

namespace Gatsby.Analysis.Semantic
{
    internal sealed class SemanticBinder
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

        public DiagnosticBag Diagnostics => _diagnostics;

        public SemanticExpression SemanticExpression(ExpressionSyntax syntax)
        {
            return syntax.Kind switch
            {
                TokenType.LiteralExpression => SemanticLiteralExpression((LiteralExpressionSyntax) syntax),
                TokenType.UnaryExpression => SemanticUnaryExpression((UnaryExpressionSyntax) syntax),
                TokenType.BinaryExpression => SemanticBinaryExpression((BinaryExpressionSyntax) syntax),
                TokenType.ParenthesizedExpression => SemanticExpression(((ParenthesizedExpressionSyntax)syntax).Expression),
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

            if (boundOperator != null) 
                return new SemanticUnaryExpression(boundOperator, boundOperand);

            _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundOperand.Type);
            return boundOperand;

        }

        private SemanticExpression SemanticBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var boundLeft = SemanticExpression(syntax.Left);
            var boundRight = SemanticExpression(syntax.Right);
            var boundOperator = SemanticBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

            if (boundOperator != null) 
                return new SemanticBinaryExpression(boundLeft, boundOperator, boundRight);

            _diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundLeft.Type, boundRight.Type);

            return boundLeft;
        }
    }
}
