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
using Augurk.CSharpAnalyzer.Options;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oakton;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Augurk.CSharpAnalyzer.Commands
{
    /// <summary>
    /// An <see cref="OaktonCommand{T}"/> that implements the analyze command.
    /// </summary>
    [Description("Analyze a solution")]
    public class AnalyzeCommand : OaktonAsyncCommand<AnalyzeOptions>
    {
        public Func<string, Task<Workspace>> GetWorkspaceFunc { get; set; }

        /// <summary>
        /// Called when the command is being executed.
        /// </summary>
        /// <param name="input">An <see cref="AnalyzeOptions"/> containing the options passed to the command.</param>
        /// <returns>Returns <c>true</c> if the analysis was succesful, otherwise <c>false</c>.</returns>
        public override async Task<bool> Execute(AnalyzeOptions input)
        {
            try
            {
                ConsoleWriter.Write(ConsoleColor.White, $"Starting analysis for solution {input.Solution}");
                JToken result = await Analyze(input);

                string filename = Path.Combine(Path.GetDirectoryName(input.Solution), $"{Path.GetFileNameWithoutExtension(input.Solution)}.aar");
                using (FileStream fs = File.Open(filename, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonTextWriter writer = new JsonTextWriter(sw))
                {
                    result.WriteTo(writer);
                }

                ConsoleWriter.Write(ConsoleColor.White, $"Analysis succesful for solution {input.Solution}");
                return true;
            }
            catch (Exception ex)
            {
                ConsoleWriter.Write(ConsoleColor.Red, $"Analysis failed for solution {input.Solution}");
                ConsoleWriter.Write(ConsoleColor.White, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Performs the actual analysis.
        /// </summary>
        /// <param name="options">Options passed to the analyze command.</param>
        public async Task<JToken> Analyze(AnalyzeOptions options)
        {
            // Check if the provided solution path is an absolute path
            string solutionPath = options.Solution;
            if (!Path.IsPathRooted(solutionPath))
            {
                solutionPath = Path.Combine(Environment.CurrentDirectory, solutionPath);
            }

            // Load the solution into an adhoc workspace
            Workspace workspace = await this.GetWorkspaceFunc(solutionPath);

            // Define lazies for the compilation of each project
            var projects = new Dictionary<Project, Lazy<Compilation>>();
            foreach (var project in workspace.CurrentSolution.Projects)
            {
                projects.Add(project, new Lazy<Compilation>(() => project.GetCompilationAsync().Result));
            }

            // Find the specifications project
            var specProject = workspace.CurrentSolution.Projects.FirstOrDefault(p => p.Name == options.SpecificationsProject);
            var compilation = projects[specProject].Value;

            // Make sure that the project compiled succesfully
            var diagnostics = compilation.GetDiagnostics();
            if (diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error))
            {
                // Errors occured during compilation, we cannot continue here
                ConsoleWriter.Write(ConsoleColor.Red, $"The following errors occured during compilation of solution '{solutionPath}'");
                foreach (var diagnostic in diagnostics)
                {
                    ConsoleWriter.WriteWithIndent(ConsoleColor.Red, 1, diagnostic.ToString());
                }

                throw new InvalidOperationException("Unable to analyze solution due to compile errors.");
            }

            // Build the analysis context and go through each syntax tree
            var context = new AnalyzeContext(projects, options);
            foreach (var tree in compilation.SyntaxTrees)
            {
                // Find entry-points 
                var visitor = new EntryPointAnalyzer(context, compilation.GetSemanticModel(tree));
                visitor.Visit(tree.GetRoot());
            }

            // Build up a report in Json format
            var analysisReport = new JObject();

            // Add the solution name. As Augurk is unaware of language, 
            // it only refers to project in the broadest sense of the word
            analysisReport.Add("AnalyzedProject", Path.GetFileName(options.Solution));

            analysisReport.Add("Timestamp", DateTime.UtcNow);

            // Add the actual results from this analysis
            analysisReport.Add("RootInvocations", context.Collector.GetJsonOutput());

            // Return the report
            return analysisReport;
        }
    }
}
