using Augurk.CSharpAnalyzer.Analyzers;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Microsoft.CodeAnalysis
{
    /// <summary>
    /// Contains extension methods for the <see cref="SyntaxTree"/> type.
    /// </summary>
    internal static class SyntaxTreeExtensions
    {
        /// <summary>
        /// Gets the <see cref="SemanticModel"/> for the current <paramref name="tree"/> by resolving it through the provided <paramref name="context"/>.
        /// </summary>
        /// <param name="tree">A <see cref="SyntaxTree"/> for which to get the semantic model.</param>
        /// <param name="context">An <see cref="AnalyzeContext"/> tracking information about the analysis.</param>
        /// <returns>Returns the <see cref="SemanticModel"/> for the provided <paramref name="tree"/>.</returns>
        public static SemanticModel GetSemanticModel(this SyntaxTree tree, AnalyzeContext context)
        {
            var project = context.Projects.Keys.FirstOrDefault(p => p.Documents.Any(d => d.FilePath == tree.FilePath));
            var compilation = context.Projects[project].Value;
            return compilation.GetSemanticModel(tree);
        }
    }
}
