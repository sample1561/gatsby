using System;

namespace Gatsby.Analysis.AbstractSyntax
{
    internal abstract class AbstractExpression : AbstractNode
    {
        public abstract Type Type { get; }
    }
}
