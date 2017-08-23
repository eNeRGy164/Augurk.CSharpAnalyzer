using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.MSBuild;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static readonly Dictionary<Project, Lazy<Compilation>> Projects = new Dictionary<Project, Lazy<Compilation>>();

        static async Task Main(string[] args)
        {
            var workspace = MSBuildWorkspace.Create();
            var solution = await workspace.OpenSolutionAsync(@"C:\Projects\De Vries\DeVries.WMS.Regulations\DeVries.WMS.Regulations.sln");

            foreach (var project in solution.Projects)
            {
                Projects.Add(project, new Lazy<Compilation>(() => project.GetCompilationAsync().Result));
            }

            var specProject = solution.Projects.FirstOrDefault(p => p.Name == "Specifications");
            var compilation = Projects[specProject].Value;


            foreach (var tree in compilation.SyntaxTrees)
            {
                var visitor = new EntryPointFinder(compilation, compilation.GetSemanticModel(tree));
                visitor.Visit(tree.GetRoot());
            }


            Console.ReadLine();
        }

        class EntryPointFinder : CSharpSyntaxRewriter
        {
            public EntryPointFinder(Compilation compilation, SemanticModel model)
            {
                this.compilation = compilation;
                this.model = model;
            }

            private readonly Compilation compilation;
            private readonly SemanticModel model;

            public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                var symbolInfo = model.GetDeclaredSymbol(node);
                var attributes = symbolInfo.GetAttributes();
                if (attributes.Any(attribute => attribute.AttributeClass.Name == "BindingAttribute"))// && attribute.AttributeClass.ContainingNamespace.Name == "TechTalk.SpecFlow"))
                {
                    Console.WriteLine(node.Identifier.ValueText);
                    return base.VisitClassDeclaration(node);
                }

                return node;
            }

            public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                var symbolInfo = model.GetDeclaredSymbol(node);
                var attributes = symbolInfo.GetAttributes();
                if (attributes.Any(attribute => attribute.AttributeClass.Name == "WhenAttribute"))
                {
                    Console.WriteLine("\t" + node.Identifier.ValueText);
                    return base.VisitMethodDeclaration(node);
                }

                return node;
            }

            public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                if (node.Expression.Kind() == SyntaxKind.SimpleMemberAccessExpression)
                {
                    var methodInvoked = model.GetSymbolInfo(node);
                    var location = methodInvoked.Symbol?.Locations.FirstOrDefault(l => l.IsInSource);
                    if (location != null)
                    {
                        var visitor = new StacktraceCollector(location.SourceTree, methodInvoked.Symbol);
                        visitor.Visit(location.SourceTree.GetRoot());
                    }
                }

                return base.VisitInvocationExpression(node);
            }
        }

        class StacktraceCollector : CSharpSyntaxRewriter
        {
            private readonly Compilation compilation;
            private readonly SyntaxTree tree;
            private readonly ISymbol entryPoint;
            private readonly SemanticModel model;

            public StacktraceCollector(SyntaxTree tree, ISymbol entryPoint)
            {
                var project = Projects.Keys.FirstOrDefault(p => p.Documents.Any(d => d.FilePath == tree.FilePath));
                this.compilation = Projects[project].Value;
                this.tree = tree;
                this.entryPoint = entryPoint;
                this.model = this.compilation.GetSemanticModel(this.tree);
            }

            public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                var symbol = this.model.GetDeclaredSymbol(node);
                if (symbol.ToString() == entryPoint.ToString())
                {
                    Console.WriteLine("\t\t" + symbol.ToString());
                }

                return base.VisitMethodDeclaration(node);
            }
        }
    }
}
