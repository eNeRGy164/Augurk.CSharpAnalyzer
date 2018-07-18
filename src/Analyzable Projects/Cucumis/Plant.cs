using System.Threading.Tasks;

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
        /// It's freezing cold out there, so I'm gonna die!
        /// </summary>
        public void FreezeAndThaw()
        {
            //Not really frost-resistant, unless proven otherwise
            this.Wither();
        }

        /// <summary>
        /// Water me!
        /// </summary>
        /// <param name="gardener">A gardener that can do the wattering.</param>
        public void Water(Gardener gardener)
        {
            gardener.Water(this);
        }

        /// <summary>
        /// Eventually every plant Withers, in it's own way...
        /// </summary>
        public abstract void Wither();

        /// <summary>
        /// Procreation for a plant is slooooow, better make it asynchronous...
        /// </summary>
        public virtual async Task Procreate()
        {
            //Lets start this futile task, as it will probably get plucked before its gets to it
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// Prunes this plant.
        /// </summary>
        public virtual void Prune()
        {
            // Default implementation does nothing
        }
    }
}
