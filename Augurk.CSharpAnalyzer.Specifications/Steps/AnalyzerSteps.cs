using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Augurk.CSharpAnalyzer.Collectors;
using Augurk.CSharpAnalyzer.Commands;
using Augurk.CSharpAnalyzer.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
using System.Threading.Tasks;

namespace Augurk.CSharpAnalyzer.Specifications.Steps
{
    [Binding]
    public class AnalyzerSteps
    {
        private string _solution = Path.Combine(AppContext.BaseDirectory, @"..\..\..\Augurk.CSharpAnalyzer.sln");
        private string _targetProject;
        private string _sutProject;
        private JToken _output;

        [Given(@"'(.*)' contains feature files")]
        public void GivenContainsFeatureFiles(string projectName)
        {
            _targetProject = projectName;
        }
        
        [Given(@"those features describe '(.*)'")]
        public void GivenThoseFeaturesDescribe(string projectName)
        {
            _sutProject = projectName;
        }
        
        [When(@"an analysis is run")]
        public async Task WhenAnAnalysisIsRun()
        {
            var options = new AnalyzeOptions();
            options.Solution = _solution;
            options.SpecificationsProject = _targetProject;
            options.SystemUnderTest = _sutProject;

            var command = new AnalyzeCommand();

            _output = await command.Analyze(options);
        }

        [Then(@"the following report is returned for '(.*)'")]
        public void ThenTheFollowingReportIsReturnedFor(string projectName, Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the first (.*) lines of the reported return for '(.*)' are")]
        public void ThenTheFirstLinesOfTheReportedReturnForAre(int numberOfLinesToCheck, string projectName, Table table)
        {
            if (_output == null)
            {
                Assert.Inconclusive("Please use a matching WHEN step before using this THEN step.");
            }

            Assert.AreEqual(numberOfLinesToCheck, table.RowCount, "The provided number of rows does not match the mentioned number of rows.");

            List<dynamic> flatList = new List<dynamic>();
            foreach (var invocation in _output)
            {
                JToken regExpressions = invocation["RegularExpressions"];

                flatList.Add(new
                {
                    Kind = invocation["Kind"].Value<String>(),
                    Signature = regExpressions == null ? invocation["Signature"].Value<String>() : String.Join(",", ((JArray)regExpressions).ToList())
                });
            }

            for (int counter = 0; counter < numberOfLinesToCheck; counter++)
            {
                Assert.AreEqual(table.Rows[0]["Kind"], flatList[0].Kind, $"The Kind does not match on row {counter}");
                Assert.AreEqual(table.Rows[0]["Expression/Signature"], flatList[0].Signature, $"The Expression/Signature does not match on row {counter}");
            }
        }
    }
}
