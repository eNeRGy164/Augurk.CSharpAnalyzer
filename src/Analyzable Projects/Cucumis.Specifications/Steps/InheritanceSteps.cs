﻿using Cucumis.Specifications.Support;
using TechTalk.SpecFlow;

namespace Cucumis.Specifications.Steps
{
    [Binding]
    internal sealed class InheritanceSteps
    {
        [When("entrypoint is invoked on inherited automation class")]
        public void WhenEntrypointIsInvokedOnInheritedAutomationClass()
        {
            // The InheritedGardener is defined within the test namespace
            new InheritedGardener().PlantGherkin();
        }

        [When("entrypoint is invoked through an inherited automation class")]
        public void WhenEntrypointIsInvokedThroughAnInheritedAutomationClass()
        {
            // The InheritedGardener is so much more efficient
            new InheritedGardener().PlantGherkinAndWaterIt();
        }

        [When("same method is invoked with different concrete types")]
        public void WhenSameMethodIsInvokedWithDifferentConcreteTypes()
        {
            var gardener = new Gardener();
            gardener.HarvestGherkin(new Gherkin());
            gardener.HarvestGherkin(new PickyGherkin());
        }

        [When("an instance method is invoked from its base")]
        public void WhenAnInstanceMethodIsInvokedFromItsBase()
        {
            // The Bloom method is only defined on the base
            new Melothria().Bloom();
        }

        [When("a base method is called from a far off generations")]
        public void WhenABaseMethodIsCalledFromFarOffGenerations()
        {
            Gherkin gherkin = new StubbornGherkin();
            gherkin.CutVine();
        }

        [When("an inherited instance method is invoked indirectly")]
        public void WhenAnInheritedInstanceMethodIsInvokedIndirectly()
        {
            PickyGherkin gherkin = new PickyGherkin();
            PrepareAndCutVine(gherkin);
        }

        private static void PrepareAndCutVine(Gherkin gherkin)
        {
            CutTheVine(gherkin);
        }

        private static void CutTheVine(Gherkin gherkin)
        {
            gherkin.CutVine();
        }
    }
}
