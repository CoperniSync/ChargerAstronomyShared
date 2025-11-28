using System;
using System.Text;
using System.Collections.Generic;
using ChargerAstronomyShared.Domain.Equatorial;

namespace ChargerAstronomyShared.Domain.Horizontal
{
    /// <summary>
    /// Represents the moon in horizontal coordinate form.
    /// </summary>
    public class HorizontalMoon : HorizontalBody
    {
        /// <summary>
        /// Creates a new object by wrapping a <see cref="EquatorialCelestialBody"/>
        /// </summary>
        /// <param name="body">The <see cref="EquatorialCelestialBody"/> to base this object off of.</param>
        public HorizontalMoon() : base(BodyType.Moon)
        {
            // empty in old stargazer - jojo
        }

        /// <summary>
        /// A double representing the moon phase from 0° to 360°
        /// </summary>
        public double Phase { get; set; }

    }
}
