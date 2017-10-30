using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Augurk.CSharpAnalyzer
{
    public class EntryPointFinder : CSharpSyntaxRewriter
    {
        private readonly Dictionary<Project, Lazy<Compilation>> projects;
        private readonly SemanticModel model;
        private readonly IStackTraceCollector collector;

        public EntryPointFinder(Dictionary<Project, Lazy<Compilation>> projects, SemanticModel model, IStackTraceCollector collector)
        {
            this.projects = projects;
            this.model = model;
            this.collector = collector;
        }

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
                    var visitor = new StackTraceAnalyzer(projects, declaringSyntaxNode.SyntaxTree, targetTypeInfo, collector);
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
}
