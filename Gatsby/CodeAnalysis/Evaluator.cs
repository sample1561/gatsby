﻿using System;
 using Gatsby.CodeAnalysis.AbstractSyntax;

 namespace Gatsby.CodeAnalysis
{
    internal sealed class Evaluator
    {
        private readonly AbstractExpression _root;

        public Evaluator(AbstractExpression root)
        {
            _root = root;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(AbstractExpression node)
        {
            if (node is AbstractLiteralExpression n)
                return n.Value;

            if (node is AbstractUnaryExpression u)
            {
                var operand = (int)EvaluateExpression(u.Operand);

                switch (u.OperatorKind)
                {
                    case AbstractUnaryOperatorKind.Identity:
                        return operand;
                    case AbstractUnaryOperatorKind.Negation:
                        return -1*operand;
                    default:
                        throw new Exception($"Unexpected unary operator {u.OperatorKind}");
                }
            }

            if (node is AbstractBinaryExpression b)
            {
                var left = (int)EvaluateExpression(b.Left);
                var right = (int)EvaluateExpression(b.Right);

                switch (b.OperatorKind)
                {
                    case AbstractBinaryOperatorKind.Addition:
                        return left + right;
                    case AbstractBinaryOperatorKind.Subtraction:
                        return left - right;
                    case AbstractBinaryOperatorKind.Multiplication:
                        return left * right;
                    case AbstractBinaryOperatorKind.Division:
                        return left / right;
                    case AbstractBinaryOperatorKind.Modulo:
                        return left % right;
                    case AbstractBinaryOperatorKind.Power:
                        return (int) Math.Pow(left, right);
                    default:
                        throw new Exception($"Unexpected binary operator {b.OperatorKind}");
                }
            }

            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}