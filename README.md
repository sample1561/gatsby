# Gatsby
Compiler in C# for Compiler Design Project

# Features Completed
- Parser, Lexer, Parse Tree
- Parsing using Operator Precedence Parsing
- Support Binary and Unary Operators
- Bool and INT32 Datatypes (C# Definition)

# Command Line Operation
- Arithmetic : Addition, Substraction, Division, Multiplication, Power, Modulas, Negation
- Boolean : AND, OR, Negation, Equality, Inequality
- Comparitive : Equals, Not-equals, Less Than, Less Than Equals, Greater Than, Greater Than Equals
- Bitwise : Left Shift, Right Shift, AND, OR, XOR 
- Variable Declaration & Assignement

# Operator precedence
- Created Syntax rules such that each operator has a numberic value denoting its precedence

# Semantic Representation
- The Semantic Representation is done by walking the syntax tree and binding the nodes a semantic tree with to symbolic information. The SemanticBinder represents the semantic analysis of our compiler and will perform things like looking up variable names in scope, performing type checks, and enforcing correctness rules.
- Currently we are using it to find type mismatch, ie for A && B, A and B have to be boolean true or
for C >= D, C and D have to be INT
