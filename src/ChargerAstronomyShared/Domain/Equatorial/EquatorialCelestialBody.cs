using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ChargerAstronomyShared.Domain.Equatorial
{
    //using ChargerAstronomyShared.Domain.Common;

    /// <summary>
    /// A celestial body represented in equatorial coordinates.
    /// </summary>
    public abstract class EquatorialCelestialBody
    {
        public EquatorialCelestialBody(BodyType body)
        {
            this.bodyType = body;
        }

        private BodyType bodyType;
        /// <summary>
        /// Measured in decimal hours from the point in the sky where the sun crosses the celestial equator during the spring equinox of 2000
        /// </summary>
        public double RightAscension { get; set; }
        /// <summary>
        /// A measure in degrees how far an object is north or south of the celestial equator during the spring equinox of 2000
        /// </summary>
        public double Declination { get; set; }
        /// <summary>
        /// A logarithmic representation of a star's brightness (negative values being brighter)
        /// </summary>
        public double Magnitude { get; set; }
        /// <summary>
        /// Distance in light years from the earth.
        /// </summary>
        public double Distance { get; set; }

        public BodyType BodyType => bodyType;
    }
}
