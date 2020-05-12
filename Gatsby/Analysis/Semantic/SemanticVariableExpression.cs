using System;
using Gatsby.Analysis.Diagnostics;
using Gatsby.Analysis.Semantic.Expression;
using Gatsby.Analysis.Semantic.Node;

namespace Gatsby.Analysis.Semantic
{
    internal sealed class SemanticVariableExpression : SemanticExpression
    {
        public VariableSymbol Variable { get; }

        public SemanticVariableExpression(VariableSymbol variable)
        {
            Variable = variable;
        }

        public override Type Type => Variable.Type;
        public override SemanticNodeKind Kind => SemanticNodeKind.VariableExpression;
    }
}