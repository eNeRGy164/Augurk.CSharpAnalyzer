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
using System;
using System.Collections.Generic;
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

        private readonly IMethodSymbol methodAnalyzed;
        private readonly TypeInfo? targetType;
        private readonly IEnumerable<TypeInfo?> argumentTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationTreeAnalyzer"/> class.
        /// </summary>
        /// <param name="context">The <see cref="AnalyzeContext"/> tracking information regarding the analysis.</param>
        /// <param name="model">A <see cref="SemanticModel"/> for the code being analyzed.</param>
        public InvocationTreeAnalyzer(AnalyzeContext context, SemanticModel model)
            : this(context, model, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationTreeAnalyzer"/> class.
        /// </summary>
        /// <param name="context">The <see cref="AnalyzeContext"/> tracking information regarding the analysis.</param>
        /// <param name="model">A <see cref="SemanticModel"/> for the code being analyzed.</param>
        /// <param name="targetType">Type on which the method being analyzed here is invoked.</param>
        /// <param name="methodAnalyzed">The <see cref="IMethodSymbol"/> representing the method being invoked.</param>
        /// <param name="argumentTypes">Types of the argument passed to the method being analyzed here.</param>
        public InvocationTreeAnalyzer(AnalyzeContext context, SemanticModel model, IMethodSymbol methodAnalyzed, TypeInfo? targetType, IEnumerable<TypeInfo?> argumentTypes)
        {
            this.context = context;
            this.model = model;
            this.methodAnalyzed = methodAnalyzed;
            this.targetType = targetType;
            this.argumentTypes = argumentTypes;
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
                IMethodSymbol methodInvoked = model.GetSymbolInfo(node).Symbol as IMethodSymbol;
                if (methodInvoked == null)
                {
                    // Nothing to see here
                    return node;
                }

                SyntaxReference declaringSyntaxReference = methodInvoked.GetComparableSyntax();
                if (declaringSyntaxReference == null)
                {
                    return StepOver(node, methodInvoked);
                }

                return HandleInvocation(node, methodInvoked, declaringSyntaxReference, this.targetType);
            }

            return base.VisitInvocationExpression(node);
        }

        private SyntaxNode HandleInvocation(InvocationExpressionSyntax node, IMethodSymbol methodInvoked, SyntaxReference declaringSyntaxReference, TypeInfo? targetType)
        {
            // Determine how the method is being invoked
            if (methodInvoked.ContainingType.TypeKind == TypeKind.Interface)
            {
                // Find implementation, or step over
                return HandleInterfaceMethod(node, methodInvoked, declaringSyntaxReference, targetType);
            }
            else if (methodInvoked.ContainingType.TypeKind == TypeKind.Class && methodInvoked.ContainingType.IsAbstract)
            {
                if (methodInvoked.IsAbstract)
                {
                    // Find implementation, or step over
                    return HandleAbstractMethod(node, methodInvoked, declaringSyntaxReference, targetType);
                }
                else
                {
                    if (methodInvoked.IsVirtual || methodInvoked.IsOverride)
                    {
                        // Find implementation and step in, or step into virtual method
                        return HandleVirtualMethod(node, methodInvoked, declaringSyntaxReference, targetType);
                    }
                    else
                    {
                        // Step in
                        return StepIn(node, new InvokedMethod(methodInvoked, targetType, node.GetArgumentTypes(methodInvoked, model), declaringSyntaxReference));
                    }
                }
            }
            else
            {
                if (methodInvoked.IsVirtual || methodInvoked.IsOverride)
                {
                    // Find implementation and step in, or step into virtual method
                    return HandleVirtualMethod(node, methodInvoked, declaringSyntaxReference, targetType);
                }
                else
                {
                    // Step in
                    return StepIn(node, new InvokedMethod(methodInvoked, targetType, node.GetArgumentTypes(methodInvoked, model), declaringSyntaxReference));
                }
            }
        }

        private SyntaxNode HandleInterfaceMethod(InvocationExpressionSyntax node, IMethodSymbol method, SyntaxReference declaringSyntaxReference, TypeInfo? targetType)
        {
            TypeInfo? target = node.GetTargetOfInvocation(method, model, targetType);
            if (!target.HasValue)
            {
                return StepOver(node, method);
            }

            IMethodSymbol implementingMethod = target.Value.Type.FindImplementationForInterfaceMember(method) as IMethodSymbol;
            if (implementingMethod != null)
            {
                return HandleInvocation(node, implementingMethod, implementingMethod.GetComparableSyntax(), target);
            }
            else
            {
                return StepOver(node, method);
            }
        }

        private SyntaxNode HandleAbstractMethod(InvocationExpressionSyntax node, IMethodSymbol method, SyntaxReference declaringSyntaxReference, TypeInfo? targetType)
        {
            TypeInfo? target = node.GetTargetOfInvocation(method, model, targetType);
            if (target.HasValue)
            {
                // Find the members of the type on which the current method is being invoked that are defined as an override
                foreach (var member in target?.Type.GetMembers().OfType<IMethodSymbol>().Where(member => member.IsOverride))
                {
                    // Check if the current member is the abstract/virtual method being invoked
                    if (member.OverriddenMethod.GetComparableSyntax().Equals(declaringSyntaxReference))
                    {
                        return HandleInvocation(node, member, member.GetComparableSyntax(), target);
                    }
                }
            }

            return StepOver(node, method);
        }

        private SyntaxNode HandleVirtualMethod(InvocationExpressionSyntax node, IMethodSymbol method, SyntaxReference declaringSyntaxReference, TypeInfo? targetType)
        {
            TypeInfo? target = node.GetTargetOfInvocation(method, model, targetType);
            if (target.HasValue)
            {
                // Find the members of the type on which the current method is being invoked that are defined as an override
                foreach (var member in target?.Type.GetMembers().OfType<IMethodSymbol>().Where(member => member.IsOverride))
                {
                    // Check if the current member is the abstract/virtual method being invoked
                    if (member.OverriddenMethod.GetComparableSyntax().Equals(declaringSyntaxReference))
                    {
                        return HandleInvocation(node, member, member.GetComparableSyntax(), target);
                    }
                }
            }

            return StepIn(node, new InvokedMethod(method, this.targetType, node.GetArgumentTypes(method, model), declaringSyntaxReference));
        }

        private SyntaxNode StepOver(InvocationExpressionSyntax node, IMethodSymbol method)
        {
            context.Collector.StepOver(method);
            return node;
        }

        private SyntaxNode StepIn(InvocationExpressionSyntax node, InvokedMethod method)
        {
            if (context.Collector.IsAlreadyCollected(method.Method))
            {
                return StepOver(node, method.Method);
            }
            else
            {
                context.Collector.StepInto(method.Method);
                var visitor = new InvocationTreeAnalyzer(context, method.DeclaringSyntaxReference.SyntaxTree.GetSemanticModel(context),
                    method.Method, method.TargetType, method.ArgumentTypes);
                visitor.Visit(method.DeclaringSyntaxReference.GetSyntax());
                context.Collector.StepOut();
                return node;
            }
        }

        private struct InvokedMethod
        {
            public InvokedMethod(IMethodSymbol method, TypeInfo? targetType, IEnumerable<TypeInfo?> argumentTypes, SyntaxReference declaringSyntaxReference)
            {
                this.Method = method;
                this.TargetType = targetType;
                this.ArgumentTypes = argumentTypes;
                this.DeclaringSyntaxReference = declaringSyntaxReference;
            }

            public IMethodSymbol Method { get; private set; }
            public TypeInfo? TargetType { get; private set; }
            public IEnumerable<TypeInfo?> ArgumentTypes { get; private set; }
            public SyntaxReference DeclaringSyntaxReference { get; private set; }
        }
    }
}
