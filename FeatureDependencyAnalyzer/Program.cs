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
using Microsoft.CodeAnalysis.FindSymbols;

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

            IStackTraceCollector collector = new ConsoleStrackTraceCollector();
            foreach (var tree in compilation.SyntaxTrees)
            {
                var visitor = new EntryPointFinder(compilation.GetSemanticModel(tree), collector);
                visitor.Visit(tree.GetRoot());
            }


            Console.ReadLine();
        }

        class EntryPointFinder : CSharpSyntaxRewriter
        {
            public EntryPointFinder(SemanticModel model, IStackTraceCollector collector)
            {
                this.model = model;
                this.collector = collector;
            }

            private readonly SemanticModel model;
            private readonly IStackTraceCollector collector;

            public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                var symbolInfo = model.GetDeclaredSymbol(node);
                var attributes = symbolInfo.GetAttributes();

                // TODO Remove this if
                if (symbolInfo.Name.Contains("DayRemunerationSteps"))
                {
                    if (attributes.Any(attribute => attribute.AttributeClass.Name == "BindingAttribute"))// && attribute.AttributeClass.ContainingNamespace.Name == "TechTalk.SpecFlow"))
                    {
                        return base.VisitClassDeclaration(node);
                    }
                }

                return node;
            }

            public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
            {
                var symbolInfo = model.GetDeclaredSymbol(node);
                var attributes = symbolInfo.GetAttributes();
                if (attributes.Any(attribute => attribute.AttributeClass.Name == "WhenAttribute"))
                {
                    this.collector.StepInto(symbolInfo);
                    return base.VisitMethodDeclaration(node);
                }

                return node;
            }

            public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                if (node.Expression.Kind() == SyntaxKind.SimpleMemberAccessExpression)
                {
                    TypeInfo? targetTypeInfo = null;
                    var methodInvoked = model.GetSymbolInfo(node);
                    var memberAccessExpressionSyntax = node.Expression as MemberAccessExpressionSyntax;
                    if (memberAccessExpressionSyntax != null)
                    {
                        targetTypeInfo = model.GetTypeInfo(memberAccessExpressionSyntax.Expression);
                    }

                    var declaringSyntaxNode = methodInvoked.Symbol?.DeclaringSyntaxReferences.FirstOrDefault();
                    if (declaringSyntaxNode != null)
                    {
                        collector.StepInto(methodInvoked.Symbol as IMethodSymbol);
                        var visitor = new StacktraceCollector(declaringSyntaxNode.SyntaxTree, targetTypeInfo, collector);
                        visitor.Visit(declaringSyntaxNode.GetSyntax());
                        collector.StepOut();
                    }
                    else
                    {
                        collector.StepOver(methodInvoked.Symbol as IMethodSymbol);
                    }
                }

                return base.VisitInvocationExpression(node);
            }
        }

        class StacktraceCollector : CSharpSyntaxRewriter
        {
            private readonly SemanticModel model;
            private readonly TypeInfo? targetType;
            private readonly IStackTraceCollector collector;

            public StacktraceCollector(SyntaxTree tree, TypeInfo? targetType, IStackTraceCollector collector)
            {
                var project = Projects.Keys.FirstOrDefault(p => p.Documents.Any(d => d.FilePath == tree.FilePath));
                var compilation = Projects[project].Value;
                this.model = compilation.GetSemanticModel(tree);
                this.collector = collector;
                this.targetType = targetType;
            }

            public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                if (node.Expression.Kind() == SyntaxKind.SimpleMemberAccessExpression ||
                    node.Expression.Kind() == SyntaxKind.InvocationExpression ||
                    node.Expression.Kind() == SyntaxKind.IdentifierName)
                {
                    var methodInvoked = model.GetSymbolInfo(node);
                    var declaringSyntaxReference = methodInvoked.Symbol?.DeclaringSyntaxReferences.FirstOrDefault();
                    if (methodInvoked.Symbol == null)
                    {
                        return node;
                    }

                    if ((methodInvoked.Symbol.IsAbstract || methodInvoked.Symbol.IsVirtual) && targetType.HasValue && declaringSyntaxReference != null)
                    {
                        foreach (var member in targetType?.Type.GetMembers().OfType<IMethodSymbol>().Where(member => member.IsOverride))
                        {
                            if (member.OverriddenMethod.DeclaringSyntaxReferences[0].Equals(declaringSyntaxReference))
                            {
                                collector.StepInto(member);
                                var visitor = new StacktraceCollector(member.DeclaringSyntaxReferences[0].SyntaxTree, this.targetType, collector);
                                visitor.Visit(member.DeclaringSyntaxReferences[0].GetSyntax());
                                collector.StepOut();
                            }
                        }
                    }
                    else if (declaringSyntaxReference != null)
                    {
                        TypeInfo? targetTypeInfo = node.Expression.Kind() == SyntaxKind.IdentifierName ? this.targetType : null;
                        // TODO Get type of parameter
                        var memberAccessExpressionSyntax = node.Expression as MemberAccessExpressionSyntax;
                        if (memberAccessExpressionSyntax != null)
                        {
                            targetTypeInfo = model.GetTypeInfo(memberAccessExpressionSyntax.Expression);
                        }

                        collector.StepInto(methodInvoked.Symbol as IMethodSymbol);
                        var visitor = new StacktraceCollector(declaringSyntaxReference.SyntaxTree, targetTypeInfo, collector);
                        visitor.Visit(declaringSyntaxReference.GetSyntax());
                        collector.StepOut();
                    }
                    else
                    {
                        collector.StepOver(methodInvoked.Symbol as IMethodSymbol);
                    }
                }

                return base.VisitInvocationExpression(node);
            }
        }
    }
}
