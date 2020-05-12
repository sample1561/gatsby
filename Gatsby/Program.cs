using System;
using System.Linq;
using Gatsby.Analysis;
using Gatsby.Analysis.Semantic;
using Gatsby.Analysis.Syntax;
using Gatsby.Analysis.Syntax.Lexer;
using Gatsby.Analysis.Syntax.Parser;
using Gatsby.Analysis.Syntax.Tree;

namespace Gatsby
{
    internal static class Program
    {        
        private static void Main()
        {
            var showTree = false;
            var showToken = false;

            while (true)
            {
                Console.Write(">> ");
                var line = Console.ReadLine();
                
                if (string.IsNullOrWhiteSpace(line))
                    return;

                if (line == "#tree")
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing parse trees." : "Not showing parse trees");
                    continue;
                }
                
                if (line == "#stream" || line == "#token")
                {
                    showToken = !showToken;
                    Console.WriteLine(showToken ? "Showing Tokenization" : "Not showing Tokenization");
                    continue;
                }
                
                if (line == "#clear")
                {
                    Console.Clear();
                    continue;
                }

                var syntaxTree = SyntaxTree.Parse(line);
                var compilation = new Compilation(syntaxTree);
                var result = compilation.Evaluate();
                
                var diagnostics = result.Diagnostics;

                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;                
                    PrettyPrinter(syntaxTree.Root);
                    Console.ResetColor();
                }
                
                if (showToken)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;                
                    var parser = new Parser(line);
                    parser.PrintTokenization();
                    Console.ResetColor();
                }

                if (!diagnostics.Any())
                {
                    Console.WriteLine(result.Value);
                }
                
                else
                {
                    foreach (var diagnostic in diagnostics)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();
                        
                        var prefix = line.Substring(0, diagnostic.Span.Start);
                        var error = line.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
                        var suffix = line.Substring(diagnostic.Span.End);
                        
                        Console.Write("    ");
                        Console.Write(prefix);
                        
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(error);
                        Console.ResetColor();

                        Console.Write(suffix);
                        Console.WriteLine();
                    }
                    
                    Console.WriteLine();
                }
            }
        }

        private static void PrettyPrinter(SyntaxNode node, string indent = "", bool isLastChild = true)
        {
            var marker = isLastChild ? "└──" : "├──";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);

            if (node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();
            
            indent += isLastChild ? "   " : "│  ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                PrettyPrinter(child, indent, child == lastChild);
            }
        }
    }
}
