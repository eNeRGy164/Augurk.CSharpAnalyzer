using System;
using System.Collections.Generic;
using Augurk.CSharpAnalyzer.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Augurk.CSharpAnalyzer.Collectors
{
    public class DefaultInvocationTreeCollector : IInvocationTreeCollector
    {
        private readonly List<MethodWrapper> _invocations = new List<MethodWrapper>();
        private readonly Dictionary<IMethodSymbol, MethodWrapper> _wrappers = new Dictionary<IMethodSymbol, MethodWrapper>(); 
        private readonly Stack<MethodWrapper> _currentStack = new Stack<MethodWrapper>();

        public bool IsAlreadyCollected(IMethodSymbol method)
        {
            //SyntaxReference syntax = method.GetComparableSyntax();
            //if(syntax == null)
            //{
            //    return false;
            //}

            return _wrappers.ContainsKey(method);
        }

        /// <summary>
        /// Called when a method is being stepped into.
        /// </summary>
        /// <param name="method">An <see cref="IMethodSymbol"/> describing the method that is being stepped into.</param>
        public void StepInto(IMethodSymbol method)
        {
            _currentStack.Push(Step(method));
        }

        /// <summary>
        /// Called when a method is being stepped out of.
        /// </summary>
        public void StepOut()
        {
            _currentStack.Pop();
        }

        /// <summary>
        /// Called when a method is being stepped over.
        /// </summary>
        /// <param name="method">An <see cref="IMethodSymbol"/> describing the method that is being stepped over.</param>
        public void StepOver(IMethodSymbol method)
        {
            Step(method);
        }

        private MethodWrapper Step(IMethodSymbol method)
        {
            //SyntaxReference syntax = method.GetComparableSyntax();

            // Try to find a wrapper; otherwise, create one
            MethodWrapper wrapper;
            if (!_wrappers.TryGetValue(method, out wrapper))
            {
                wrapper = new MethodWrapper(method);
                _wrappers.Add(method, wrapper);
            }

            if (_currentStack.Count == 0)
            {
                _invocations.Add(wrapper);
            }
            else
            {
                _currentStack.Peek().Invocations.Add(wrapper);
            }

            return wrapper;
        }

        private class MethodWrapper
        {
            private IMethodSymbol _method;
            public List<MethodWrapper> Invocations { get; } = new List<MethodWrapper>(); 

            public MethodWrapper(IMethodSymbol method)
            {
                _method = method;
            }
        }
    }
}
