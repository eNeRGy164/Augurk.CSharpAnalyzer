using Augurk.CSharpAnalyzer.Analyzers;
using Augurk.CSharpAnalyzer.Collectors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Augurk.CSharpAnalyzer
{
    public class EntryPointAnalyzer : CSharpSyntaxRewriter
    {
        private readonly AnalyzeContext context;
        private readonly SemanticModel model;

        public EntryPointAnalyzer(AnalyzeContext context, SemanticModel model)
        {
            this.context = context;
            this.model = model;
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
                this.context.Collector.StepInto(symbolInfo);
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
                    this.context.Collector.StepInto(methodInvoked.Symbol as IMethodSymbol);
                    var visitor = new StackTraceAnalyzer(context, declaringSyntaxNode.SyntaxTree, targetTypeInfo);
                    visitor.Visit(declaringSyntaxNode.GetSyntax());
                    this.context.Collector.StepOut();
                }
                else
                {
                    this.context.Collector.StepOver(methodInvoked.Symbol as IMethodSymbol);
                }
            }

            return base.VisitInvocationExpression(node);
        }
    }
}
