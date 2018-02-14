namespace Cucumis
{
    /// <summary>
    /// Defines extension methods for the <see cref="Gherkin"/> class.
    /// </summary>
    public static class GherkinExtensions
    {
        public static void Harvest(this Gherkin gherkin)
        {
            new Gardener().HarvestGherkin(gherkin);
        }
    }
}
