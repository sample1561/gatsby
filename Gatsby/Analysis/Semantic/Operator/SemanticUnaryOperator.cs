using System;
using Gatsby.Analysis.Semantic.Operator.Kind;
using Gatsby.Analysis.Syntax.Lexer;

namespace Gatsby.Analysis.Semantic.Operator
{
    internal sealed class SemanticUnaryOperator
    {
        public TokenType TokenType { get; }
        public SemanticUnaryOperatorKind Kind { get; }
        public Type OperandType { get; }
        public Type ResultType { get; }

        //Operation doesn't change the operand type
        public SemanticUnaryOperator(TokenType tokenKind, SemanticUnaryOperatorKind kind, Type operandType) 
            : this(tokenKind, kind, operandType, operandType)
        {
        }

        //Operation doesn't changes the operand type
        public SemanticUnaryOperator(TokenType tokenKind, SemanticUnaryOperatorKind kind,
            Type operandType, Type resultType)
        {
            TokenType = tokenKind;
            Kind = kind;
            OperandType = operandType;
            ResultType = resultType;
        }

        private static SemanticUnaryOperator[] _operators =
        {
            //Arithmetic Unary Operators
            new SemanticUnaryOperator(TokenType.Plus, SemanticUnaryOperatorKind.Identity, typeof(int)),
            new SemanticUnaryOperator(TokenType.Minus, SemanticUnaryOperatorKind.Negative, typeof(int)),
            
            //Binary Unary Operators
            new SemanticUnaryOperator(TokenType.Negation, SemanticUnaryOperatorKind.LogicalNegation, typeof(bool)),
            
            //Bitwise Negation
            new SemanticUnaryOperator(TokenType.BitwiseNegation,SemanticUnaryOperatorKind.BitwiseNegation,typeof(int)) 
        };

        public static SemanticUnaryOperator Bind(TokenType tokenType, Type operandType)
        {
            //Bind our existing operator to our new SemanticUnaryOperator 
            foreach (var op in _operators)
            {
                if (op.TokenType == tokenType && op.OperandType == operandType)
                    return op;
            }

            return null;
        }
    }
}