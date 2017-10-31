namespace Cucumis.Specifications.Support
{
    /// <summary>
    /// A gardener who inherited it's job from it's parent. 
    /// </summary>
    /// <remarks>
    /// Naturally, the child is more efficient.
    /// </remarks>
    class InheritedGardener : Gardener
    {
        /// <summary>
        /// Plants a Gherkin AND waters it!
        /// </summary>
        internal void PlantGherkinAndWaterIt()
        {
            PlantGherkin();
            WaterPlants();
        }
    }
}
