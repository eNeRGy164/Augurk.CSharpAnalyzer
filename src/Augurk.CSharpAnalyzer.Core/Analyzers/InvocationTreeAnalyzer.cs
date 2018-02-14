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

        private readonly InvokedMethod invokedMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationTreeAnalyzer"/> class.
        /// </summary>
        /// <param name="context">The <see cref="AnalyzeContext"/> tracking information regarding the analysis.</param>
        /// <param name="model">A <see cref="SemanticModel"/> for the code being analyzed.</param>
        public InvocationTreeAnalyzer(AnalyzeContext context, SemanticModel model)
            : this(context, model, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationTreeAnalyzer"/> class.
        /// </summary>
        /// <param name="context">The <see cref="AnalyzeContext"/> tracking information regarding the analysis.</param>
        /// <param name="model">A <see cref="SemanticModel"/> for the code being analyzed.</param>
        /// <param name="invokedMethod">An <see cref="InvokedMethod"/> instance that represents the method currently being analyzed.</param>
        public InvocationTreeAnalyzer(AnalyzeContext context, SemanticModel model, InvokedMethod? invokedMethod)
        {
            this.context = context;
            this.model = model;
            this.invokedMethod = invokedMethod.GetValueOrDefault();
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

                return HandleInvocation(node, methodInvoked, declaringSyntaxReference, this.invokedMethod.TargetType);
            }

            return base.VisitInvocationExpression(node);
        }

        private SyntaxNode HandleInvocation(InvocationExpressionSyntax node, IMethodSymbol methodInvoked, SyntaxReference declaringSyntaxReference, TypeInfo? targetType)
        {
            // Determine how the method is being invoked
            if (methodInvoked.IsExtensionMethod)
            {
                // Find target type and step in
                return HandleExtensionMethod(node, methodInvoked, declaringSyntaxReference, targetType);
            }
            else if (methodInvoked.ContainingType.TypeKind == TypeKind.Interface)
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
                        targetType = node.GetTargetOfInvocation(methodInvoked, model, this.invokedMethod);
                        return StepInto(node, new InvokedMethod(methodInvoked, targetType, node.GetArgumentTypes(methodInvoked, model, this.invokedMethod), declaringSyntaxReference));
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
                    return StepInto(node, new InvokedMethod(methodInvoked, targetType, node.GetArgumentTypes(methodInvoked, model, this.invokedMethod), declaringSyntaxReference));
                }
            }
        }

        private SyntaxNode HandleExtensionMethod(InvocationExpressionSyntax node, IMethodSymbol method, SyntaxReference declaringSyntaxReference, TypeInfo? targetType)
        {
            TypeInfo? target = node.GetTargetOfInvocation(method, model, this.invokedMethod);
            // Finish by locking in these types through a ToList
            IEnumerable<TypeInfo?> argumentTypes = Enumerable.Repeat(target, 1).Concat(node.GetArgumentTypes(method.ReducedFrom, model, this.invokedMethod)).ToList();
            return StepIntoExtensionMethod(node, new InvokedMethod(method.ReducedFrom, null, argumentTypes, method.ReducedFrom.GetComparableSyntax()));
        }

        private SyntaxNode HandleInterfaceMethod(InvocationExpressionSyntax node, IMethodSymbol method, SyntaxReference declaringSyntaxReference, TypeInfo? targetType)
        {
            TypeInfo? target = node.GetTargetOfInvocation(method, model, this.invokedMethod);
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
            TypeInfo? target = node.GetTargetOfInvocation(method, model, this.invokedMethod);
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
            TypeInfo? target = node.GetTargetOfInvocation(method, model, this.invokedMethod);
            if (target.HasValue)
            {
                // Find the members of the type on which the current method is being invoked that are defined as an override
                foreach (var member in target?.Type.GetMembers().OfType<IMethodSymbol>().Where(member => member.IsOverride))
                {
                    // Walk the override tree until we find the method being invoked
                    IMethodSymbol overridenMethod = member.OverriddenMethod;
                    while (overridenMethod != null && !overridenMethod.GetComparableSyntax().Equals(declaringSyntaxReference) && overridenMethod.IsOverride)
                    {
                        overridenMethod = overridenMethod.OverriddenMethod;
                    }

                    // Check if the current member is the abstract/virtual method being invoked
                    if (overridenMethod.GetComparableSyntax().Equals(declaringSyntaxReference))
                    {
                        return HandleInvocation(node, member, member.GetComparableSyntax(), target);
                    }
                }
            }

            return StepInto(node, new InvokedMethod(method, this.invokedMethod.TargetType, node.GetArgumentTypes(method, model, this.invokedMethod), declaringSyntaxReference));
        }

        private SyntaxNode StepOver(InvocationExpressionSyntax node, IMethodSymbol method)
        {
            context.Collector.StepOver(method);
            return node;
        }

        private SyntaxNode StepOverExtensionMethod(InvocationExpressionSyntax node, IMethodSymbol method, ITypeSymbol targetType)
        {
            context.Collector.StepOverExtensionMethod(method, targetType);
            return node;
        }

        private SyntaxNode StepInto(InvocationExpressionSyntax node, InvokedMethod method)
        {
            if (context.Collector.IsAlreadyCollected(method.Method))
            {
                return StepOver(node, method.Method);
            }
            else
            {
                context.Collector.StepInto(method.Method);
                var visitor = new InvocationTreeAnalyzer(context, method.DeclaringSyntaxReference.SyntaxTree.GetSemanticModel(context),
                    new InvokedMethod(method.Method, method.TargetType, method.ArgumentTypes, method.DeclaringSyntaxReference));
                visitor.Visit(method.DeclaringSyntaxReference.GetSyntax());
                context.Collector.StepOut();
                return node;
            }
        }

        private SyntaxNode StepIntoExtensionMethod(InvocationExpressionSyntax node, InvokedMethod method)
        {
            if (context.Collector.IsExtensionMethodAlreadyCollected(method.Method, method.ArgumentTypes.First()?.Type))
            {
                return StepOverExtensionMethod(node, method.Method, method.ArgumentTypes.First()?.Type);
            }
            else
            {
                context.Collector.StepIntoExtensionMethod(method.Method, method.ArgumentTypes.First()?.Type);
                var visitor = new InvocationTreeAnalyzer(context, method.DeclaringSyntaxReference.SyntaxTree.GetSemanticModel(context),
                    new InvokedMethod(method.Method, method.TargetType, method.ArgumentTypes, method.DeclaringSyntaxReference));
                visitor.Visit(method.DeclaringSyntaxReference.GetSyntax());
                context.Collector.StepOut();
                return node;
            }
        }
    }
}
