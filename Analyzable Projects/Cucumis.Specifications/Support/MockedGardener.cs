namespace Cucumis.Specifications.Support
{
    class MockedGardener : IGardener
    {
        public void Plant()
        {
            // Do nothing to limit test output length
        }

        public void WaterPlants()
        {
            new Gherkin().OnWater(new WaterEventArgs());
        }
    }
}
