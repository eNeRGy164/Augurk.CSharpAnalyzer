using System;
using Microsoft.CodeAnalysis;
using Oakton;

namespace Augurk.CSharpAnalyzer.Collectors
{
    public class ConsoleStrackTraceCollector : IStackTraceCollector
    {
        private int indent;

        public void StepInto(IMethodSymbol method)
        {
            ConsoleWriter.WriteWithIndent(method.IsOverride ? ConsoleColor.Green : ConsoleColor.White, indent, $"{method.ToString()}");
            indent++;
        }

        public void StepOut()
        {
            indent--;
        }

        public void StepOver(IMethodSymbol method)
        {
            ConsoleWriter.WriteWithIndent(ConsoleColor.DarkGray, indent, $"{method.ToString()}");
        }
    }
}
