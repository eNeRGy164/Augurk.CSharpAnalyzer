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
        /// <returns>Returns a range of tuples containing the names and types of the arguments passed in the invocation.</returns>
        public static IEnumerable<TypeInfo?> GetArgumentTypes(this InvocationExpressionSyntax expression, IMethodSymbol symbol, SemanticModel model)
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
                    argumentType = model.GetTypeInfo(argument);
                }

                yield return argumentType;
            }
        }
    }
}
