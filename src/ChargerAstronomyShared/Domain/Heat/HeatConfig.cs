using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Domain.Heat
{

    /// <summary>
    /// Represents the configuration settings for heat-related calculations, including decay rate, clamping limits, and
    /// precision thresholds.
    /// </summary>
    public sealed class HeatConfig
    {

        /// <summary>
        /// The rate at which heat decays per second.
        /// </summary>
        public float DecayPerSecond { get; set; } = 0.05f;

        /// <summary>
        /// The minimum heat value.
        /// </summary>
        public float ClampMin { get; set; } = 0f;

        /// <summary>
        /// The maximum heat value.
        /// </summary>
        public float ClampMax { get; set;  } = 1f;

        /// <summary>
        /// Gets the small positive value used as a tolerance for floating-point comparisons.
        /// </summary>
        /// <remarks>No need for setters, this is a constant.</remarks>
        public float Epsilon { get; } = 1e-6f;

        /// <summary>
        /// Indicates whether area weighting is applied in calculations.
        /// </summary>
        public bool AreaWeighted { get; set;  } = true;    
    }
}
