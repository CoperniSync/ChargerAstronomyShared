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
        public float DecayPerSecond { get; set; } = 0.05f;
        public float ClampMin { get; set; } = 0f;
        public float ClampMax { get; set;  } = 1f;
        public float Epsilon { get; } = 1e-6f; // no need to have setters for this
        public bool AreaWeighted { get; set;  } = true;    
    }
}
