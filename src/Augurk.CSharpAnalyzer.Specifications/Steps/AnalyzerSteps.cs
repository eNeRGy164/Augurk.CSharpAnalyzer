using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Augurk.CSharpAnalyzer.Commands;
using Augurk.CSharpAnalyzer.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;

namespace Augurk.CSharpAnalyzer.Specifications.Steps
{
    [Binding]
    public class AnalyzerSteps
    {
        private string _solution = Path.Combine(AppContext.BaseDirectory, @"..\..\..\Analyzable Projects\Cucumis.sln");
        private string _targetProject;
        private JToken _analyzedInvocations;

        [Given(@"'(.*)' contains feature files")]
        public void GivenContainsFeatureFiles(string projectName)
        {
            _targetProject = projectName;
        }
        
        [When(@"an analysis is run")]
        public void WhenAnAnalysisIsRun()
        {
            var options = new AnalyzeOptions
            {
                Solution = _solution,
                SpecificationsProject = _targetProject
            };

            var command = new AnalyzeCommand();

            _analyzedInvocations = command.Analyze(options)["RootInvocations"];
        }

        [Then(@"the following report is returned for '(.*)'")]
        public void ThenTheFollowingReportIsReturnedFor(string projectName, Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the first (.*) lines of the resulting report are")]
        public void ThenTheFirstLinesOfTheResultingReportAre(int numberOfLinesToCheck, Table table)
        {
            if (_analyzedInvocations == null)
            {
                Assert.Inconclusive("Please use a matching WHEN step before using this THEN step.");
            }

            Assert.AreEqual(numberOfLinesToCheck, table.RowCount, "The provided number of rows does not match the mentioned number of rows.");

            List<dynamic> flatList = new List<dynamic>();
            Flatten(_analyzedInvocations as JArray, flatList);

            for (int counter = 0; counter < numberOfLinesToCheck; counter++)
            {
                Assert.AreEqual(table.Rows[counter]["Kind"], flatList[counter].Kind, $"The Kind does not match on row {counter}");
                Assert.AreEqual(string.IsNullOrWhiteSpace(table.Rows[counter]["Local"]) ? false : Boolean.Parse(table.Rows[counter]["Local"]),
                                flatList[counter].Local, $"Row {counter} locallity is incorrect");
                Assert.AreEqual(table.Rows[counter]["Expression/Signature"], flatList[counter].Signature, $"The Expression/Signature does not match on row {counter}");
            }
        }

        [Then(@"the resulting report contains '(.*)'")]
        public void ThenTheResultingReportContains(string when, Table table)
        {
            if (_analyzedInvocations == null)
            {
                Assert.Inconclusive("Please use a matching WHEN step before using this THEN step.");
            }
            if (!when.StartsWith("When"))
            {
                Assert.Inconclusive("When text must start with 'When '");
            }

            JToken jWhen = _analyzedInvocations.SingleOrDefault(token => (token["RegularExpressions"]?.Values<string>().Contains(when.Substring(5))).GetValueOrDefault());
            Assert.IsNotNull(jWhen, "The provided step cannot be found");

            List<dynamic> flatList = new List<dynamic>();
            Flatten(new JArray(jWhen), flatList);

            for (int counter = 0; counter < Math.Min(table.RowCount, flatList.Count); counter++)
            {
                Assert.AreEqual(table.Rows[counter]["Expression/Signature"], flatList[counter].Signature,
                                $"The Expression/Signature does not match on row {counter}");
                Assert.AreEqual(table.Rows[counter]["Kind"], flatList[counter].Kind, 
                                $"The Kind does not match on row {counter}");
                Assert.AreEqual(!String.IsNullOrWhiteSpace(table.Rows[counter]["Local"]) && Boolean.Parse(table.Rows[counter]["Local"]),
                                flatList[counter].Local, $"Row {counter} locality is incorrect");
                if (table.ContainsColumn("Level"))
                {
                    Assert.AreEqual(Int32.Parse(table.Rows[counter]["Level"]), flatList[counter].Level, $"The level on row {counter} is incorrect");
                }
            }

            Assert.AreEqual(table.RowCount, flatList.Count, "The provided number of rows does not match the actual number of rows.");

        }

        private void Flatten(JArray invocations, List<dynamic> flatList, int level = 0)
        {
            foreach (var invocation in invocations)
            {
                JToken regExpressions = invocation["RegularExpressions"];

                flatList.Add(new
                {
                    Kind = invocation["Kind"].Value<String>(),
                    Local = invocation["Local"]?.Value<bool>() ?? false,
                    Level = level,
                    Signature = regExpressions == null ? invocation["Signature"].Value<String>() : String.Join(",", ((JArray)regExpressions).ToList())
                });

                JArray deeperInvocations = invocation["Invocations"] as JArray;

                if (deeperInvocations != null)
                {
                    Flatten(deeperInvocations, flatList, level + 1);
                }
            }
        }
    }
}
