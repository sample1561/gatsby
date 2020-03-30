﻿using System;
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
            var showToken = true;

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
                var binder = new SemanticBinder();
                
                //Convert the parse tree to a semantic tree 
                var abstractExpression = binder.SemanticExpression(syntaxTree.Root);

                var diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();

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
                    var e = new Evaluator(abstractExpression);
                    var result = e.Evaluate();
                    Console.WriteLine(result);
                }
                
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    foreach (var diagnostic in diagnostics)
                        Console.WriteLine(diagnostic);

                    Console.ResetColor();
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
            
            indent += isLastChild ? "   " : "│   ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                PrettyPrinter(child, indent, child == lastChild);
            }
        }
    }
}
