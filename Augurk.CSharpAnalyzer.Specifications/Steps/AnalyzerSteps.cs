using System;
using System.IO;
using Augurk.CSharpAnalyzer.Collectors;
using Augurk.CSharpAnalyzer.Commands;
using Augurk.CSharpAnalyzer.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Converters;
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
            var collector = new DefaultInvocationTreeCollector();

            string resultingJson = await command.Analyze(options);
        }

        [When()]
        [When(Culture="en-en", Regex=@"a third analysis is run")]
        public void WhenAnotherAnalysisIsRun()
        {
            var options = new AnalyzeOptions();
            options.Solution = _solution;
            options.SpecificationsProject = _targetProject;
            options.SystemUnderTest = _sutProject;

            var command = new AnalyzeCommand();
            var collector = new DefaultInvocationTreeCollector();
            command.Collector = collector;

            bool result = command.Execute(options);
            Assert.IsTrue(result, "An error occured during analysis.");

            string resultingJson = collector.GetJsonOutput().ToString();
        }

        [Then(@"the following report is returned for '(.*)'")]
        public void ThenTheFollowingReportIsReturnedFor(string projectName, Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the first (.*) lines of the reported return for '(.*)' are")]
        public void ThenTheFirstLinesOfTheReportedReturnForAre(int numberOfCheckedLines, string projectName, Table table)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
