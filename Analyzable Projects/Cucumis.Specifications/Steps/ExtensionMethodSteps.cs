using TechTalk.SpecFlow;

namespace Cucumis.Specifications.Steps
{
    [Binding]
    internal class ExtensionMethodSteps
    {
        [When("an extension method is invoked")]
        public void WhenAnExtensionMethodIsInvoked()
        {
            new PickyGherkin().Harvest();
        }
    }
}
