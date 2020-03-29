using System;
using Gatsby.Analysis.Syntax;

namespace Gatsby.Analysis.AbstractSyntax
{
    internal sealed class AbstractBinaryOperator
    {
        public TokenType TokenType { get; }
        public AbstractBinaryOperatorKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type ResultType { get; }
        
        //Operation doesn't change the operand type
        private AbstractBinaryOperator(TokenType tokenType, AbstractBinaryOperatorKind kind, Type type)
        : this(tokenType, kind, type, type, type)
        {
        }

        //Operation changes the operand type
        public AbstractBinaryOperator(TokenType tokenType, AbstractBinaryOperatorKind kind, 
            Type leftType,Type rightType, Type resultType)
        {
            RightType = rightType;
            TokenType = tokenType;
            Kind = kind;
            LeftType = leftType;
            ResultType = resultType;
        }
        
        private static AbstractBinaryOperator[] _operators =
        {
            //Arithmetic Binary Operators
            new AbstractBinaryOperator(TokenType.Plus, AbstractBinaryOperatorKind.Addition, typeof(int)),
            new AbstractBinaryOperator(TokenType.Minus, AbstractBinaryOperatorKind.Subtraction, typeof(int)),
            new AbstractBinaryOperator(TokenType.Star, AbstractBinaryOperatorKind.Multiplication, typeof(int)),
            new AbstractBinaryOperator(TokenType.Slash, AbstractBinaryOperatorKind.Division, typeof(int)),
            new AbstractBinaryOperator(TokenType.Modulo, AbstractBinaryOperatorKind.Modulo, typeof(int)),
            new AbstractBinaryOperator(TokenType.Power, AbstractBinaryOperatorKind.Power, typeof(int)),

            //Boolean Binary Operators
            new AbstractBinaryOperator(TokenType.LogicalAnd, AbstractBinaryOperatorKind.Conjunction, typeof(bool)),
            new AbstractBinaryOperator(TokenType.LogicalOr, AbstractBinaryOperatorKind.Disjunction, typeof(bool)),
        };

        public static AbstractBinaryOperator Bind(TokenType tokenType, Type leftType, Type rightType)
        {
            foreach (var op in _operators)
            {
                if (op.TokenType == tokenType && op.LeftType == leftType && op.RightType == rightType)
                    return op;
            }

            return null;
        }
    }
}