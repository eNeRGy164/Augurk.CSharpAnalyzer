using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Augurk.CSharpAnalyzer.Analyzers
{
    public struct InvokedMethod
    {
        public InvokedMethod(IMethodSymbol method, TypeInfo? targetType, IEnumerable<TypeInfo?> argumentTypes, SyntaxReference declaringSyntaxReference)
        {
            this.Method = method;
            this.TargetType = targetType;
            this.ArgumentTypes = argumentTypes;
            this.DeclaringSyntaxReference = declaringSyntaxReference;
        }

        public IMethodSymbol Method { get; private set; }
        public TypeInfo? TargetType { get; private set; }
        public IEnumerable<TypeInfo?> ArgumentTypes { get; private set; }
        public SyntaxReference DeclaringSyntaxReference { get; private set; }
    }
}
