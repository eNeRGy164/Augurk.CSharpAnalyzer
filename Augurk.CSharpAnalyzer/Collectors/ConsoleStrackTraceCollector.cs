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
using Microsoft.CodeAnalysis;
using Oakton;

namespace Augurk.CSharpAnalyzer.Collectors
{
    /// <summary>
    /// An <see cref="IStackTraceCollector"/> implementation that writes the output to the console.
    /// </summary>
    public class ConsoleStackTraceCollector : IStackTraceCollector
    {
        private int indent;

        /// <summary>
        /// Called when a method is being stepped into.
        /// </summary>
        /// <param name="method">Method that is being stepped into.</param>
        public void StepInto(IMethodSymbol method)
        {
            ConsoleWriter.WriteWithIndent(method.IsOverride ? ConsoleColor.Green : ConsoleColor.White, indent, $"{method.ToString()}");
            indent++;
        }

        /// <summary>
        /// Called when a method is being stepped out of.
        /// </summary>
        public void StepOut()
        {
            indent--;
        }

        /// <summary>
        /// Called when a method is being stepped over.
        /// </summary>
        /// <param name="method">Method that is being stepped over.</param>
        public void StepOver(IMethodSymbol method)
        {
            ConsoleWriter.WriteWithIndent(ConsoleColor.DarkGray, indent, $"{method.ToString()}");
        }
    }
}
