using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Gatsby.CodeAnalysis.Syntax;

namespace Gatsby.CodeAnalysis.AbstractSyntax
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
            var boundOperatorKind = AbstractUnaryOperatorKind(syntax.OperatorToken.Kind, boundOperand.Type);

            if (boundOperatorKind == null)
            {
                _diagnostics.Add($"Unrecognized Unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}.");
                return boundOperand;
            }

            return new AbstractUnaryExpression(boundOperatorKind.Value, boundOperand);
        }

        private AbstractExpression AbstractBinaryExpression(BinaryExpressionSyntax syntax)
        {
            //Console.Write(syntax.Left.GetType());
            var boundLeft = AbstractExpression(syntax.Left);
            var boundRight = AbstractExpression(syntax.Right);
            var boundOperatorKind = AbstractBinaryOperatorKind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

            if (boundOperatorKind == null)
            {
                _diagnostics.Add($"Binary operator '{syntax.OperatorToken.Text}' is not defined for types {boundLeft.Type} and {boundRight.Type}.");
                return boundLeft;
            }

            return new AbstractBinaryExpression(boundLeft, boundOperatorKind.Value, boundRight);
        }

        private AbstractUnaryOperatorKind? AbstractUnaryOperatorKind(TokenType kind, Type operandType)
        {
            //Arithmetic Bindings
            if (operandType == typeof(int))
            {
                return kind switch
                {
                    TokenType.Plus => AbstractSyntax.AbstractUnaryOperatorKind.Identity,
                    TokenType.Minus => AbstractSyntax.AbstractUnaryOperatorKind.Negation,
                    _ => throw new Exception($"Unrecognized Arithmetic Unary Operator {kind}")
                };
            }
            
            else if (operandType == typeof(bool))
            {
                return kind switch
                {
                    TokenType.Negation => AbstractSyntax.AbstractUnaryOperatorKind.LogicalNegation,
                    _ => throw new Exception($"Unrecognized Binary Unary Operator {kind}")
                };
            }
            
            return null;
        }

        private AbstractBinaryOperatorKind? AbstractBinaryOperatorKind(TokenType kind, Type leftType, Type rightType)
        {
            //Arithmetic Binary Operators
            if (leftType == typeof(int) && rightType == typeof(int))
            {
                return kind switch
                {
                    TokenType.Plus => AbstractSyntax.AbstractBinaryOperatorKind.Addition,
                    TokenType.Minus => AbstractSyntax.AbstractBinaryOperatorKind.Subtraction,
                    TokenType.Star => AbstractSyntax.AbstractBinaryOperatorKind.Multiplication,
                    TokenType.Slash => AbstractSyntax.AbstractBinaryOperatorKind.Division,
                    TokenType.Modulo => AbstractSyntax.AbstractBinaryOperatorKind.Modulo,
                    TokenType.Power => AbstractSyntax.AbstractBinaryOperatorKind.Power,
                    _ => throw new Exception($"Unrecognized Arithmetic Binary Operator {kind}")
                };
            }
            
            //Boolean Binary Operators
            if (leftType == typeof(bool) && rightType == typeof(bool))
            {
                return kind switch
                {
                    TokenType.And => AbstractSyntax.AbstractBinaryOperatorKind.Conjunction,
                    TokenType.Or => AbstractSyntax.AbstractBinaryOperatorKind.Disjunction,
                    _ => throw new Exception($"Unrecognized Boolean Binary Operator {kind}")
                };
            }

            //Mismatch bool-int pairing
            if (leftType == typeof(bool) && rightType == typeof(int) ||
                leftType == typeof(int) && rightType == typeof(bool))
            {
                throw new Exception($"Mismatch parse of operands {leftType} & {rightType}");
            }

            return null;
        }
    }
}
