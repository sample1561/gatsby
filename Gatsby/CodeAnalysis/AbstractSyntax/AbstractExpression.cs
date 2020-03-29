using System;

namespace Gatsby.CodeAnalysis.AbstractSyntax
{
    internal abstract class AbstractExpression : AbstractNode
    {
        public abstract Type Type { get; }
    }
}
