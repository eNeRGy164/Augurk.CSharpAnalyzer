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
using Microsoft.CodeAnalysis;

namespace Augurk.CSharpAnalyzer.Collectors
{
    /// <summary>
    /// Describes the interface for an invocation tree collector.
    /// </summary>
    public interface IInvocationTreeCollector
    {
        /// <summary>
        /// Called when a method is being stepped into.
        /// </summary>
        /// <param name="method">An <see cref="IMethodSymbol"/> describing the method that is being stepped into.</param>
        void StepInto(IMethodSymbol method);
        /// <summary>
        /// Called when a method is being stepped out of.
        /// </summary>
        void StepOut();
        /// <summary>
        /// Called when a method is being stepped over.
        /// </summary>
        /// <param name="method">An <see cref="IMethodSymbol"/> describing the method that is being stepped over.</param>
        void StepOver(IMethodSymbol method);
    }
}
