namespace Cucumis.Specifications.Support
{
    /// <summary>
    /// A driver to interact with the <see cref="Gardener"/>.
    /// </summary>
    /// <remarks>
    /// Wouldn't a Gardener be more likely to drive a lawnmower?
    /// </remarks>
    class GardenerDriver
    {
        private readonly Gardener _gardener;
        private readonly IGardener _iGardener;

        public GardenerDriver()
        {
            _gardener = new Gardener();
            _iGardener = _gardener;
        }

        public void WaterPlants()
        {
            // The garderner is a concrete type, as such it will be further explored
            _gardener.WaterPlants();
        }

        public void WaterPlantsIndirectly()
        {
            // The iGardener is only an interface, there is no way to find out which
            // concrete type has been put into it; as such it cannot be explored any
            // further
            _iGardener.WaterPlants();
        }
    }
}
