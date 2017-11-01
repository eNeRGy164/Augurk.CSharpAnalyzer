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
    public partial class AnalyzeWithInheritanceFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext _testContext;
        
#line 1 "AnalyzeWithInheritance.feature"
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
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Analyze With Inheritance", null, ProgrammingLanguage.CSharp, ((string[])(null)));
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
                        && (testRunner.FeatureContext.FeatureInfo.Title != "Analyze With Inheritance")))
            {
                global::Augurk.CSharpAnalyzer.Specifications.Features.AnalyzeWithInheritanceFeature.FeatureSetup(null);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("entrypoint is invoked on inherited automation class")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Analyze With Inheritance")]
        public virtual void EntrypointIsInvokedOnInheritedAutomationClass()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("entrypoint is invoked on inherited automation class", ((string[])(null)));
#line 3
this.ScenarioSetup(scenarioInfo);
#line 6
 testRunner.Given("\'Cucumis.Specifications\' contains feature files", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 7
 testRunner.When("an analysis is run", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Kind",
                        "Local",
                        "Expression/Signature"});
            table1.AddRow(new string[] {
                        "When",
                        "",
                        "entrypoint is invoked on inherited automation class"});
            table1.AddRow(new string[] {
                        "Public",
                        "true",
                        "Cucumis.Gardener.PlantGherkin(), Cucumis"});
            table1.AddRow(new string[] {
                        "Public",
                        "",
                        "System.Console.WriteLine(string), mscorlib"});
#line 8
 testRunner.Then("the resulting report contains \'When entrypoint is invoked on inherited automation" +
                    " class\'", ((string)(null)), table1, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("entrypoint is invoked through an inherited automation class")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Analyze With Inheritance")]
        public virtual void EntrypointIsInvokedThroughAnInheritedAutomationClass()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("entrypoint is invoked through an inherited automation class", ((string[])(null)));
#line 14
this.ScenarioSetup(scenarioInfo);
#line 16
 testRunner.Given("\'Cucumis.Specifications\' contains feature files", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 17
 testRunner.When("an analysis is run", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Kind",
                        "Local",
                        "Expression/Signature"});
            table2.AddRow(new string[] {
                        "When",
                        "",
                        "entrypoint is invoked through an inherited automation class"});
            table2.AddRow(new string[] {
                        "Internal",
                        "",
                        "Cucumis.Specifications.Support.InheritedGardener.PlantGherkinAndWaterIt(), Cucumi" +
                            "s.Specifications"});
            table2.AddRow(new string[] {
                        "Public",
                        "true",
                        "Cucumis.Gardener.PlantGherkin(), Cucumis"});
            table2.AddRow(new string[] {
                        "Public",
                        "",
                        "System.Console.WriteLine(string), mscorlib"});
            table2.AddRow(new string[] {
                        "Public",
                        "true",
                        "Cucumis.Gardener.WaterPlants(), Cucumis"});
            table2.AddRow(new string[] {
                        "Public",
                        "",
                        "System.Console.WriteLine(string), mscorlib"});
#line 18
 testRunner.Then("the resulting report contains \'When entrypoint is invoked through an inherited au" +
                    "tomation class\'", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("same method is invoked with different concrete types")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Analyze With Inheritance")]
        public virtual void SameMethodIsInvokedWithDifferentConcreteTypes()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("same method is invoked with different concrete types", ((string[])(null)));
#line 27
this.ScenarioSetup(scenarioInfo);
#line 29
 testRunner.Given("\'Cucumis.Specifications\' contains feature files", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 30
 testRunner.When("an analysis is run", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Kind",
                        "Local",
                        "Level",
                        "Expression/Signature"});
            table3.AddRow(new string[] {
                        "When",
                        "",
                        "0",
                        "same method is invoked with different concrete types"});
            table3.AddRow(new string[] {
                        "Public",
                        "true",
                        "1",
                        "Cucumis.Gardener.HarvestGherkin(Cucumis.Gherkin), Cucumis"});
            table3.AddRow(new string[] {
                        "Public",
                        "",
                        "2",
                        "System.Console.WriteLine(string), mscorlib"});
            table3.AddRow(new string[] {
                        "Internal",
                        "true",
                        "2",
                        "Cucumis.Gherkin.CutVine(), Cucumis"});
            table3.AddRow(new string[] {
                        "Public",
                        "",
                        "3",
                        "System.Console.WriteLine(string), mscorlib"});
            table3.AddRow(new string[] {
                        "Public",
                        "true",
                        "1",
                        "Cucumis.Gardener.HarvestGherkin(Cucumis.Gherkin), Cucumis"});
            table3.AddRow(new string[] {
                        "Public",
                        "",
                        "2",
                        "System.Console.WriteLine(string), mscorlib"});
            table3.AddRow(new string[] {
                        "Internal",
                        "true",
                        "2",
                        "Cucumis.PickyGherkin.CutVine(), Cucumis"});
            table3.AddRow(new string[] {
                        "Internal",
                        "true",
                        "3",
                        "Cucumis.Gherkin.CutVine(), Cucumis"});
            table3.AddRow(new string[] {
                        "Public",
                        "",
                        "4",
                        "System.Console.WriteLine(string), mscorlib"});
            table3.AddRow(new string[] {
                        "Public",
                        "",
                        "3",
                        "System.Console.WriteLine(string), mscorlib"});
#line 31
 testRunner.Then("the resulting report contains \'When same method is invoked with different concret" +
                    "e types\'", ((string)(null)), table3, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("an instance method is invoked from its base")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Analyze With Inheritance")]
        public virtual void AnInstanceMethodIsInvokedFromItsBase()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("an instance method is invoked from its base", ((string[])(null)));
#line 45
this.ScenarioSetup(scenarioInfo);
#line 47
 testRunner.Given("\'Cucumis.Specifications\' contains feature files", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 48
 testRunner.When("an analysis is run", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Kind",
                        "Local",
                        "Level",
                        "Expression/Signature"});
            table4.AddRow(new string[] {
                        "When",
                        "",
                        "0",
                        "an instance method is invoked from its base"});
            table4.AddRow(new string[] {
                        "Public",
                        "true",
                        "1",
                        "Cucumis.Plant.Bloom(), Cucumis"});
            table4.AddRow(new string[] {
                        "Public",
                        "true",
                        "2",
                        "Cucumis.Melothria.Wither(), Cucumis"});
            table4.AddRow(new string[] {
                        "Private",
                        "true",
                        "3",
                        "Cucumis.Melothria.Rot(), Cucumis"});
            table4.AddRow(new string[] {
                        "Public",
                        "",
                        "4",
                        "System.Console.WriteLine(string), mscorlib"});
#line 49
 testRunner.Then("the resulting report contains \'When an instance method is invoked from its base\'", ((string)(null)), table4, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
