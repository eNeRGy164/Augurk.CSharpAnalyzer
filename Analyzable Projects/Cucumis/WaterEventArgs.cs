using System;

namespace Cucumis
{
    /// <summary>
    /// These are the EventArgs that would be provided when something is watered.
    /// </summary>
    public class WaterEventArgs:EventArgs
    {
        /// <summary>
        /// Indicates whether the water is acid free.
        /// </summary>
        public bool AcidFree { get; set; }
    }
}
