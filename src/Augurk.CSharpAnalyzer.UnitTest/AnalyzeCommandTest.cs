using Augurk.CSharpAnalyzer.Commands;
using Augurk.CSharpAnalyzer.Options;
using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Augurk.CSharpAnalyzer.UnitTest
{
    /// <summary>
    /// Tests the <see cref="AnalyzeCommand"/> class.
    /// </summary>
    [TestClass]
    public class AnalyzeCommandTest
    {
        /// <summary>
        /// Tests that performing analysis from a .NET Core project works correctly.
        /// </summary>
        [TestMethod]
        [Ignore] // This test fails due to a bug in MSBuild. See https://github.com/Augurk/Augurk.CSharpAnalyzer/issues/1
        public async Task RunAnalysisFromNetCoreProject()
        {
            // Arrange
            AnalyzeOptions analyzeOptions = new AnalyzeOptions()
            {
                Solution = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\Analyzable Projects\Cucumis.sln"),
                SpecificationsProject = "Cucumis.Specifications"
            };
            AnalyzeCommand analyzeCommand = new AnalyzeCommand();
            analyzeCommand.GetWorkspaceFunc = GetWorkspace;

            // Act
            JToken result = await analyzeCommand.Analyze(analyzeOptions);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result["RootInvocations"].Any());
        }

        static Task<Workspace> GetWorkspace(string solutionPath)
        {
            var workspace = new AdhocWorkspace();
            var analyzerManager = new AnalyzerManager(solutionPath);
            foreach (var project in analyzerManager.Projects.Values)
            {
                project.AddToWorkspace(workspace);
            }

            return Task.FromResult<Workspace>(workspace);
        }
    }
}
