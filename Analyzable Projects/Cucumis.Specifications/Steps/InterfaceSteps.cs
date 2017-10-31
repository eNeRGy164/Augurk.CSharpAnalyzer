using Cucumis.Specifications.Support;
using TechTalk.SpecFlow;

namespace Cucumis.Specifications.Steps
{
    [Binding]
    internal sealed class InterfaceSteps
    {
        [When("entrypoint is an explicit interface implementation")]
        public void WhenEntrypointIsAnExplicitInterfaceImplementation()
        {
            IGardener gardener = new Gardener();
            gardener.Plant();
        }

        [When("entrypoint is an implicit interface implementation")]
        public void WhenEntrypointIsAnImplicitInterfaceImplementation()
        {
            IGardener gardener = new Gardener();
            gardener.WaterPlants();
        }

        [When("entrypoint is invoked after invocation on interface")]
        public void WhenEntrypointIsInvokedAfterInvocationOnInterface()
        {
            // First let the mocked gardener plant something
            IGardener gardener = new MockedGardener();
            gardener.Plant();

            // Then let the real gardener water it
            new Gardener().WaterPlants();

        }
    }
}
