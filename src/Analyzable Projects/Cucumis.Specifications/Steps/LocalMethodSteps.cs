using Cucumis.Specifications.Support;
using TechTalk.SpecFlow;

namespace Cucumis.Specifications.Steps
{
    [Binding]
    internal sealed class LocalMethodSteps
    {
        [When("a local method is called within the entrypoint")]
        public void WhenALocalMethodIsCalledWithinTheEntrypoint()
        {
            Gherkin gherkin = new Gherkin();
            // The OnWater will make a local call without the "this." identifier
            gherkin.OnWater(new WaterEventArgs());
        }

        [When("an explicit base method is called within the entrypoint")]
        public void WhenAnExplicitBaseMethodIsCalledWithinTheEntrypoint()
        {
            Gherkin gherkin = new PickyGherkin();
            // The OnWater will make a base call with the "base." identifier
            gherkin.OnWater(new WaterEventArgs());
        }

        [When("a local method is called within the entrypoint explicitly on this")]
        public void WhenALocalMethodIsCalledWithinTheEntrypointExplicitlyOnThis()
        {
            Gherkin gherkin = new Gherkin();
            gherkin.OnPlant(new PlantEventArgs());
        }
    }
}
