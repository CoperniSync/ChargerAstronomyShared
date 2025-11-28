using System;
using System.Text;
using System.Collections.Generic;
using ChargerAstronomyShared.Domain.Equatorial;

namespace ChargerAstronomyShared.Domain.Horizontal
{
    /// <summary>
    /// Represents an object that can be located in the sky according to horizontal coordinates.
    /// </summary>
    public abstract class HorizontalBody : EquatorialCelestialBody
    {
        /// <summary>
        /// Creates a new object by wrapping a <see cref="EquatorialCelestialBody"/>
        /// </summary>
        protected HorizontalBody(EquatorialCelestialBody body) : base(body.BodyType)
        {
            this.RightAscension = body.RightAscension;
            this.Declination = body.Declination;
            this.Distance = body.Distance;
            this.Magnitude = body.Magnitude;
        }

        protected HorizontalBody(BodyType bodyType) : base(bodyType)
        {
        }

        /// <summary>
        /// The angle in decimal degrees formed between the horizon and the star
        /// </summary>
        public double Altitude { get; set; }

        /// <summary>
        /// The angle in decimal degrees formed between due north and the star
        /// </summary>
        public double Azimuth { get; set; }

    }
}
