using System;
using System.Text;
using System.Collections.Generic;
using ChargerAstronomyShared.Domain.Equatorial;

namespace ChargerAstronomyShared.Domain.Horizontal
{
    /// <summary>
    /// A planet in our solar system in horizontal coordinate form.
    /// </summary>
    public class HorizontalPlanet : HorizontalBody
    {
        public double PhaseAngle { get; set; }
        public string Name { get; }

        /// <summary>
        /// Returns a new planet object
        /// </summary>
        /// <param name="name">The name of the planet</param>
        public HorizontalPlanet(BodyType body, string Name) : base(body)
        {
           this.Name = Name;
        }
    }
}
