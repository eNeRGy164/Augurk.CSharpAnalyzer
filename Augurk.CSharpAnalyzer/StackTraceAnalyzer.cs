using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Augurk.CSharpAnalyzer
{
    public class StacktraceCollector : CSharpSyntaxRewriter
    {
        private readonly SemanticModel model;
        private readonly TypeInfo? targetType;
        private readonly IStackTraceCollector collector;

        public StacktraceCollector(SyntaxTree tree, TypeInfo? targetType, IStackTraceCollector collector)
        {
            var project = Program.Projects.Keys.FirstOrDefault(p => p.Documents.Any(d => d.FilePath == tree.FilePath));
            var compilation = Program.Projects[project].Value;
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
