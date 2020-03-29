using System.Collections.Generic;

namespace Gatsby.CodeAnalysis.Syntax
{
    public abstract class SyntaxNode
    {
        public abstract TokenType Kind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}