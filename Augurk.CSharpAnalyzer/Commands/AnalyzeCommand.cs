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
using Augurk.CSharpAnalyzer.Collectors;
using Augurk.CSharpAnalyzer.Options;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Oakton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Augurk.CSharpAnalyzer.Commands
{
    /// <summary>
    /// An <see cref="OaktonCommand{T}"/> that implements the analyze command.
    /// </summary>
    [Description("Analyze a solution")]
    public class AnalyzeCommand : OaktonCommand<AnalyzeOptions>
    {
        /// <summary>
        /// Called when the command is being executed.
        /// </summary>
        /// <param name="input">An <see cref="AnalyzeOptions"/> containing the options passed to the command.</param>
        /// <returns>Returns <c>true</c> if the analysis was succesful, otherwise <c>false</c>.</returns>
        public override bool Execute(AnalyzeOptions input)
        {
            try
            {
                ConsoleWriter.Write(ConsoleColor.White, $"Starting analysis for solution {input.Solution}");
                Analyze(input).Wait();
                ConsoleWriter.Write(ConsoleColor.White, $"Analysis succesful for solution {input.Solution}");
                return true;
            }
            catch (Exception)
            {
                ConsoleWriter.Write(ConsoleColor.Red, $"Analysis failed for solution {input.Solution}");
                return false;
            }
        }

        /// <summary>
        /// Performs the actual analysis.
        /// </summary>
        /// <param name="options">Options passed to the analyze command.</param>
        private async Task Analyze(AnalyzeOptions options)
        {
            // Load the solution
            var workspace = MSBuildWorkspace.Create();
            var solution = await workspace.OpenSolutionAsync(options.Solution);
            var projects = new Dictionary<Project, Lazy<Compilation>>();

            // Define lazies for the compilation of each project
            foreach (var project in solution.Projects)
            {
                projects.Add(project, new Lazy<Compilation>(() => project.GetCompilationAsync().Result));
            }

            // Find the specifications project
            var specProject = solution.Projects.FirstOrDefault(p => p.Name == options.SpecificationsProject);
            var compilation = projects[specProject].Value;

            // Build the analysis context and go through each syntax tree
            var context = new AnalyzeContext(projects, new ConsoleStrackTraceCollector(), options);
            foreach (var tree in compilation.SyntaxTrees)
            {
                // Find entry-points 
                var visitor = new EntryPointAnalyzer(context, compilation.GetSemanticModel(tree));
                visitor.Visit(tree.GetRoot());
            }
        }
    }
}
