/*
 Copyright 2017, Augurk
 
 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at
 
 http://www.apache.org/licenses/LICENSE-2.0
 
 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
*/

using System.Collections.Generic;
using Microsoft.CodeAnalysis;

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
