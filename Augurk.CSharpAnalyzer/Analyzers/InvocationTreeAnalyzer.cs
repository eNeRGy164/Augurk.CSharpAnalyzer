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
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Augurk.CSharpAnalyzer.Analyzers
{
    /// <summary>
    /// Analyzes CSharp code of a method being invoked on a particular type to find other invocations.
    /// </summary>
    public class InvocationTreeAnalyzer : CSharpSyntaxRewriter
    {
        private readonly AnalyzeContext context;
        private readonly SemanticModel model;
        private readonly TypeInfo? targetType;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationTreeAnalyzer"/> class.
        /// </summary>
        /// <param name="context">The <see cref="AnalyzeContext"/> tracking information regarding the analysis.</param>
        /// <param name="tree">A <see cref="SyntaxTree"/> that needs to be analyzed.</param>
        /// <param name="targetType">Type on which the method being analyzed here is invoked.</param>
        public InvocationTreeAnalyzer(AnalyzeContext context, SyntaxTree tree, TypeInfo? targetType)
        {
            this.context = context;
            var project = context.Projects.Keys.FirstOrDefault(p => p.Documents.Any(d => d.FilePath == tree.FilePath));
            var compilation = context.Projects[project].Value;
            this.model = compilation.GetSemanticModel(tree);
            this.targetType = targetType;
        }

        /// <summary>
        /// Called when a method invocation is discovered in source code.
        /// </summary>
        /// <param name="node">An <see cref="InvocationExpressionSyntax"/> describing the method being invoked.</param>
        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            // Determine the kind of invocation
            if (node.Expression.Kind() == SyntaxKind.SimpleMemberAccessExpression ||
                node.Expression.Kind() == SyntaxKind.InvocationExpression ||
                node.Expression.Kind() == SyntaxKind.IdentifierName)
            {
                // Check if the method being invoked is defined in source
                var methodInvoked = model.GetSymbolInfo(node);
                var declaringSyntaxReference = methodInvoked.Symbol?.DeclaringSyntaxReferences.FirstOrDefault();
                if (methodInvoked.Symbol == null)
                {
                    return node;
                }

                // Check if the method being invoked is abstract or virtual
                if ((methodInvoked.Symbol.IsAbstract || methodInvoked.Symbol.IsVirtual) && targetType.HasValue && declaringSyntaxReference != null)
                {
                    // Find the members of the type on which the current method is being invoked that are defined as an override
                    foreach (var member in targetType?.Type.GetMembers().OfType<IMethodSymbol>().Where(member => member.IsOverride))
                    {
                        // Check if the current member is the abstract/virtual method being invoked
                        if (member.OverriddenMethod.DeclaringSyntaxReferences[0].Equals(declaringSyntaxReference))
                        {
                            // Step into the abstract call
                            context.Collector.StepInto(member);
                            var visitor = new InvocationTreeAnalyzer(context, member.DeclaringSyntaxReferences[0].SyntaxTree, targetType);
                            visitor.Visit(member.DeclaringSyntaxReferences[0].GetSyntax());
                            context.Collector.StepOut();
                        }
                    }
                }
                else if (declaringSyntaxReference != null)
                {
                    // TODO Get type of parameter
                    // Step into the method being invoked
                    var targetTypeInfo = node.Expression.Kind() == SyntaxKind.IdentifierName ? targetType : node.GetTargetType(model);
                    context.Collector.StepInto(methodInvoked.Symbol as IMethodSymbol);
                    var visitor = new InvocationTreeAnalyzer(context, declaringSyntaxReference.SyntaxTree, targetTypeInfo);
                    visitor.Visit(declaringSyntaxReference.GetSyntax());
                    context.Collector.StepOut();
                }
                else
                {
                    // Target method is not defined in source, so trace it but do not step into it
                    context.Collector.StepOver(methodInvoked.Symbol as IMethodSymbol);
                }
            }

            return base.VisitInvocationExpression(node);
        }
    }
}
