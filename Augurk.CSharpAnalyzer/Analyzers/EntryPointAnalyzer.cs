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

using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Augurk.CSharpAnalyzer.Analyzers
{
    /// <summary>
    /// Analyzes CSharp code of a specifications project in order to find entry points for a particular feature (ie. When steps).
    /// </summary>
    public class EntryPointAnalyzer : CSharpSyntaxRewriter
    {
        private readonly AnalyzeContext context;
        private readonly SemanticModel model;
        private bool insideEntryPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntryPointAnalyzer"/> class.
        /// </summary>
        /// <param name="context">The <see cref="AnalyzeContext"/> that tracks the analysis.</param>
        /// <param name="model">A <see cref="SemanticModel"/> of the specifications project.</param>
        public EntryPointAnalyzer(AnalyzeContext context, SemanticModel model)
        {
            this.context = context;
            this.model = model;
        }

        /// <summary>
        /// Called when a class declaration is discovered in the source code.
        /// </summary>
        /// <param name="node">A <see cref="ClassDeclarationSyntax"/> describing the declared class.</param>
        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            // Get attributes for the class declaration
            var symbolInfo = model.GetDeclaredSymbol(node);
            var attributes = symbolInfo.GetAttributes();

            // Check for the presence of a BindingAttribute (we assume that SpecFlow is being used here)
            if (attributes.Any(attribute => attribute.AttributeClass.Name == "BindingAttribute" && attribute.AttributeClass.ContainingNamespace.Name == "SpecFlow"))
            {
                // A class with a [BindingAttribute] is of interest, so dig deeper
                return base.VisitClassDeclaration(node);
            }

            // Do not analyze classes without a BindingAttribute any further
            return node;
        }

        /// <summary>
        /// Called when a method declaration is discovered in the source code.
        /// </summary>
        /// <param name="node">A <see cref="MethodDeclarationSyntax"/> describing the declared method.</param>
        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            // Get attributes for the method declaration
            var symbolInfo = model.GetDeclaredSymbol(node);
            var attributes = symbolInfo.GetAttributes();

            // Check for the presence of a WhenAttribute (we assume that SpecFlow is being used here)
            if (attributes.Any(attribute => attribute.AttributeClass.Name == "WhenAttribute" && attribute.AttributeClass.ContainingNamespace.Name == "SpecFlow"))
            {
                // A method with a [WhenAttribute] is of interest, let's dig deeper
                this.insideEntryPoint = true;
                context.Collector.StepInto(symbolInfo);
                var result = base.VisitMethodDeclaration(node);
                context.Collector.StepOut();
                this.insideEntryPoint = false;
                return result;
            }

            // Do not analyze methods without a WhenAttribute any further
            return node;
        }

        /// <summary>
        /// Called when a method invocation is discovered in the source code.
        /// </summary>
        /// <param name="node">A <see cref="InvocationExpressionSyntax"/> describing the method being invoked.</param>
        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            // Determine the kind of invocation
            if (insideEntryPoint && node.Expression.Kind() == SyntaxKind.SimpleMemberAccessExpression)
            {
                // Find the method that is being invoked and the type on which it is invoked
                SymbolInfo methodInvoked = model.GetSymbolInfo(node);
                TypeInfo? targetTypeInfo = node.GetTargetType(model);
                IEnumerable<TypeInfo?> argumentTypes = node.GetArgumentTypes(methodInvoked.Symbol as IMethodSymbol, model);

                // Find the source location of the method being invoked
                SyntaxReference declaringSyntaxNode = methodInvoked.Symbol?.GetComparableSyntax();
                if (declaringSyntaxNode != null)
                {
                    // Method being invoked is defined in source, so dig deeper
                    context.Collector.StepInto(methodInvoked.Symbol as IMethodSymbol);
                    var visitor = new InvocationTreeAnalyzer(context, declaringSyntaxNode.SyntaxTree, methodInvoked.Symbol as IMethodSymbol, targetTypeInfo, argumentTypes);
                    visitor.Visit(declaringSyntaxNode.GetSyntax());
                    context.Collector.StepOut();
                }
                else
                {
                    // Method being invoked is not defined in source, trace it but don't go deeper
                    context.Collector.StepOver(methodInvoked.Symbol as IMethodSymbol);
                }
            }

            // Dig deeper
            return base.VisitInvocationExpression(node);
        }
    }
}
