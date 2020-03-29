using System.Collections.Generic;

namespace Gatsby.Analysis.Syntax
{
    public abstract class SyntaxNode
    {
        public abstract TokenType Kind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}