using System;
using System.Linq;
using Gatsby.Analysis.Diagnostics;
using Gatsby.Analysis.Semantic;
using Gatsby.Analysis.Syntax.Tree;

namespace Gatsby.Analysis
{
    public class Compilation
    {
        public SyntaxTree Syntax { get;  }

        public Compilation(SyntaxTree syntax)
        {
            Syntax = syntax;
        }

        public EvaluationResult Evaluate()
        {
            var binder = new SemanticBinder();
            var boundExpression = binder.SemanticExpression(Syntax.Root);

            var diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).ToArray();

            if (diagnostics.Any())
                return new EvaluationResult(diagnostics,null);

            var evaluator = new Evaluator(boundExpression);
            var value = evaluator.Evaluate();
            
            return new EvaluationResult(Array.Empty<Diagnostic>(),value);
        }
    }
}