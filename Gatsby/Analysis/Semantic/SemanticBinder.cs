using System;
using System.Collections.Generic;
using System.Linq;
using Gatsby.Analysis.Diagnostics;
using Gatsby.Analysis.Semantic.Expression;
using Gatsby.Analysis.Semantic.Operator;
using Gatsby.Analysis.Syntax.Expression;
using Gatsby.Analysis.Syntax.Lexer;

namespace Gatsby.Analysis.Semantic
{
    internal sealed class SemanticBinder
    {
        private readonly Dictionary<VariableSymbol, object> _variables;
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

        public DiagnosticBag Diagnostics => _diagnostics;

        public SemanticBinder(Dictionary<VariableSymbol, object> variables)
        {
            _variables = variables;
        }

        public SemanticExpression SemanticExpression(ExpressionSyntax syntax)
        {
            return syntax.Kind switch
            {
                TokenType.ParenthesizedExpression => BindParenthesizedExpression((ParenthesizedExpressionSyntax)syntax),
                TokenType.LiteralExpression => BindLiteralExpression((LiteralExpressionSyntax) syntax),
                TokenType.NameExpression => BindNameExpression((NameExpressionSyntax)syntax),
                TokenType.AssignmentExpression => BindAssignmentExpression((AssignmentExpressionSyntax)syntax),
                TokenType.UnaryExpression => BindUnaryExpression((UnaryExpressionSyntax) syntax),
                TokenType.BinaryExpression => BindBinaryExpression((BinaryExpressionSyntax) syntax),
                _ => throw new Exception($"Unexpected token kind {syntax.Kind}")
            };
        }

        private SemanticExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
        {
            return SemanticExpression(syntax.Expression);
        }

        private SemanticExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var value = syntax.Value ?? 0;
            return new SemanticLiteralExpression(value);
        }
        
        private SemanticExpression BindNameExpression(NameExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;

            var variable = _variables.Keys.FirstOrDefault(v => v.Name == name);
            
            if (variable != null) 
                return new SemanticVariableExpression(variable);
            
            _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
            return new SemanticLiteralExpression(0);

        }
        
        private SemanticExpression BindAssignmentExpression(AssignmentExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var semanticExpression = SemanticExpression(syntax.Expression);

            var existingVariables = _variables.Keys.FirstOrDefault(v => v.Name == name);
            if (existingVariables != null)
                _variables.Remove(existingVariables);
            
            var variable = new VariableSymbol(name, semanticExpression.Type);
            _variables[variable] = null;
            
            return new SemanticAssignmentExpression(variable, semanticExpression);
        }

        private SemanticExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperand = SemanticExpression(syntax.Operand);
            var boundOperator = SemanticUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);

            if (boundOperator != null) 
                return new SemanticUnaryExpression(boundOperator, boundOperand);

            _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundOperand.Type);
            return boundOperand;

        }

        private SemanticExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
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
