using System;
using Gatsby.Analysis.Syntax;

namespace Gatsby.Analysis.AbstractSyntax
{
    internal sealed class AbstractUnaryOperator
    {
        public TokenType TokenType { get; }
        public AbstractUnaryOperatorKind Kind { get; }
        public Type OperandType { get; }
        public Type ResultType { get; }

        //Operation doesn't change the operand type
        public AbstractUnaryOperator(TokenType tokenKind, AbstractUnaryOperatorKind kind, Type operandType) 
            : this(tokenKind, kind, operandType, operandType)
        {
        }

        //Operation doesn't changes the operand type
        public AbstractUnaryOperator(TokenType tokenKind, AbstractUnaryOperatorKind kind,
            Type operandType, Type resultType)
        {
            TokenType = tokenKind;
            Kind = kind;
            OperandType = operandType;
            ResultType = resultType;
        }

        private static AbstractUnaryOperator[] _operators =
        {
            //Arithmetic Unary Operators
            new AbstractUnaryOperator(TokenType.Plus, AbstractUnaryOperatorKind.Identity, typeof(int)),
            new AbstractUnaryOperator(TokenType.Minus, AbstractUnaryOperatorKind.Negative, typeof(int)),
            
            //Binary Unary Operators
            new AbstractUnaryOperator(TokenType.Negation, AbstractUnaryOperatorKind.LogicalNegation, typeof(bool))
        };

        public static AbstractUnaryOperator Bind(TokenType tokenType, Type operandType)
        {
            //Bind our existing operator to our new AbstractUnaryOperator 
            foreach (var op in _operators)
            {
                if (op.TokenType == tokenType && op.OperandType == operandType)
                    return op;
            }

            return null;
        }
    }
}