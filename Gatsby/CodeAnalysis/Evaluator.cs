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
                var operand = EvaluateExpression(u.Operand);

                return u.OperatorKind switch
                {
                    AbstractUnaryOperatorKind.Identity => (object) (int) operand,
                    AbstractUnaryOperatorKind.Negation => (-1 * (int) operand),
                    AbstractUnaryOperatorKind.LogicalNegation => !(bool) operand,
                    _ => throw new Exception($"Unexpected unary operator {u.OperatorKind}")
                };
            }

            if (node is AbstractBinaryExpression b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                return b.OperatorKind switch
                {
                    AbstractBinaryOperatorKind.Addition => ((int)left + (int)right),
                    AbstractBinaryOperatorKind.Subtraction => ((int)left - (int)right),
                    AbstractBinaryOperatorKind.Multiplication => ((int)left * (int)right),
                    AbstractBinaryOperatorKind.Division => ((int)left / (int)right),
                    AbstractBinaryOperatorKind.Modulo => ((int)left % (int)right),
                    AbstractBinaryOperatorKind.Power => (int) Math.Pow((int)left, (int)right),
                    AbstractBinaryOperatorKind.Conjunction => (bool)left && (bool)right,
                    AbstractBinaryOperatorKind.Disjunction => (bool)left || (bool)right,
                    
                    _ => throw new Exception($"Unexpected binary operator {b.OperatorKind}")
                };
            }

            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}