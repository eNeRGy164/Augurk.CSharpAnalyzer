using Augurk.CSharpAnalyzer.Commands;
using Augurk.CSharpAnalyzer.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

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
        public void RunAnalysisFromNetCoreProject()
        {
            // Arrange
            AnalyzeOptions analyzeOptions = new AnalyzeOptions()
            {
                Solution = Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\Analyzable Projects\Cucumis.sln"),
                SpecificationsProject = "Cucumis.Specifications"
            };
            AnalyzeCommand analyzeCommand = new AnalyzeCommand();

            // Act
            JToken result = analyzeCommand.Analyze(analyzeOptions);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Children().Count() > 0);
        }
    }
}
