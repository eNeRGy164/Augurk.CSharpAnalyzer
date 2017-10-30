using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Augurk.CSharpAnalyzer.Analyzers
{
    public class StackTraceAnalyzer : CSharpSyntaxRewriter
    {
        private readonly AnalyzeContext context;
        private readonly SemanticModel model;
        private readonly TypeInfo? targetType;

        public StackTraceAnalyzer(AnalyzeContext context, SyntaxTree tree, TypeInfo? targetType)
        {
            this.context = context;
            var project = context.Projects.Keys.FirstOrDefault(p => p.Documents.Any(d => d.FilePath == tree.FilePath));
            var compilation = context.Projects[project].Value;
            this.model = compilation.GetSemanticModel(tree);
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
                            context.Collector.StepInto(member);
                            var visitor = new StackTraceAnalyzer(context, member.DeclaringSyntaxReferences[0].SyntaxTree, this.targetType);
                            visitor.Visit(member.DeclaringSyntaxReferences[0].GetSyntax());
                            context.Collector.StepOut();
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

                    context.Collector.StepInto(methodInvoked.Symbol as IMethodSymbol);
                    var visitor = new StackTraceAnalyzer(context, declaringSyntaxReference.SyntaxTree, targetTypeInfo);
                    visitor.Visit(declaringSyntaxReference.GetSyntax());
                    context.Collector.StepOut();
                }
                else
                {
                    context.Collector.StepOver(methodInvoked.Symbol as IMethodSymbol);
                }
            }

            return base.VisitInvocationExpression(node);
        }
    }
}
