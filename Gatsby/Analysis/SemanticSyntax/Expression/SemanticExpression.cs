using System;
using Gatsby.Analysis.SemanticSyntax.Node;

namespace Gatsby.Analysis.SemanticSyntax.Expression
{
    internal abstract class SemanticExpression : SemanticNode
    {
        public abstract Type Type { get; }
    }
}
