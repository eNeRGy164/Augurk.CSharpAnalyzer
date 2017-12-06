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
using Augurk.CSharpAnalyzer.Analyzers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CodeAnalysis.CSharp.Syntax
{
    /// <summary>
    /// Contains extension methods for the <see cref="InvocationExpressionSyntax"/> type.
    /// </summary>
    internal static class InvocationExpressionSyntaxExtensions
    {
        /// <summary>
        /// Gets the type on which the method invocation of <paramref name="expression"/> is being performed.
        /// </summary>
        /// <param name="expression">An <see cref="InvocationExpressionSyntax"/> representing the invocation.</param>
        /// <param name="model">A <see cref="SemanticModel"/> for the project used to get type information.</param>
        /// <returns>Returns a <see cref="TypeInfo"/> instance, or <c>null</c> if no type information could be determined.</returns>
        public static TypeInfo? GetTargetType(this InvocationExpressionSyntax expression, SemanticModel model)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            TypeInfo? result = null;
            var memberAccessExpressionSyntax = expression.Expression as MemberAccessExpressionSyntax;
            if (memberAccessExpressionSyntax != null)
            {
                result = model.GetTypeInfo(memberAccessExpressionSyntax.Expression);
            }

            return result;
        }

        /// <summary>
        /// Gets the names and types of the arguments passed in the invocation.
        /// </summary>
        /// <param name="expression">An <see cref="InvocationExpressionSyntax"/> representing the invocation.</param>
        /// <param name="symbol">An <see cref="IMethodSymbol"/> representing the method being invoked.</param>
        /// <param name="model">A <see cref="SemanticModel"/> for the project used to get type information.</param>
        /// <param name="context">An <see cref="InvokedMethod"/> representing the context in which the provided <paramref name="symbol"/> is invoked.</param>
        /// <returns>Returns a range of tuples containing the names and types of the arguments passed in the invocation.</returns>
        public static IEnumerable<TypeInfo?> GetArgumentTypes(this InvocationExpressionSyntax expression, IMethodSymbol symbol, SemanticModel model, InvokedMethod context)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            if (expression.ArgumentList == null)
            {
                yield break;
            }

            for (int i = 0; i < expression.ArgumentList.Arguments.Count; i++)
            {
                TypeInfo? argumentType = null;
                var argument = expression.ArgumentList.Arguments[i].ChildNodes().FirstOrDefault();
                if (argument != null)
                {
                    SymbolInfo symbolInfo = model.GetSymbolInfo(argument);
                    if (symbolInfo.Symbol?.Kind == SymbolKind.Parameter)
                    {
                        var parameter = context.Method.Parameters.FirstOrDefault(p => p.Name == symbolInfo.Symbol.Name);
                        var parameterIndex = context.Method.Parameters.IndexOf(parameter);
                        argumentType = context.ArgumentTypes.ElementAt(parameterIndex);
                    }
                    else
                    {
                        argumentType = model.GetTypeInfo(argument);
                    }
                }

                yield return argumentType;
            }
        }

        /// <summary>
        /// Gets the target of the provided <paramref name="expression"/> using the provided <paramref name="symbol"/> and <paramref name="model"/>.
        /// </summary>
        /// <param name="expression">An <see cref="InvocationExpressionSyntax"/> instance describing the invocation of a particular method.</param>
        /// <param name="symbol">An <see cref="IMethodSymbol"/> describing the method being invoked by the <paramref name="expression"/>.</param>
        /// <param name="model">A <see cref="SemanticModel"/> that can be used to resolve type information.</param>
        /// <param name="context">An <see cref="InvokedMethod"/> that represents the context in which to determine the target of the invocation.</param>
        /// <returns>Returns a <see cref="TypeInfo"/> instance representing the type of the target being invoked, or <c>null</c> if the type could not be determined.</returns>
        public static TypeInfo? GetTargetOfInvocation(this InvocationExpressionSyntax expression, IMethodSymbol symbol, SemanticModel model, InvokedMethod context)
        {
            TypeInfo? targetTypeInfo = expression.Expression.Kind() == SyntaxKind.IdentifierName ? context.TargetType : expression.GetTargetType(model);
            if (targetTypeInfo.HasValue)
            {
                MemberAccessExpressionSyntax memberAccess = expression.Expression as MemberAccessExpressionSyntax;
                IdentifierNameSyntax identifier = memberAccess?.Expression as IdentifierNameSyntax ?? expression.Expression as IdentifierNameSyntax;
                if (identifier != null)
                {
                    SymbolInfo identifierSymbol = model.GetSymbolInfo(identifier);
                    switch (identifierSymbol.Symbol.Kind)
                    {
                        case SymbolKind.Local:
                            VariableDeclaratorSyntax syntax = identifierSymbol.Symbol.GetComparableSyntax()?.GetSyntax() as VariableDeclaratorSyntax;
                            if (syntax != null && syntax.ChildNodes().Any())
                            {
                                targetTypeInfo = model.GetTypeInfo(syntax.ChildNodes().First().ChildNodes().First());
                            }
                            break;
                        case SymbolKind.Parameter:
                            if (context.Method != null)
                            {
                                var parameter = context.Method.Parameters.FirstOrDefault(p => p.Name == identifierSymbol.Symbol.Name);
                                var parameterIndex = context.Method.Parameters.IndexOf(parameter);
                                targetTypeInfo = context.ArgumentTypes.ElementAt(parameterIndex);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return targetTypeInfo;
        }
    }
}
