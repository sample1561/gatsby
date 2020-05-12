using System;
using Gatsby.Analysis.Semantic.Node;
using Gatsby.Analysis.Syntax.Parser;

namespace Gatsby.Analysis.Semantic.Expression
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