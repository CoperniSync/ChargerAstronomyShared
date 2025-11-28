using System;
using System.Collections.Generic;
using System.Text;


namespace ChargerAstronomyShared.Contracts.Models
{
    /// <summary>
    /// A rectangular region of the sky defined by right ascension and declination.
    /// </summary>
    public sealed class SkyRegion
    {
        /// <summary>
        /// Right ascension minimum in hours [0,24).
        /// </summary>
        public double RaMinHours { get; }

        /// <summary>
        /// Right ascension maximum in hours (0,24].
        /// </summary>
        public double RaMaxHours { get; }

        /// <summary>
        /// Declination minimum in degrees [-90,+90].
        /// </summary>
        public double DecMinDeg { get; }

        /// <summary>
        /// Declination maximum in degrees [-90,+90].
        /// </summary>
        public double DecMaxDeg { get; }

        /// <summary>
        /// Initializes a new <see cref="SkyRegion"/>, a rectangular region of the sky defined by right ascension and declination.
        /// </summary>
        /// <param name="raMinHours">The minimum right ascension (RA) boundary, in hours. Must be in the range [0, 24).</param>
        /// <param name="raMaxHours">The maximum right ascension (RA) boundary, in hours. Must be in the range (0, 24] and greater than or equal
        /// to <paramref name="raMinHours"/>.</param>
        /// <param name="decMinDeg">The minimum declination (Dec) boundary, in degrees. Must be in the range [-90, 90].</param>
        /// <param name="decMaxDeg">The maximum declination (Dec) boundary, in degrees. Must be in the range [-90, 90] and greater than or equal
        /// to <paramref name="decMinDeg"/>.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="raMinHours"/> is not in the range [0, 24), or if <paramref name="raMaxHours"/> is
        /// not in the range (0, 24]. Thrown if <paramref name="decMinDeg"/> or <paramref name="decMaxDeg"/> is not in
        /// the range [-90, 90].</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="raMinHours"/> is greater than <paramref name="raMaxHours"/>, or if <paramref
        /// name="decMinDeg"/> is greater than <paramref name="decMaxDeg"/>.</exception>
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
            DecMinDeg = decMinDeg;
            DecMaxDeg = decMaxDeg;
        }
    }
}
