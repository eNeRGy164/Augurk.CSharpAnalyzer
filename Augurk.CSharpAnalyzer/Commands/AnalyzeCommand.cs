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
    [Description("Analyze a solution")]
    public class AnalyzeCommand : OaktonCommand<AnalyzeOptions>
    {
        public override bool Execute(AnalyzeOptions input)
        {
            try
            {
                ConsoleWriter.Write(ConsoleColor.White, $"Starting analysis for solution {input.Solution}");
                Analyze(input).Wait();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task Analyze(AnalyzeOptions options)
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = await workspace.OpenSolutionAsync(options.Solution);
            var projects = new Dictionary<Project, Lazy<Compilation>>();

            foreach (var project in solution.Projects)
            {
                projects.Add(project, new Lazy<Compilation>(() => project.GetCompilationAsync().Result));
            }

            var specProject = solution.Projects.FirstOrDefault(p => p.Name == options.SpecificationsProject);
            var compilation = projects[specProject].Value;

            var context = new AnalyzeContext(projects, new ConsoleStrackTraceCollector(), options);
            foreach (var tree in compilation.SyntaxTrees)
            {
                var visitor = new EntryPointAnalyzer(context, compilation.GetSemanticModel(tree));
                visitor.Visit(tree.GetRoot());
            }
        }
    }
}
