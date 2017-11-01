namespace Cucumis
{
    /// <summary>
    /// This class represents a Gherkin; the edible variety, not the written one.
    /// </summary>
    public class PickyGherkin : Gherkin
    {
        /// <summary>
        /// This methods get called when this Gherkin is watered.
        /// </summary>
        public override void OnWater(WaterEventArgs args)
        {
            if (args.AcidFree)
            {
                // The explicit mention of base is relevant for the test!
                base.Grow();
            }
        }
    }
}
