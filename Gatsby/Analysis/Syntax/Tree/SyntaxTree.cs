using System.Collections.Generic;
using System.Linq;
using Gatsby.Analysis.Syntax.Expression;
using Gatsby.Analysis.Syntax.Lexer;

namespace Gatsby.Analysis.Syntax.Tree
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(ExpressionSyntax root, SyntaxToken endOfFileToken,
            IEnumerable<string> diagnostics)
        {
            Root = root;
            EndOfFileToken = endOfFileToken;
            Diagnostics = diagnostics.ToArray();
        }

        public IReadOnlyList<string> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public static SyntaxTree Parse(string text)
        {
            var parser = new Parser.Parser(text);
            return parser.Parse();
        }
    }
}