using Cucumis.Specifications.Support;
using TechTalk.SpecFlow;

namespace Cucumis.Specifications.Steps
{
    [Binding]
    internal sealed class DriverSteps
    {
        private readonly GardenerDriver _driver;

        public DriverSteps(GardenerDriver driver)
        {
            _driver = driver;
        }

        [When("entrypoint is invoked through a driver directly")]
        public void WhenEntrypointIsInvokedThroughADriverDirectly()
        {
            _driver.WaterPlants();
        }

        [When("entrypoint is indirectly invoked through a driver")]
        public void WhenEntrypointIsIndirectlyInvokedThroughADriver()
        {
            _driver.WaterPlantsIndirectly();
        }
    }
}
