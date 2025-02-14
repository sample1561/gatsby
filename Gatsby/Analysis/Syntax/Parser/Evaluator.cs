﻿using System;
using System.Collections.Generic;
using Gatsby.Analysis.Semantic.Expression;
using Gatsby.Analysis.Semantic.Operator.Kind;

namespace Gatsby.Analysis.Syntax.Parser
{
    internal sealed class Evaluator
    {
        private readonly SemanticExpression _root;
        private readonly Dictionary<VariableSymbol, object> _variables;

        public Evaluator(SemanticExpression root, Dictionary<VariableSymbol,object> variables)
        {
            _root = root;
            _variables = variables;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(SemanticExpression node)
        {
            //Convert the semantic representation into a concrete one
            switch (node)
            {
                case SemanticLiteralExpression n:
                    return n.Value;

                case SemanticVariableExpression v:
                    return _variables[v.Variable];

                case SemanticAssignmentExpression a:
                {
                    var value = EvaluateExpression(a.Expression);
                    _variables[a.Variable] = value;
                    return value;
                }
                
                case SemanticUnaryExpression u:
                {
                    var operand = EvaluateExpression(u.Operand);

                    return u.Operator.Kind switch
                    {
                        SemanticUnaryOperatorKind.Identity => (object) (int) operand,
                        SemanticUnaryOperatorKind.Negative => (-1 * (int) operand),
                        SemanticUnaryOperatorKind.LogicalNegation => !(bool) operand,
                        SemanticUnaryOperatorKind.BitwiseNegation => ~(int)operand, 
                        _ => throw new Exception($"Unexpected unary operator {u.Operator.Kind}")
                    };
                }
                
                case SemanticBinaryExpression b:
                {
                    var left = EvaluateExpression(b.Left);
                    var right = EvaluateExpression(b.Right);

                    return b.Operator.Kind switch
                    {
                        SemanticBinaryOperatorKind.Addition => ((int)left + (int)right),
                        SemanticBinaryOperatorKind.Subtraction => ((int)left - (int)right),
                    
                        SemanticBinaryOperatorKind.Multiplication => ((int)left * (int)right),
                        SemanticBinaryOperatorKind.Division => ((int)left / (int)right),
                        SemanticBinaryOperatorKind.ReverseDivision => ((int)right / (int)left),
                        SemanticBinaryOperatorKind.Modulo => ((int)left % (int)right),
                    
                        SemanticBinaryOperatorKind.Power => (int) Math.Pow((int)left, (int)right),
                    
                        SemanticBinaryOperatorKind.Conjunction => (bool)left && (bool)right,
                        SemanticBinaryOperatorKind.Disjunction => (bool)left || (bool)right,
                    
                        SemanticBinaryOperatorKind.EqualsTo => Equals(left,right), 
                        SemanticBinaryOperatorKind.NotEqualsTo => !Equals(left,right), 
                    
                        SemanticBinaryOperatorKind.GreaterThan => (int)left > (int)right,
                        SemanticBinaryOperatorKind.GreaterThanEquals => (int)left >= (int)right, 
                    
                        SemanticBinaryOperatorKind.LessThan => (int)left < (int)right, 
                        SemanticBinaryOperatorKind.LessThanEquals => (int)left <= (int)right, 
                    
                        SemanticBinaryOperatorKind.BitwiseAnd => (int)left & (int)right, 
                        SemanticBinaryOperatorKind.BitwiseOr => (int)left | (int)right,
                        SemanticBinaryOperatorKind.BitwiseXor => (int)left ^ (int)right,
                        SemanticBinaryOperatorKind.LeftShift => (int)left << (int)right,
                        SemanticBinaryOperatorKind.RightShift => (int)left >> (int)right,
                    
                        _ => throw new Exception($"Unexpected binary operator {b.Operator}")
                    };
                }
                
                default:
                    throw new Exception($"Unexpected node {node.Kind}");
            }
        }
    }
}