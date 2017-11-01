namespace Cucumis
{
    /// <summary>
    /// Represents the plant prototype.
    /// </summary>
    public abstract class Plant
    {
        /// <summary>
        /// Bloom very pretty, or something
        /// </summary>
        public void Bloom()
        {
            //After 10 minutes:
            Wither();
        }

        /// <summary>
        /// Eventually every plant Withers, in it's own way...
        /// </summary>
        public abstract void Wither();
    }
}
