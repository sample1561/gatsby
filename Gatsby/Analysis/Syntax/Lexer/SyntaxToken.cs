using System.Collections.Generic;
using System.Linq;
using Gatsby.Analysis.Syntax.Tree;

namespace Gatsby.Analysis.Syntax.Lexer
{
    public sealed class SyntaxToken : SyntaxNode
    {
        public SyntaxToken(TokenType kind, int position, string text, object value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public override TokenType Kind { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}