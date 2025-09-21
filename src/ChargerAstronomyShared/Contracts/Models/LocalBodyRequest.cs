using ChargerAstronomyShared.Domain.Equatorial;
using ChargerAstronomyShared.Domain.Horizontal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChargerAstronomyShared.Contracts.Models
{
    public class LocalBodyContainer
    {
        /// <summary>
        /// A data structure for holding the local celestial bodies
        /// </summary>
        public HorizontalMoon moon { get; internal set; }
        public HorizontalSun sun { get; internal set; }
        public List<HorizontalPlanet> planets { get; internal set; }
        public LocalBodyContainer(HorizontalMoon newMoon, HorizontalSun newSun, List<HorizontalPlanet> newPlanets)
        {
            moon = newMoon;
            sun = newSun;
            planets = newPlanets;  
        }

    }

    public interface LocalBodyRequest
    {
        LocalBodyContainer celestialList {get; internal set; }

        public void requestUpdate();    // request for the values in the Local Body container to be updated

    }
}
