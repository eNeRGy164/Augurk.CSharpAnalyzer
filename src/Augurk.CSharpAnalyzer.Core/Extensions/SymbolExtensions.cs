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

namespace Microsoft.CodeAnalysis
{
    /// <summary>
    /// Contains extension methods for the <see cref="ISymbol"/> interface.
    /// </summary>
    public static class SymbolExtensions
    {
        /// <summary>
        /// Gets the comparable sytax for the provided symbol.
        /// </summary>
        /// <param name="symbol">
        /// The <see cref="IMethodSymbol"/> instance for which the comparable syntax should be retrieved.
        /// </param>
        /// <returns>
        /// The <see cref="SyntaxReference"/> for the provided symbol if available; otherwise, <c>null</c>
        /// </returns>
        public static SyntaxReference GetComparableSyntax(this ISymbol symbol)
        {
            if (symbol == null || symbol.DeclaringSyntaxReferences.Length == 0)
            {
                return null;
            }

            if (symbol.DeclaringSyntaxReferences.Length > 1)
            {
                throw new NotSupportedException($"Symbol \"{symbol}\" has more than one (1) declaration. " +
                                                $"The Augurk CSharpAnalyzer was not designed to handle this scenario.");
            }

            return symbol.DeclaringSyntaxReferences[0];
        }
    }
}
