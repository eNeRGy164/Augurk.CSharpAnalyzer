using Microsoft.CodeAnalysis;

namespace ConsoleApplication1
{
    public interface IStackTraceCollector
    {
        void StepInto(IMethodSymbol method);
        void StepOut();
        void StepOver(IMethodSymbol method);
    }
}
