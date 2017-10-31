using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
