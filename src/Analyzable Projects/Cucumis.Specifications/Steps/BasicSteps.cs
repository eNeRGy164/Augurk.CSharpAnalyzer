using Cucumis.Specifications.Support;
using System;
using TechTalk.SpecFlow;

namespace Cucumis.Specifications.Steps
{
    [Binding]
    public sealed class BasicSteps
    {
        [When("entrypoint is invoked directly")]
        public void WhenEntrypointIsInvokedDirectly()
        {
            var gardener = new Gardener();
            gardener.PlantGherkin();
        }

        [When("entrypoint is surrounded by other invocations")]
        public void WhenEntrypointIsSurroundedByOtherInvocations()
        {
            Console.WriteLine("Invoking implementation");
            var gardener = new Gardener();
            gardener.PlantGherkin();
            ConsoleWriter.WriteDefaultMessage();
        }

        [When("two separate entrypoints are invoked")]
        public void WhenTwoEntrypointsAreInvoked()
        {
            var gardener = new Gardener();
            gardener.PlantGherkin();
            gardener.WaterPlants();
        }
    }
}
