using Augurk.CSharpAnalyzer.Collectors;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Augurk.CSharpAnalyzer.Specifications.Support
{
    internal class TestInvocationTreeCollector : IInvocationTreeCollector
    {
        private readonly List<IMethodSymbol> methods = new List<IMethodSymbol>();

        public void StepInto(IMethodSymbol method)
        {
            methods.Add(method);
        }

        public void StepOut()
        {
            // Nothing useful to do here
        }

        public void StepOver(IMethodSymbol method)
        {
            methods.Add(method);
        }
    }
}
