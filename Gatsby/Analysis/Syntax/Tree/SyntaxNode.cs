using System.Collections.Generic;
using Gatsby.Analysis.Syntax.Lexer;

namespace Gatsby.Analysis.Syntax.Tree
{
    public abstract class SyntaxNode
    {
        public abstract TokenType Kind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}