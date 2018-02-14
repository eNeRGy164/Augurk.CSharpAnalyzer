using System;

namespace Cucumis
{
    /// <summary>
    /// This class represents a Gherkin; the edible variety, not the written one.
    /// </summary>
    public class Gherkin
    {
        /// <summary>
        /// This methods get called when this Gherkin is watered.
        /// </summary>
        public virtual void OnWater(WaterEventArgs args)
        {
            Grow();
        }

        public void OnPlant(PlantEventArgs args)
        {
            this.SetInitialSize("Seed");
        }

        /// <summary>
        /// Grows the Gherkin.
        /// </summary>
        /// <remarks>
        /// It needs to grow so we can eventuall harvest it.
        /// </remarks>
        protected void Grow()
        {
            Console.WriteLine("Gherkin: I'm growing, weeee!");
        }

        internal virtual void CutVine()
        {
            Console.WriteLine("Gherkin: Someone just cut my vine! Goodbye cruel world!");
        }

        private void SetInitialSize(string size)
        {
            Console.WriteLine($"Gherkin: I'm size {size}");
        }
    }
}
