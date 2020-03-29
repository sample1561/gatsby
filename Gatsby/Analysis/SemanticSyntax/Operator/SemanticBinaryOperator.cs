using System;
using Gatsby.Analysis.SemanticSyntax.Operator.OperatorKind;
using Gatsby.Analysis.Syntax.Lexer;

namespace Gatsby.Analysis.SemanticSyntax.Operator
{
    internal sealed class SemanticBinaryOperator
    {
        public TokenType TokenType { get; }
        public SemanticBinaryOperatorKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type Type { get; }
        
        //Operation doesn't change the operand type
        private SemanticBinaryOperator(TokenType tokenType, SemanticBinaryOperatorKind kind, Type type)
        : this(tokenType, kind, type, type, type)
        {
        }
        
        private SemanticBinaryOperator(TokenType tokenType, SemanticBinaryOperatorKind kind, Type operandType, Type resultType)
            : this(tokenType, kind, operandType, operandType, resultType)
        {
        }

        //Operation changes the operand type
        public SemanticBinaryOperator(TokenType tokenType, SemanticBinaryOperatorKind kind, 
            Type leftType,Type rightType, Type type)
        {
            RightType = rightType;
            TokenType = tokenType;
            Kind = kind;
            LeftType = leftType;
            Type = type;
        }
        
        private static SemanticBinaryOperator[] _operators =
        {
            //Arithmetic Binary Operators
            new SemanticBinaryOperator(TokenType.Plus, SemanticBinaryOperatorKind.Addition, typeof(int)),
            new SemanticBinaryOperator(TokenType.Minus, SemanticBinaryOperatorKind.Subtraction, typeof(int)),
            new SemanticBinaryOperator(TokenType.Star, SemanticBinaryOperatorKind.Multiplication, typeof(int)),
            new SemanticBinaryOperator(TokenType.Slash, SemanticBinaryOperatorKind.Division, typeof(int)),
            new SemanticBinaryOperator(TokenType.ReverseSlash, SemanticBinaryOperatorKind.ReverseDivision, typeof(int)),
            new SemanticBinaryOperator(TokenType.Modulo, SemanticBinaryOperatorKind.Modulo, typeof(int)),
            new SemanticBinaryOperator(TokenType.Power, SemanticBinaryOperatorKind.Power, typeof(int)),

            //Boolean Binary Operators
            new SemanticBinaryOperator(TokenType.LogicalAnd, SemanticBinaryOperatorKind.Conjunction, typeof(bool)),
            new SemanticBinaryOperator(TokenType.LogicalOr, SemanticBinaryOperatorKind.Disjunction, typeof(bool)),
            
            //Comparision Operators - Arithmetic
            new SemanticBinaryOperator(TokenType.EqualsTo, SemanticBinaryOperatorKind.EqualsTo,typeof(int),typeof(bool)), 
            new SemanticBinaryOperator(TokenType.NotEqualsTo, SemanticBinaryOperatorKind.NotEqualsTo,typeof(int),typeof(bool)),
            
            new SemanticBinaryOperator(TokenType.GreaterThan, SemanticBinaryOperatorKind.GreaterThan,typeof(int),typeof(bool)),
            new SemanticBinaryOperator(TokenType.GreaterThanEquals, SemanticBinaryOperatorKind.GreaterThanEquals,typeof(int),typeof(bool)),
            new SemanticBinaryOperator(TokenType.LessThan, SemanticBinaryOperatorKind.LessThan,typeof(int),typeof(bool)),
            new SemanticBinaryOperator(TokenType.LessThanEquals, SemanticBinaryOperatorKind.LessThanEquals,typeof(int),typeof(bool)),
            
            //Comparision Operators - Boolean
            new SemanticBinaryOperator(TokenType.EqualsTo, SemanticBinaryOperatorKind.EqualsTo,typeof(bool)), 
            new SemanticBinaryOperator(TokenType.NotEqualsTo, SemanticBinaryOperatorKind.NotEqualsTo,typeof(bool)),
            
            //Bitwise Operators -
            new SemanticBinaryOperator(TokenType.BitwiseAnd, SemanticBinaryOperatorKind.BitwiseAnd,typeof(int)), 
            new SemanticBinaryOperator(TokenType.BitwiseOr, SemanticBinaryOperatorKind.BitwiseOr,typeof(int)),
            new SemanticBinaryOperator(TokenType.BitwiseXor, SemanticBinaryOperatorKind.BitwiseXor,typeof(int)),
            new SemanticBinaryOperator(TokenType.EqualsTo, SemanticBinaryOperatorKind.EqualsTo,typeof(int)),
            new SemanticBinaryOperator(TokenType.LeftShift, SemanticBinaryOperatorKind.LeftShift,typeof(int)),
            new SemanticBinaryOperator(TokenType.RightShift, SemanticBinaryOperatorKind.RightShift,typeof(int))
        };

        public static SemanticBinaryOperator Bind(TokenType tokenType, Type leftType, Type rightType)
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