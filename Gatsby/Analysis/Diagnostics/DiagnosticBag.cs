using System;
using System.Collections;
using System.Collections.Generic;
using Gatsby.Analysis.Syntax.Lexer;

namespace Gatsby.Analysis.Diagnostics
{
    internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();
        
        private void Report(TextSpan span, string message)
        {
            var diagnostic = new Diagnostic(span, message);
            _diagnostics.Add(diagnostic);
        }
        
        public void AddRange(DiagnosticBag lexerDiagnostics)
        {
            _diagnostics.AddRange(lexerDiagnostics._diagnostics);
        }

        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void ReportInvalidNumber(TextSpan textSpan, string text, Type type)
        {
            var message = $"The number {text} isn't valid {type}";
            Report(textSpan, message);
        }

        public void ReportBadCharacter(int position, char current)
        {
            var span = new TextSpan(position, 1);
            var message = $"Unrecognized Input: '{current}'";
            Report(span ,message);
        }
        
        public void ReportUnexpectedToken(TextSpan span, TokenType actual, TokenType expected)
        {
            var message = $"Unexpected token <{actual}>, expected <{expected}>";
            Report(span, message);
        }

        public void ReportUndefinedUnaryOperator(TextSpan span, string text, Type boundOperandType)
        {
            var message =
                $"Unrecognized Unary operator '{text}' is not defined for type {boundOperandType}.";
            Report(span, message);
        }

        public void ReportUndefinedBinaryOperator(TextSpan span, string text, Type left, Type right)
        {
            var message = $"Binary operator '{text}' is not defined for types {left} and {right}.";
            Report(span, message);
        }
    }
}