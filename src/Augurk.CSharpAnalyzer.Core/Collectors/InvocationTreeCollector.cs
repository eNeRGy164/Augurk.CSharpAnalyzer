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
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;
using Augurk.CSharpAnalyzer.Analyzers;
using System;

namespace Augurk.CSharpAnalyzer.Collectors
{
    public class InvocationTreeCollector
    {
        private readonly List<MethodWrapper> _invocations = new List<MethodWrapper>();
        private readonly Dictionary<IMethodSymbol, MethodWrapper> _wrappers = new Dictionary<IMethodSymbol, MethodWrapper>();
        private readonly Dictionary<IMethodSymbol, Dictionary<ITypeSymbol, MethodWrapper>> _extensionMethodWrappers = new Dictionary<IMethodSymbol, Dictionary<ITypeSymbol, MethodWrapper>>();
        private readonly Stack<MethodWrapper> _currentStack = new Stack<MethodWrapper>();
        private readonly AnalyzeContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationTreeCollector"/> class.
        /// </summary>
        /// <param name="context">The <see cref="AnalyzeContext"/> in which the analysis is being run.</param>
        public InvocationTreeCollector(AnalyzeContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Determines if the provided <paramref name="method"/> has previously been collected, and thus can be stepped over.
        /// </summary>
        /// <param name="method">An <see cref="IMethodSymbol"/> instance that describes the method to check for.</param>
        /// <returns>Returns <c>true</c> if the method has previously been collected, otherwise <c>false</c>.</returns>
        public bool IsAlreadyCollected(IMethodSymbol method)
        {
            MethodWrapper wrapper;
            if (_wrappers.TryGetValue(method, out wrapper))
            {
                return _currentStack.Contains(wrapper);
            }

            return false;
        }
        
        /// <summary>
        /// Determines if the provided <paramref name="method"/> has previously been collected as an extension method on <paramref name="targetType"/>.
        /// </summary>
        /// <param name="method">An <see cref="IMethodSymbol"/> representing the extension method to check for.</param>
        /// <param name="targetType">An <see cref="ITypeSymbol"/> representing the concrete type on which the extension method is being invoked.</param>
        /// <returns>Returns <c>true</c> if the extension method for the provided <paramref name="targetType"/> has previously been collected, otherwise <c>false</c>.</returns>
        public bool IsExtensionMethodAlreadyCollected(IMethodSymbol method, ITypeSymbol targetType)
        {
            if (!method.IsExtensionMethod)
            {
                throw new ArgumentException("Provided method is not an extension method.", nameof(method));
            }

            Dictionary<ITypeSymbol, MethodWrapper> wrappers;
            if (!_extensionMethodWrappers.TryGetValue(method, out wrappers))
            {
                return false;
            }

            return wrappers.ContainsKey(targetType);
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
        /// Called when an extension method is being stepped into.
        /// </summary>
        /// <param name="method">An <see cref="IMethodSymbol"/> describing the extension method that is being stepped into.</param>
        /// <param name="targetType">An <see cref="ITypeSymbol"/> representing the concrete type on which the extension method is invoked.</param>
        public void StepIntoExtensionMethod(IMethodSymbol method, ITypeSymbol targetType)
        {
            if (!method.IsExtensionMethod)
            {
                throw new ArgumentException("Provided method is not an extension method.", nameof(method));
            }

            _currentStack.Push(StepExtensionMethod(method, targetType));
        }

        /// <summary>
        /// Called when a method is being stepped over.
        /// </summary>
        /// <param name="method">An <see cref="IMethodSymbol"/> describing the method that is being stepped over.</param>
        public void StepOver(IMethodSymbol method)
        {
            Step(method);
        }

        /// <summary>
        /// Called when a method is being stepped over.
        /// </summary>
        /// <param name="method">An <see cref="IMethodSymbol"/> describing the method that is being stepped over.</param>
        /// <param name="targetType">An <see cref="ITypeSymbol"/> representing the concrete type on which the extension method is being invoked.</param>
        public void StepOverExtensionMethod(IMethodSymbol method, ITypeSymbol targetType)
        {
            if (!method.IsExtensionMethod)
            {
                throw new ArgumentException("Provided method is not an extension method.", nameof(method));
            }

            StepExtensionMethod(method, targetType);
        }

        /// <summary>
        /// Called when a method is being stepped out of.
        /// </summary>
        public void StepOut()
        {
            MethodWrapper wrapper = _currentStack.Pop();
            _wrappers.Remove(wrapper.Method);
        }

        public JToken GetJsonOutput()
        {
            return GetJsonOutput(_invocations);
        }

        private JToken GetJsonOutput(IEnumerable<MethodWrapper> invocations, Stack<MethodWrapper> invocationStack = null)
        {
            var rootInvocations = new JArray();
            if (invocationStack == null)
            {
                invocationStack = new Stack<MethodWrapper>();
            }

            foreach (var invocation in invocations)
            {
                var jInvocation = new JObject();

                var method = invocation.Method;

                var kind = invocation.RegularExpressions.Length > 0 ?
                           "When" :
                           method.DeclaredAccessibility.ToString();

                jInvocation.Add("Kind", kind);
                jInvocation.Add("Signature", $"{method.ToDisplayString()}, {method.ContainingAssembly.Name}");

                if (invocation.RegularExpressions.Length > 0)
                {
                    jInvocation.Add("RegularExpressions", new JArray(invocation.RegularExpressions));
                }

                SyntaxReference methodDeclaration = method.GetComparableSyntax();
                if (methodDeclaration != null && _context.SpecifcationsProject.GetDocument(methodDeclaration.SyntaxTree) == null)
                {
                    jInvocation.Add("Local", true);
                }

                if (!invocationStack.Contains(invocation))
                {
                    invocationStack.Push(invocation);
                    jInvocation.Add("Invocations", GetJsonOutput(invocation.Invocations, invocationStack));
                    invocationStack.Pop();
                }
                else
                {
                    jInvocation.Add("Note", "Recursive");
                }

                rootInvocations.Add(jInvocation);
            }

            return rootInvocations;
        }

        private MethodWrapper Step(IMethodSymbol method)
        {
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

        private MethodWrapper StepExtensionMethod(IMethodSymbol method, ITypeSymbol targetType)
        {
            // Try to find a wrapper; otherwise, create one
            Dictionary<ITypeSymbol, MethodWrapper> wrappers;
            if (!_extensionMethodWrappers.TryGetValue(method, out wrappers))
            {
                wrappers = new Dictionary<ITypeSymbol, MethodWrapper>();
                _extensionMethodWrappers.Add(method, wrappers);
            }

            MethodWrapper wrapper;
            if (!wrappers.TryGetValue(targetType, out wrapper))
            {
                wrapper = new MethodWrapper(method);
                wrappers.Add(targetType, wrapper);
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
            public IMethodSymbol Method { get; }

            public List<MethodWrapper> Invocations { get; } = new List<MethodWrapper>(); 

            public ImmutableArray<string> RegularExpressions { get; }

            public MethodWrapper(IMethodSymbol method)
            {
                Method = method;

                RegularExpressions = GetRegularExpressions();
            }

            private ImmutableArray<string> GetRegularExpressions()
            {
                return Method.GetAttributes()
                           .Where(attribute => attribute.AttributeClass.Name.EndsWith("WhenAttribute")
                                            && (attribute.ConstructorArguments.Length > 0
                                            || attribute.NamedArguments.Any(na => na.Key == "Regex")))
                           .Select(attribute => 
                                attribute.NamedArguments.FirstOrDefault(na => na.Key =="Regex").Value.Value?.ToString() ?? 
                                attribute.ConstructorArguments.First().Value.ToString())
                           .ToImmutableArray();
            }
        }
    }
}
