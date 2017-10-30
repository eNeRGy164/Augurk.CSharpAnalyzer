using Microsoft.CodeAnalysis;

namespace Augurk.CSharpAnalyzer.Collectors
{
    public interface IStackTraceCollector
    {
        void StepInto(IMethodSymbol method);
        void StepOut();
        void StepOver(IMethodSymbol method);
    }
}
