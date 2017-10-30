﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.2.0.0
//      SpecFlow Generator Version:2.2.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Augurk.CSharpAnalyzer.Specifications.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.2.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class AnalyzeACProjectWithASingleTestProjectFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "Analyze.feature"
#line hidden
        
        public virtual Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner(null, 0);
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Analyze a C# project with a single test project", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Analyze a C# project with a single test project")))
            {
                global::Augurk.CSharpAnalyzer.Specifications.Features.AnalyzeACProjectWithASingleTestProjectFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(TestContext);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Analyze")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Analyze a C# project with a single test project")]
        public virtual void Analyze()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Analyze", ((string[])(null)));
#line 3
this.ScenarioSetup(scenarioInfo);
#line 4
testRunner.Given("\'Augurk.CSharpAnalyzer.Specifications\' contains feature files", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 5
testRunner.And("those features describe \'Augurk.CSharpAnalyzer\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 6
testRunner.When("an analysis is run", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Type",
                        "Signature"});
            table1.AddRow(new string[] {
                        "When",
                        "an analysis is run"});
            table1.AddRow(new string[] {
                        "EntryPoint",
                        "Augurk.CSharpAnalyzer.AnalyzeCommand.Execute(Augurk.CSharpAnalyzer.Options.Analyz" +
                            "eOptions)"});
            table1.AddRow(new string[] {
                        "Public-Invoke",
                        "Oakton.ConsoleWriter.Write(ConsoleColor, string)"});
            table1.AddRow(new string[] {
                        "Private-Invoke",
                        "Augurk.CSharpAnalyzer.Analyze(Augurk.CSharpAnalyzer.Options.AnalyzeOptions)"});
#line 7
testRunner.Then("the following report is returned for \'Augurk.CSharpAnalyzer\'", ((string)(null)), table1, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion