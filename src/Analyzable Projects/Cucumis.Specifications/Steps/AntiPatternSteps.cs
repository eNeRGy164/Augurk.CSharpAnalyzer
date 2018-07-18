using Augurk.CSharpAnalyzer.Annotations;
using TechTalk.SpecFlow;

namespace Cucumis.Specifications.Steps
{
    [Binding]
    internal sealed class AntiPatternSteps
    {
        // This when step actually tests the logic of the Wither method on Plant but we cannot invoke it directly, thus we go through FreezeAndThaw
        [When("the automated code cannot be invoked directly")]
        [AutomationTarget(typeof(Melothria), nameof(Melothria.Wither))]
        public void WhenTheAutomatedCodeCannotBeInvokedDirectly()
        {
            // Withering a plant requires a lot of setup, thus it is easier to just invoke it directly
            Plant plant = new Melothria();
            plant.FreezeAndThaw();
        }

        // This when step actually tests the logic of the Water method on Gardener that takes a single Plant, but we cannot invoke it directly, thus we go through Water on Plant.
        [When("only the top level overload should match")]
        [AutomationTarget(typeof(Gardener), nameof(Gardener.Water), OverloadHandling.First)]
        public void WhenOnlyTheTopLevelOverloadShouldMatch()
        {
            Plant plant = new Melothria();
            plant.Water(new Gardener());
        }

        // This when step actually tests the logic of the Water method on Gardener that takes multiple Plants, but we cannot invoke it directly, thus we go through Water on Plant.
        [When("only the lowest level overload should match")]
        [AutomationTarget(typeof(Gardener), nameof(Gardener.Water), OverloadHandling.Last)]
        public void WhenOnlyTheLowestLevelOverloadShouldMatch()
        {
            // Watering multiple plants requires a lot of setup, thus it is easier to test with only 1 plant, but we know the actual implementation is in the lowest overload
            Plant plant = new Melothria();
            plant.Water(new Gardener());
        }

        // This when step actually tests the logic of the Water method on Gardener that takes multiple Plants, but we cannot invoke it directly, thus we go through Water on Plant.
        [When("all overloads should match")]
        [AutomationTarget(typeof(Gardener), nameof(Gardener.Water), OverloadHandling.All)]
        public void WhenAllOverloadsShouldMatch()
        {
            // Watering multiple plants requires a lot of setup, thus it is easier to test with only 1 plant, but we know the actual implementation is in the lowest overload
            Plant plant = new Melothria();
            plant.Water(new Gardener());
        }
    }
}
