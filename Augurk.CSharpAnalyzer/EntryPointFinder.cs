using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Augurk.CSharpAnalyzer
{
    public class EntryPointFinder : CSharpSyntaxRewriter
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
}
