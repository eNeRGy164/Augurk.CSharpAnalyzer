using TechTalk.SpecFlow;

namespace Cucumis.Specifications.Steps
{
    [Binding]
    internal class ExtensionMethodSteps
    {
        [When("an extension method is invoked")]
        public void WhenAnExtensionMethodIsInvoked()
        {
            new Gherkin().Harvest();
        }

        [When("an extension method is invoked on a derived type")]
        public void WhenAnExtensionMethodIsInvokedOnADerivedType()
        {
            new PickyGherkin().Harvest();
        }
    }
}
