using System;

namespace Cucumis
{
    /// <summary>
    /// Represtents a plant from the Melothria family
    /// </summary>
    public class Melothria : Plant
    {
        /// <summary>
        /// This plant doesn't wither, it rots.
        /// </summary>
        public override void Wither()
        {
            Rot();
        }

        private void Rot()
        {
            Console.WriteLine("Melothria: Oh no! I'm rotting! Save yourselves!");
        }
    }
}
