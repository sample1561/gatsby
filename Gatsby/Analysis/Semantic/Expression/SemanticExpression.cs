using System;
using Gatsby.Analysis.Semantic.Node;

namespace Gatsby.Analysis.Semantic.Expression
{
    internal abstract class SemanticExpression : SemanticNode
    {
        public abstract Type Type { get; }
    }
}
