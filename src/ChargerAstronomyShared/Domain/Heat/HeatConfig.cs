using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Domain.Heat
{
    public sealed class HeatConfig
    {
        public float TauSeconds = 1.5f;        // decay time constant
        public float ActiveThreshold = 0.6f;   // HOT if heat >= this
        public int MinWarmFrames = 10;       // this will be the number of grace frame before eviction
        public int MinColdFrames = 30;       // extra frames at 0 before evict signal
    }
}
