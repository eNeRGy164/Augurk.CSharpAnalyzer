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

        [When("entrypoint is invoked through an interface implementation")]
        public void WhenEntrypointIsInvokedThroughAnInterfaceImplementation()
        {
            // The mocked gardener implements an interface from the system
            // under test but the real entrypoint is the call it does on the
            // provided Gherkin.
            IGardener gardener = new MockedGardener();
            gardener.WaterPlants();
        }

        [When("this actually means that")]
        public void WhenThisActuallyMeansThat()
        {
            Plant plant = new Melothria();
            // The FreezeAndThaw method uses the this-operator
            // to reference an abstract method
            plant.FreezeAndThaw();
        }
    }
}
