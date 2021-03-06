﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.4.0.0
//      SpecFlow Generator Version:2.4.0.0
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
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class AnalyzeExtensionMethodsFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "AnalyzeExtensionMethods.feature"
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
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Analyze Extension Methods", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Analyze Extension Methods")))
            {
                global::Augurk.CSharpAnalyzer.Specifications.Features.AnalyzeExtensionMethodsFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>(_testContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("an extension method is invoked")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Analyze Extension Methods")]
        public virtual void AnExtensionMethodIsInvoked()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("an extension method is invoked", null, ((string[])(null)));
#line 3
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 5
 testRunner.Given("\'Cucumis.Specifications\' contains feature files", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.When("an analysis is run", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Kind",
                        "Local",
                        "Level",
                        "Expression/Signature"});
            table1.AddRow(new string[] {
                        "When",
                        "",
                        "0",
                        "an extension method is invoked"});
            table1.AddRow(new string[] {
                        "Public",
                        "true",
                        "1",
                        "Cucumis.GherkinExtensions.Harvest(Cucumis.Gherkin), Cucumis"});
            table1.AddRow(new string[] {
                        "Public",
                        "true",
                        "2",
                        "Cucumis.Gardener.HarvestGherkin(Cucumis.Gherkin), Cucumis"});
            table1.AddRow(new string[] {
                        "Public",
                        "",
                        "3",
                        "System.Console.WriteLine(string), mscorlib"});
            table1.AddRow(new string[] {
                        "Internal",
                        "true",
                        "3",
                        "Cucumis.Gherkin.CutVine(), Cucumis"});
            table1.AddRow(new string[] {
                        "Public",
                        "",
                        "4",
                        "System.Console.WriteLine(string), mscorlib"});
#line 7
 testRunner.Then("the resulting report contains \'When an extension method is invoked\'", ((string)(null)), table1, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("an extension method is invoked on a derived type")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Analyze Extension Methods")]
        public virtual void AnExtensionMethodIsInvokedOnADerivedType()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("an extension method is invoked on a derived type", null, ((string[])(null)));
#line 16
this.ScenarioInitialize(scenarioInfo);
            this.ScenarioStart();
#line 18
 testRunner.Given("\'Cucumis.Specifications\' contains feature files", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 19
 testRunner.When("an analysis is run", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Kind",
                        "Local",
                        "Level",
                        "Expression/Signature"});
            table2.AddRow(new string[] {
                        "When",
                        "",
                        "0",
                        "an extension method is invoked on a derived type"});
            table2.AddRow(new string[] {
                        "Public",
                        "true",
                        "1",
                        "Cucumis.GherkinExtensions.Harvest(Cucumis.Gherkin), Cucumis"});
            table2.AddRow(new string[] {
                        "Public",
                        "true",
                        "2",
                        "Cucumis.Gardener.HarvestGherkin(Cucumis.Gherkin), Cucumis"});
            table2.AddRow(new string[] {
                        "Public",
                        "",
                        "3",
                        "System.Console.WriteLine(string), mscorlib"});
            table2.AddRow(new string[] {
                        "Internal",
                        "true",
                        "3",
                        "Cucumis.PickyGherkin.CutVine(), Cucumis"});
            table2.AddRow(new string[] {
                        "Internal",
                        "true",
                        "4",
                        "Cucumis.Gherkin.CutVine(), Cucumis"});
            table2.AddRow(new string[] {
                        "Public",
                        "",
                        "5",
                        "System.Console.WriteLine(string), mscorlib"});
            table2.AddRow(new string[] {
                        "Public",
                        "",
                        "4",
                        "System.Console.WriteLine(string), mscorlib"});
#line 20
 testRunner.Then("the resulting report contains \'When an extension method is invoked on a derived t" +
                    "ype\'", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
