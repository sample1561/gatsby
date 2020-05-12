using System;
using System.Collections.Generic;
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

        public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables)
        {
            var binder = new SemanticBinder(variables);
            var boundExpression = binder.SemanticExpression(Syntax.Root);

            var diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).ToArray();

            if (diagnostics.Any())
                return new EvaluationResult(diagnostics,null);

            var evaluator = new Evaluator(boundExpression, variables);
            var value = evaluator.Evaluate();
            
            return new EvaluationResult(Array.Empty<Diagnostic>(),value);
        }
    }
}