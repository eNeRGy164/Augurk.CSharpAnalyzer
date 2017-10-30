using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System.Threading.Tasks;

namespace Augurk.CSharpAnalyzer
{
    class Program
    {
        public static readonly Dictionary<Project, Lazy<Compilation>> Projects = new Dictionary<Project, Lazy<Compilation>>();

        static void Main(string[] args)
        {
            PrivateMain(args).Wait();
        }

        private static async Task PrivateMain(string[] args)
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = await workspace.OpenSolutionAsync(args[0]);

            foreach (var project in solution.Projects)
            {
                Projects.Add(project, new Lazy<Compilation>(() => project.GetCompilationAsync().Result));
            }

            var specProject = solution.Projects.FirstOrDefault(p => p.Name == "Specifications");
            var compilation = Projects[specProject].Value;

            IStackTraceCollector collector = new ConsoleStrackTraceCollector();
            foreach (var tree in compilation.SyntaxTrees)
            {
                var visitor = new EntryPointFinder(compilation.GetSemanticModel(tree), collector);
                visitor.Visit(tree.GetRoot());
            }


            Console.ReadLine();
        }
    }
}
