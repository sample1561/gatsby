namespace Gatsby.Analysis.Semantic.Operator.Kind
{
    internal enum SemanticBinaryOperatorKind
    {
        //Arithmetic Operators
        Addition,
        Subtraction,
        Multiplication,
        Division,
        ReverseDivision,
        Modulo,
        Power,
        
        //Boolean Operators
        Conjunction,
        Disjunction,
        
        //Comparision Operators
        EqualsTo,
        NotEqualsTo,
        LessThan,
        LessThanEquals,
        GreaterThan,
        GreaterThanEquals,
        
        //Bitwise Operators
        BitwiseAnd,
        BitwiseOr,
        BitwiseXor,
        LeftShift,
        RightShift
    }
}
