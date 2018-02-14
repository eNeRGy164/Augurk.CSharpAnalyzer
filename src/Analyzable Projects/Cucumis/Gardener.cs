using System;

namespace Cucumis
{
    /// <summary>
    /// This class represents a basic gardener.
    /// </summary>
    public class Gardener : IGardener
    {
        /// <summary>
        /// Plant a plant.
        /// </summary>
        void IGardener.Plant()
        {
            Console.WriteLine("Gardener: I planted a plant, because I it was in my job description.");
        }

        /// <summary>
        /// Plant a Gherkin.
        /// </summary>
        /// <remarks>
        /// Planting a Gherkin is so much better than planting a plant.
        /// </remarks>
        public void PlantGherkin()
        {
            Console.WriteLine("Gardener: I just planted a gherkin!");
        }

        /// <summary>
        /// Water the plants.
        /// </summary>
        public void WaterPlants()
        {
            Console.WriteLine("Gardener: I just watered the plants!");
        }

        /// <summary>
        /// Harvests the provided Gherkin.
        /// </summary>
        public void HarvestGherkin(Gherkin gherkin)
        {
            Console.WriteLine("Gardener: I am about to harvest a gherkin!");
            gherkin.CutVine();
        }
    }
}
