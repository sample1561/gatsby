using System;
using Gatsby.Analysis.Diagnostics;
using Gatsby.Analysis.Semantic.Expression;
using Gatsby.Analysis.Semantic.Node;
using Gatsby.Analysis.Syntax.Expression;

namespace Gatsby.Analysis.Semantic
{
    internal sealed class SemanticAssignmentExpression : SemanticExpression
    {
        public SemanticAssignmentExpression(VariableSymbol variable, SemanticExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }

        public VariableSymbol Variable { get; }
        public SemanticExpression Expression { get; }
        
        public override SemanticNodeKind Kind => SemanticNodeKind.AssignmentExpression;
        public override Type Type => Variable.Type;
    }
}