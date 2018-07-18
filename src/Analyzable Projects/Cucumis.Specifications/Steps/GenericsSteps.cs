using TechTalk.SpecFlow;

namespace Cucumis.Specifications.Steps
{
    [Binding]
    internal sealed class GenericsSteps
    {
        [When("a generic method is invoked")]
        public void WhenAGenericMethodIsInvoked()
        {
            Gardener gardener = new Gardener();
            gardener.Harvest(new Melothria());
        }
    }
}
