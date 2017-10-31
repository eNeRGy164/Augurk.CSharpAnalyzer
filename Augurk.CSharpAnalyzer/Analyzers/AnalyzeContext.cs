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
using Augurk.CSharpAnalyzer.Collectors;
using Augurk.CSharpAnalyzer.Options;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Augurk.CSharpAnalyzer.Analyzers
{
    /// <summary>
    /// Wraps all the information required during an analysis.
    /// </summary>
    public class AnalyzeContext
    {
        /// <summary>
        /// Initializes a new <see cref="AnalyzeContext"/> instance.
        /// </summary>
        /// <param name="projects">A dictionary of projects and their compilation for easy access.</param>
        /// <param name="options">The <see cref="AnalyzeOptions"/> that were passed to the command line.</param>
        public AnalyzeContext(IDictionary<Project, Lazy<Compilation>> projects, AnalyzeOptions options)
        {
            this.Projects = projects;
            this.Collector = new InvocationTreeCollector(this);
            this.Options = options;

            this.SpecifcationsProject = projects.FirstOrDefault(p => p.Key.Name == options.SpecificationsProject).Key;
            if (this.SpecifcationsProject == null)
            {
                throw new ArgumentException($"Project {options.SpecificationsProject} not found in solution.");
            }
        }

        /// <summary>
        /// A dictionary where the key is the project and the value is a <see cref="Lazy{T}"/> of the compilation for that project.
        /// </summary>
        public IDictionary<Project, Lazy<Compilation>> Projects { get; private set; }
        /// <summary>
        /// An <see cref="IInvocationTreeCollector"/> implementation that collects the chain of calls.
        /// </summary>
        public InvocationTreeCollector Collector { get; private set; }
        /// <summary>
        /// An <see cref="AnalyzeOptions"/> instance containing the options that were passed to the command line.
        /// </summary>
        public AnalyzeOptions Options { get; private set; }

        /// <summary>
        /// Gets the <see cref="Project"/> representing the system under test.
        /// </summary>
        public Project SpecifcationsProject { get; private set; }
    }
}
