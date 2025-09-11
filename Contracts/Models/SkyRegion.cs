using System;
using System.Collections.Generic;
using System.Text;


namespace ChargerAstronomyShared.Contracts.ContractsModels
{
    public sealed class SkyRegion
    {
        public double RaMinHours { get; }
        public double RaMaxHours { get; }
        public double DecMinDeg { get; }
        public double DecMaxDeg { get; }

        public SkyRegion(double raMinHours, double raMaxHours, double decMinDeg, double decMaxDeg)
        {

            if (raMinHours < 0.0 || raMinHours >= 24.0)
                throw new ArgumentOutOfRangeException(nameof(raMinHours), "RA min must be in [0,24).");

            if (raMaxHours <= 0.0 || raMaxHours > 24.0)
                throw new ArgumentOutOfRangeException(nameof(raMaxHours), "RA max must be in (0,24].");

            if (raMinHours > raMaxHours)
                throw new ArgumentException("raMinHours must be <= raMaxHours ", nameof(raMinHours));

            if (decMinDeg < -90.0 || decMinDeg > 90.0)
                throw new ArgumentOutOfRangeException(nameof(decMinDeg), "Dec min must be in [-90,+90].");

            if (decMaxDeg < -90.0 || decMaxDeg > 90.0)
                throw new ArgumentOutOfRangeException(nameof(decMaxDeg), "Dec max must be in [-90,+90].");

            if (decMinDeg > decMaxDeg)
                throw new ArgumentException("decMinDeg must be <= decMaxDeg.", nameof(decMinDeg));

            RaMinHours = raMinHours;
            RaMaxHours = raMaxHours;
            DecMinDeg  = decMinDeg;
            DecMaxDeg  = decMaxDeg;
        }
    }
}
