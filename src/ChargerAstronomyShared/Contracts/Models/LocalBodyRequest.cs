using ChargerAstronomyShared.Domain.Equatorial;
using ChargerAstronomyShared.Domain.Horizontal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Models
{
    /// <summary>
    /// A container for horizontal instances of local celestial bodies.
    /// </summary>
    public class LocalBodyContainer
    {
        /// <summary>
        /// A horizontal instance of the Moon.
        /// </summary>
        public HorizontalMoon moon { get; internal set; }

        /// <summary>
        /// A horizontal instance of the Sun.
        /// </summary>
        public HorizontalSun sun { get; internal set; }

        /// <summary>
        /// A list of horizontal planets instances.
        /// </summary>
        public List<HorizontalPlanet> planets { get; internal set; }

        /// <summary>
        /// Creates a new instance and sets celestial bodies.
        /// </summary>
        /// <param name="newMoon">The <see cref="HorizontalMoon"/> instance representing the Moon.</param>
        /// <param name="newSun">The <see cref="HorizontalSun"/> instance representing the Sun.</param>
        /// <param name="newPlanets">A list of <see cref="HorizontalPlanet"/> objects representing the planets.  This list must not be null.</param>
        public LocalBodyContainer(HorizontalMoon newMoon, HorizontalSun newSun, List<HorizontalPlanet> newPlanets)
        {
            moon = newMoon;
            sun = newSun;
            planets = newPlanets;  
        }

    }

    /// <summary>
    /// A request to update local body positions.
    /// </summary>
    public interface LocalBodyRequest
    {
        LocalBodyContainer celestialList {get; internal set; }

        public void requestUpdate();    // request for the values in the Local Body container to be updated

    }
}
