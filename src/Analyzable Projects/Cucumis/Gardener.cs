using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Water a list of plants.
        /// </summary>
        /// <param name="plantsToWater">A list of plants to water.</param>
        internal void Water(IEnumerable<Plant> plantsToWater)
        {
            Console.WriteLine($"Gardener: I just watered {plantsToWater.Count()} plants.");
        }

        /// <summary>
        /// Water a specific plant.
        /// </summary>
        /// <param name="plantToWater">A specific plant to water.</param>
        internal void Water(Plant plantToWater)
        {
            Water(Enumerable.Repeat(plantToWater, 1));
        }

        /// <summary>
        /// Harvests the provided Gherkin.
        /// </summary>
        public void HarvestGherkin(Gherkin gherkin)
        {
            Console.WriteLine("Gardener: I am about to harvest a gherkin!");
            gherkin.CutVine();
        }

        /// <summary>
        /// Harvests the provided <paramref name="plant"/> of an arbitrary type.
        /// </summary>
        /// <typeparam name="T">Type of the plant to harvest.</typeparam>
        /// <param name="plant">A plant to harvest.</param>
        public void Harvest<T>(T plant)
            where T : Plant
        {
            Console.WriteLine($"Gardener: I am about to harvest a {typeof(T)}");
            plant.Prune();
        }
    }
}
