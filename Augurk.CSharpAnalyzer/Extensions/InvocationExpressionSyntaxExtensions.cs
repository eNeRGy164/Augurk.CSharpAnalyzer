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
    }
}
